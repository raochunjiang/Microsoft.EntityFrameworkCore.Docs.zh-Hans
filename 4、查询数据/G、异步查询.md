# 异步查询

异步查询能够在数据库执行查询时避免阻塞线程。这对于避免冻结胖客户端应用程序（thick-client application）的 UI 来说很有用。异步操作还能够提升 Web 应用程序的生产能力，在数据库执行查询时线程可以被空出来为其他请求服务。更多信息请查阅 [C#异步编程](https://docs.microsoft.com/zh-cn/dotnet/csharp/async)。

> 警告
>
> EF Core 不支持在同一个上下文实例上运行多并行操作。应该总是在下一个操作开始之前等待上一个操作的完成。这通常是在每个异步操作上使用 `await` 关键字来实现的。

Entity Framework Core 提供了一组用于触发执行查询和返回数的异步扩展方法来替代 LINQ 方法。样例包括 `ToListAsync()`、`ToArrayAsync()`、`SingleAsync()` 等等。而类似于 `Where(...)`、`OrderBy` 等等方法则没有相应的异步版本，因为这些方法只是用来构建表达式树，不会引发执行数据库查询。

> 重要提示
>
> EF Core 异步扩展方法是在 `Microsoft.EntityFrameworkCore` 命名空间下定义的，必须引入该命名空间才能使用这些扩展方法。

```C#
public async Task<List<Blog>> GetBlogsAsync()
{
    using (var context = new BloggingContext())
    {
        return await context.Blogs.ToListAsync();
    }
}
```