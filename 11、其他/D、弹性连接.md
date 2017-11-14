# 弹性链接

弹性链接会在数据库命令失败时自动重试。通过提供封装了故障检测和命令重试所需逻辑的“执行策略”,该功能可以应用于任何数据库。EF Core 提供程序能够根据特定的数据库故障条件和最优重试策略来提供执行策略。

比如说，SQL Server 提供程序包含一个特定的针对 SQL Server（包括 SQL Azure）的执行策略。它很清楚可以被重试的异常类型、具有合理的默认最大尝试次数、合理的两次重试之间的默认延迟等等。

在为上下文实例配置相关选项的时候就可以指定一个执行策略。这通常是在你派生上下文的 `OnConfiguring` 方法中完成，对于 ASP.NET Core 应用程序则在 `Startup.cs` 中完成。

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFMiscellanous.ConnectionResiliency;Trusted_Connection=True;",
            options => options.EnableRetryOnFailure());
}
```

## 自定义执行策略

这里提供了自定义执行策略的注册机制，如果你想要更改默认的策略，就可以使用它。

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseMyProvider(
            "<connection string>",
            options => options.ExecutionStrategy(...));
}
```

## 执行策略与事务


