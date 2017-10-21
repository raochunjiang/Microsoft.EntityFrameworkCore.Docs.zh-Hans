# 加载关联数据

Entity Framework Core 允许你在模型中使用导航属性来下载关联的实体。加载关联数据有三种通用的 O/RM 模式。

* **贪婪加载** 意味着关联数据会作为初始查询的一部分从数据库加载
* **显式加载** 意味着关联数据会在迟些时候从数据库显式加载
* **延迟加载** 意味着关联数据的加载是透明的，当访问导航属性的时候其数据才从数据库加载。**EF Core 还不能使用延迟加载**

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Querying)。

## 贪婪加载

可以使用 `Include` 方法来指定被包含在查询结果中的关联数据。在以下代码样例中，从结果集返回的 blogs 将包含由 posts 数据填充的 `Posts` 属性值。

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
        .ToList();
}
```

> 提示
>
> Entity Framework Core 会自动将导航属性装配为之前已加载到上下文实例中的实体。所以即便你没有显式为导航属性包含数据，该属性仍然可能被填充，因为一些或所有关联实体在之前已经加载过。

可以在单一查询中包含多个关系的关联数据。

```C#
using(var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
        .Include(blog => blog.Owner)
        .ToList();
}
```

### 多级包含

可以使用 `ThenInclude` 方法来在关系中钻取以包含多个级别的关联数据。以下代码样例会加载所有 blog 和他们关联的 post，以及每个 post 的 author。

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
            .ThenInclude(post => post.Author)
        .ToList();
}
```

可以连接多个 `ThenInclude` 来包含更深层次的关联数据。

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
            .ThenInclude(post => post.Author)
                .ThenInclude(author => author.Photo)
        .ToList();
}
```

可以组合使用上述方法，这样就可以在一个查询中包含多个根和多个级别的关联数据。

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
            .ThenInclude(post => post.Author)
            .ThenInclude(author => author.Photo)
        .Include(blog => blog.Owner)
            .ThenInclude(owner => owner.Photo)
        .ToList();
}
```

你可能想要为某个已包含的实体包含多个关联实体。比如，查询 `Blog` 的时候包含了 `Posts`，然后你想要将 `Posts` 的 `Author` 和 `Tags` 都包含进来。这时你需要为它们分别从根开始指定包含路径。比如，`Blog -> Posts -> Author` 和 `Blog -> Posts -> Tags`。这并不会造成冗余的连接查询，大部分情况下在生成 SQL 时 EF 会将它们合并。

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
            .ThenInclude(post => post.Author)
        .Include(blog => blog.Posts)
            .ThenInclude(post => post.Tags)
        .ToList();
}
```

### 忽略包含

当一个查询变更为不再返回其开始指定的实体类型的实例时，包含操作会被忽略。

在以下代码样例中，包含操作是基于 `Blog` 的，但时 `Select` 操作将查询更改成了返回匿名类型的数据。这种情况下包含操作时没有效果的。

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Include(blog => blog.Posts)
        .Select(blog => new
        {
            Id = blog.BlogId,
            Url = blog.Url
        })
        .ToList();
}
```

默认情况下，当包含操作被忽略时 EF Core 会输出一个警告。查看 [日志记录](../11、其他/C、日志记录.md)  以了解查看日志输出的更多信息。你可以更改这个行为以在一个包含操作被忽略时抛出异常或做其他事情。这可以在装配上下文实例的选项时完成 - 通常在 `DbContext.OnConfiguring` 或者 `Startup.cs`里面（如果你使用的是 ASP.NET Core）。

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFQuerying;Trusted_Connection=True;")
        .ConfigureWarnings(warnings => warnings.Throw(CoreEventId.IncludeIgnoredWarning));
}
```

## 显式加载

> 注意
>
> 该功能是从 EF Core 1.1 开始引入的。

可以使用 `DbContext.Entry(...)` 来显式加载导航属性。

```C#
using( var context = new BloggingContext())
{
    var blog = context.Blogs
        .Single(b => b.BlogId ==1);
    
    context.Entry(blog)
        .Collection(b => b.Posts)
        .Load();

    context.Entry(bolg)
        .Reference(b => b.Owner)
        .Load();
}
```

还可以通过执行独立的返回关联实体的查询来显式加载导航属性。如果启用了变更跟踪，那么在加载实体的时候 EF Core 将会自动设置新加载实体的导航属性以引用到任何以加载的实体。对于已加载的实体，则自动设置其属性为引用到新加载的实体。

### 查询关联实体

还可以（通过 `Query()` 方法）获得表示导航属性内容的 LINQ 查询。

这允许你在不加载关联实体到内存中的情况下做一些事情，比如说进行聚合运算。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs
        .Single(b => b.BlogId == 1);

    var postCount = context.Entry(blog)
        .Collection(b => b.Posts)
        .Query()
        .Count();
}
```

还可以进行筛选（过滤）以确定只加载哪些关联实体到内存中。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs
        .Single(b => b.BlogId == 1);

    var goodPosts = context.Entry(blog)
        .Collection(b => b.Posts)
        .Query()
        .Where(p => p.Rating > 3)
        .ToList();
}
```

## 延迟加载

EF Core 还不支持延迟加载。你可以查看我们积压工作中的 [延迟加载](https://github.com/aspnet/EntityFrameworkCore/issues/3797) 项以跟进这一功能。

## 关联数据和序列化

因为 EF Core 会自动装配导航属性，所以你的对象图可能会以循环的方式结束。比如说，加载 blog 及其关联的 post 将导致 blog 引用一个 post 的集合，集合中的每个 post 又回过来引用这个 blog。

一些序列化框架是不允许这样的循环的。例如 `Json.NET`，它会在遇到循环引用时抛出以下异常。

> Newtonsoft.Json.JsonSerializationException: Self referencing loop detected for property 'Blog' with type 'MyApplication.Models.Blog'.

如果你正在使用 ASP.NET Core，你可以配置 `Json.NET` 以忽略其在对象图中发现的循环引用。

```C#
public void ConfigureServices(IServiceCollection services)
{
    ...

    services.AddMvc()
        .AddJsonOptions(
            options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

    ...
}
```