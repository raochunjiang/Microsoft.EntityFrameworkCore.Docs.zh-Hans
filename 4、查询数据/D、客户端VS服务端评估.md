# 客户端 VS 服务端评估

Entity Framework Core 支持在客户端评估查询的一部分，同时也将其一部分推送给服务端（数据库）评估。数据库提供程序会决定查询的哪些部分交由数据库评估。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Querying)。

## 客户端评估

接下来的代码样例使用了一个帮助器方法来为从SQL Server 数据库返回的 blog 标准化 URL。因为 SQL Server 提供程序不了解该方法的具体实现，不可能将其翻译为 SQL。查询的其他方面都会在数据库服务器上进行评估，而返回并通过该方法传递的 `URL` 则在客户端进行评估。

```C#
var blogs = context.Blogs
    .OrderByDescending(blog => blog.Rating)
    .Select(blog => new
    {
        Id = blog.BlogId,
        Url = StandardizeUrl(blog.Url)
    })
    .ToList();
```

```C#
public static string StandardizeUrl(string url)
{
    url = url.ToLower();

    if (!url.StartsWith("http://"))
    {
        url = string.Concat("http://", url);
    }

    return url;
}
```

## 禁用客户端评估

尽管客户端评估的用处很大，但在一些实例中它可能导致很差的性能。考虑一下下面的查询，帮助器方法被用在了过滤器上。由于方法无法在数据库上执行，所有数据都将被拉取到内存中，然后在客户端应用过滤。基于总数据量，还有被过滤排除的数据量，就可能导致非常严重的性能问题。

```C#
var blogs = context.Blogs
    .Where(blog => StandardizeUrl(blog.Url).Contains("dotnet"))
    .ToList();
```

默认情况下，EF Core 会在执行客户端评估时输出警告。查看 [日志记录](../11、其他/C、日志记录.md)  以了解查看日志输出的更多信息。你可以更改这个行为以在发生客户端评估时抛出异常或做其他事情。这可以在装配上下文实例的选项时完成 - 通常在 `DbContext.OnConfiguring` 或者 `Startup.cs`里面（如果你使用的是 ASP.NET Core）。

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFQuerying;Trusted_Connection=True;")
        .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
}
```