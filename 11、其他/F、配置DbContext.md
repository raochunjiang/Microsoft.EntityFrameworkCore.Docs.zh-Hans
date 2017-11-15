# 配置 DbContext

本文展示的是使用 `DbContextOptions` 配置 `DbContext` 的模式。选项集（Options）主要是用来选择和配置数据存储的。

## 配置 DbContextOptions

`DbContext` 必须具有一个 `DbContextOptions` 的实例才能执行。这可以通过重写 `OnConfiguring` 来配置，或者通过构造方法参数由外部提供。

如果同时使用了两种方式，`OnConfiguring` 会基于已提供的选项集（options）被执行，这意味着 `OnConfiguring` 是成了附加的过程，可以在其中重写由构造方法参数获取的选项集。

### 构造方法参数

带构造方法的上下文类型代码:

```C#
public class BloggingContext : DbContext
{
    public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
    { }

    public DbSet<Blog> Blogs { get; set; }
}
```

> 提示
>
> DbContext 的基础构造方法还接受非泛型版本的 `DbContextOptions`。对于具有多个上下文类型的应用程序，不建议使用该非泛型版本。

从构造方法参数初始化上下文实例的应用程序代码:

```C#
var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
optionsBuilder.UseSqlite("Data Source=blog.db");

using (var context = new BloggingContext(optionsBuilder.Options))
{
  // 业务代码
}
```

> 警告
>
> `OnConfiguring` 比较迟被调用，因此可以在其中覆盖从DI或构造函数获得的选项值。该方法不适合用来测试（除非指向完整的数据库）。

带 `OnConfiguring` 的上下文类型代码：

```C#
public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=blog.db");
    }
}
```

从初始化上下文实例的应用程序代码:

```C#
using (var context = new BloggingContext())
{
  // 业务代码
}
```

## 通过依赖注入使用 DbContext

EF 支持通过依赖注入容器来使用 `DbContext`。可以使用 `AddDbContext<TContext>` 将你的 DbContext 类型添加到服务容器。

`AddDbContext` 将使得 DbContext 类型、`TContext` 以及 `DbContextOptions<TContext>` 都可以从服务容器注入获得。

查看后面的 [阅读更多](#阅读更多) 可以深入了解依赖注入。

将 DbConext 添加到依赖注入：

```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<BloggingContext>(options => options.UseSqlite("Data Source=blog.db"));
}
```

这需要向 DbContext 类型中添加接受 `DbContextOptions` 类型的参数的构造方法。

上下文类型：

```C#
public class BloggingContext : DbContext
{
    public BloggingContext(DbContextOptions<BloggingContext> options)
      :base(options)
    { }

    public DbSet<Blog> Blogs { get; set; }
}
```

（ASP.NET Core 中的）应用程序代码：

```C#
public MyController(BloggingContext context)
```

（直接使用 ServiceProvider 的）应用程序代码：

```C#
using (var context = serviceProvider.GetService<BloggingContext>())
{
  // do stuff
}

var options = serviceProvider.GetService<DbContextOptions<BloggingContext>>();
```

## 使用 `IDesignTimeDbContextFactory<TContext>`

作为上述选项的替代，你还可以提供一个 `IDesignTimeDbContextFactory<TContext>` 的实现。EF 工具集能够使用该工厂来创建 DbContext 的实例。这可能是实现设计时特定的体验的要求，比如说数据迁移。

实现这个接口能够为没有公共默认构造方法的上下文类型启用设计时服务。设计时服务将自动发现与上下文实例同一程序集的该接口的实现。

例如：

```C#
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MyProject
{
    public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
    {
        public BloggingContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
            optionsBuilder.UseSqlite("Data Source=blog.db");

            return new BloggingContext(optionsBuilder.Options);
        }
    }
}
```

## 阅读更多

* 阅读 [ASP.NET Core 入门](../2、入门指南/E、ASP.NETCore/A、ASP.NETCore.md) 以学习如何在 ASP.NET Core 中使用 EF。
* 阅读 [依赖注入](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection) 以深入学习如何使用 DI。
* 阅读 [测试](./E、测试/A、测试.md) 以了解更多信息。