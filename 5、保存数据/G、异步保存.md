# 异步保存

异步保存能够在将变更写入到数据库时避免线程阻塞。这对于避免冻结胖客户端应用程序（thick-client application）的 UI 来说很有用。异步操作还能够提升 Web 应用程序的生产能力，在数据库执行查询时线程可以被空出来为其他请求服务。更多信息请查阅 [C#异步编程](https://docs.microsoft.com/zh-cn/dotnet/csharp/async)。

> 警告
>
> EF Core 不支持在同一个上下文实例上运行多并行操作。应该总是在下一个操作开始之前等待上一个操作的完成。这通常是在每个异步操作上使用 `await` 关键字来实现的。

Entity Framework Core 提供了 `DbContext.SaveChangesAsync()` 来作为 `DbContext.SaveChanges()` 的异步替代。

> 重要提示
>
> EF Core 异步扩展方法是在 `Microsoft.EntityFrameworkCore` 命名空间下定义的，必须引入该命名空间才能使用这些扩展方法。

```C#
public static async Task AddBlogAsync(string url)
{
    using (var context = new BloggingContext())
    {
        var blog = new Blog { Url = url };
        context.Blogs.Add(blog);
        await context.SaveChangesAsync();
    }
}
```