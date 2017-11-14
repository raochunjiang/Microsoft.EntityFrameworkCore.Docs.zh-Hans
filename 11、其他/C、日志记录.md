# 日志记录

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Miscellaneous/Logging)。

## ASP.NET Core 应用程序

一旦使用了 `AddDbContext` 或 `AddDbContextPool` ，EF Core 就会自动集成 ASP.NET Core 的日志记录机制。因此，当使用 ASP.NET Core 的时候，日志记录的配置与 [ASP.NET Core 帮助文档](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging?tabs=aspnetcore2x) 中所描述的是一致的。

## 其他应用程序

EF Core 日志记录目前需要一个 ILoggerFactory，其自身配置了一个或多个 ILoggerProvider。通用提供程序是随以下程序包一起发布的：

* [Microsoft.Extensions.Logging.Console](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Console/)：简单的控制台日志记录器。
* [Microsoft.Extensions.Logging.AzureAppServices](https://www.nuget.org/packages/Microsoft.Extensions.Logging.AzureAppServices/)：支持 Azure 应用程序服务的 “诊断日志” 和 “日志流” 功能。
* [Microsoft.Extensions.Logging.Debug](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Debug/)：使用 System.Diagnostics.Debug.WriteLine() 将日志记录到调试监视器。
* [Microsoft.Extensions.Logging.EventLog](https://www.nuget.org/packages/Microsoft.Extensions.Logging.EventLog/)：记录到 Windows 事件日志。
* [Microsoft.Extensions.Logging.EventSource](https://www.nuget.org/packages/Microsoft.Extensions.Logging.EventSource/)：支持 EventSource/EventListener。
* [Microsoft.Extensions.Logging.TraceSource](https://www.nuget.org/packages/Microsoft.Extensions.Logging.TraceSource/)：使用 System.Diagnostics.TraceSource.TraceEvent() 将日志记录到跟踪监视器。

安装合适的程序包后，应用程序应该创建单一的/全局的 LoggerFactory 实例。例如，使用控制台记录日志：

```C#
public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] {new ConsoleLoggerProvider((_,__) => true,true)});
```

然后这个单一的/全局的实例应该通过 `DbContextOptionsBuilder` 注册到 EF Core。例如：

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
        .UseLoggerFactory(MyLoggerFactory) // Warning: Do not create a new ILoggerFactory instance each time
        .UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFLogging;Trusted_Connection=True;ConnectRetryCount=0");
```

> 警告
>
> 应用程序应该避免为每个上下文实例都创建新的 ILoggerFactory 实例，这很重要。否则会导致内存泄露、性能低下。

## 日志过滤

最简单的日志过滤方法就是在注册 ILoggerProvider 的时候对其进行配置。例如：

```C#
public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[]
    {
        new ConsoleLoggerProvider((category, level)
            => category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Information, true)
    });
```

在该样例中，过滤后的日志仅包含这样的信息：

* 属于 `Microsoft.EntityFrameworkCore.Database.Command` 分类的
* 等级为 `Information` 的

对于 EF Core，日志记录器分类是在 `DbLoggerCategory` 类型中定义的，主要是为了便于查找分类，但是都解析为简单字符串。

关于底层日志基础设施的详细内容请查看 [ASP.NET Core 日志帮助文档](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/logging?tabs=aspnetcore2x)。