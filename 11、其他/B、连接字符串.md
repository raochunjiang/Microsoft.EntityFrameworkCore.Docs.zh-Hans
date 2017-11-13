# 连接字符串

大部分数据库提供程序都需要一些固定格式的连接字符串来连接到数据库。有时候连接字符串会包含需要被保护的敏感信息。你可能还需要在应用程序在不同环境之间切换时更改连接字符串，不如开发环境、测试环境和生产环境。

## .NET Framework 应用程序

像 Winform、WPF、控制台应用程序以及 ASP.NET 4 等的 .NET Framework 应用程序都会有尝试（tried）连接字符串和测试（tested）连接字符串模式。连接字符串应该添加到应用程序的 App.config 文件中（对于 ASP.NET 则对应 Web.config 文件）。如果你的连接字符串包含敏感信息，比如用户名和密码，那么可以使用 [受保护的配置](https://docs.microsoft.com/dotnet/framework/data/adonet/connection-strings-and-configuration-files#encrypting-configuration-file-sections-using-protected-configuration) 来保护配置文件的内容。

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>

  <connectionStrings>
    <add name="BloggingDatabase"
         connectionString="Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;" />
  </connectionStrings>
</configuration>
```

> 提示
>
> 对于保存在 App.config 文件里的 EF Core 连接字符串中，`ProviderName` 配置不是必须的，因为数据库提供程序是通过代码来配置的。

然后就可以在上下文实例的`OnConfiguring` 方法中使用 `ConfigurationManager` 来读取连接字符串了。需要添加 `System.Configuration` 框架程序集的引用才能使用这个 API。

```C#
public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
    }
}
```

## Windows 通用平台（Universal Windows Platform，UWP）

UWP 应用程序中的连接字符串通常是指向本地文件的 SQLite 连接，通常都不会包含敏感信息，在应用程序发布后也无需变更连接字符串。因此将这些连接字符串放在代码中也无妨，像下面这样。如果你希望将它们放到代码之外，UWP 也是支持配置的概念的，详见 [UWP 帮助文档中的应用程序配置节](https://docs.microsoft.com/windows/uwp/app-settings/store-and-retrieve-app-data)。

```C#
public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
    }
}
```

## ASP.NET Core

 ASP.NET Core 中的配置系统非常灵活，连接字符串可以存储在 `appsettings.json` 、环境变量、用户私密存储或者其他配置源中，详见 [ASP.NET Core 帮助文档中的配置节](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration?tabs=basicconfiguration)。一下样例展示了存储在 `appsettings.json` 中的连接字符串。

 ```json
 {
  "ConnectionStrings": {
    "BloggingDatabase": "Server=(localdb)\\mssqllocaldb;Database=EFGetStarted.ConsoleApp.NewDb;Trusted_Connection=True;"
  },
}
 ```

 上下文通常是在 `Startup.cs` 中使用从配置中读取的连接字符串来配置的。注意 `GetConnectionString()` 方法会查找键值为 `ConnectionStrings:<连接字符串名称>` 的配置值。

 ```C#
 public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<BloggingContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("BloggingDatabase")));
}
 ```