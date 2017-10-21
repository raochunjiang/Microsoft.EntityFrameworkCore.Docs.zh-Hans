# 跟踪 VS 不跟踪

跟踪行为控制着 Entity Framework Core 是否会在其变更跟踪器里维持实体实例的信息。如果实体是被跟踪的，任何检测到的该实体的变更都将在 `SaveChanges()` 时持久化到数据库中。Entity Framework Core 还会对已跟踪的、之前已加载到 DbContext 实例中的查询和实体进行相互的导航属性装配。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Querying)。

## 跟踪查询

默认情况下，返回实体类型实例的查询是被跟踪的。这意味着你可以对这些实体进行更改，并通过 `SaveChanges()` 将这些变更持久化到数据库中。

在以下代码样例中，对 blog.Rating 的变更会在 `SaveChanges()` 时被检测并保存到数据库中。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs.SingleOrDefault(b => b.BlogId == 1);
    blog.Rating = 5;
    context.SaveChanges();
}
```

## 不跟踪查询

在查询结果为只读的场景下，不跟踪查询是很有用的。由于无需建立变更跟踪信息，查询会执行得更快。

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .AsNoTracking()
        .ToList();
}
```

还可以更改上下文级别上的默认跟踪行为。

```C#
using (var context = new BloggingContext())
{
    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

    var blogs = context.Blogs.ToList();
}
```

> 注意
>
> 不跟踪查询仍然会执行标识识别。如果结果集包含多个相同的实体，那么它们都将返回该实体类型的同一个实例。无论如何，弱引用会被用来保持对已返回实体的跟踪。如果之前具有同一标识的结果超出了作用范围，GC 会对其进行回收，你可能会获得新的实体实例。更多信息请查看 [查询的原理](./H、查询的原理.md)。

## 跟踪和推测

即便查询的结果类型不是一个实体类型，只要结果包含了实体类型，默认情况下它们还是被跟踪的。以下查询返回的是匿名类型数据，结果集中引用的 `Blog` 实例仍然是被跟踪的。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs
        .Select(b =>
            new
            {
                Blog = b,
                Posts = b.Posts.Count()
            });
}
```

如果结果集不包含任何实体类型，就不会执行任何跟踪。以下查询返回的是匿名类型数据，并且匿名类型实例只使用了实体上的一些值（没有引用实际的实体类型实例），EF Core 不会执行任何跟踪。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs
        .Select(b =>
            new
            {
                Id = b.BlogId,
                Url = b.Url
            });
}
```