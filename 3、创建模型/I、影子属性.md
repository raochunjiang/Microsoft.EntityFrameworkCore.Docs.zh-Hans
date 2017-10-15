# 影子属性

影子属性是 EF Core 模型中在 .NET 实体类型之外定义的实体类型属性。这些属性的值和状态只在变更跟踪器（`ChangeTracker`）中维护。

影子属性的值可以通过 `ChangeTracker` API 来获取和变更。

```C#
    context.Entry(myBlog).Property("LastUpdated").CurrentValue = DateTime.Now;
```

影子属性也可以在 LINQ 查询中通过静态方法 `EF.Property` 来引用。

```C#
var blogs = context.Blogs.OrderBy(b => EF.Property<DateTime>(b, "LastUpdated"));
```

## 惯例

当存在一个关系，但其中的依赖实体类型不存在相应的外键属性时，相应的影子属性就会被惯例创建。这种情况下就引入了影子外键属性。影子外键属性将被命名为 `<导航属性名><主键属性名>`（使用依赖实体类型上指向主实体类型的导航属性来命名）。如果主键属性名本身包含导航属性名，则影子属性名只会是 `<主键属性名>`。如果依赖实体类型中没有定义相应的导航属性，则改用主类型名称。

例如，以下列出的代码将产生一个引入给 `Post` 实体的 `BlogId` 影子属性。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; set; }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Blog Blog { get; set; }
}
```

## 数据注解

影子属性无法通过数据注解来创建。

## 流式 API

可以使用流式 API 来配置影子属性。在你调用了 `Property` 方法的字符串参数重载后，你可以通过链式调用对任何其他属性进行配置。

如果提供给 `Property` 方法的名称与现有属性的名称（影子属性名称或实体类型上定义的属性名称）匹配，那么代码配置的就是现有属性，而不是引入新的影子属性。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property<DateTime>("LastUpdated");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```