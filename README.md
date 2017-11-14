# Microsoft.EntityFrameworkCore.Docs.zh-Hans

微软 EntityFrameworkCore 中文文档

## 大纲

* [1、EF Core 2.0 新特性](./1、EFCore2.0新特性/A、EFCore2.0新特性.md)
    * [EF Core 1.0(以前的版本)](./1、EFCore2.0新特性/B、EFCore1.0（以前的版本）.md)
    * [EF Core 1.1(以前的版本)](./1、EFCore2.0新特性/C、EFCore1.1（以前的版本）.md)
* [2、入门指南](./2、入门指南/A、入门指南.md)
    * [安装 EF Core](./2、入门指南/B、安装EFCore.md)
    * [.NET Framework（Console、WinForm、WPF等等）](./2、入门指南/C、.NETFramework/A、.NETFramework.md)
        * [.NET Framework - 新数据库](./2、入门指南/C、.NETFramework/B、新数据库.md)
        * [.NET Framework - 现有数据库](./2、入门指南/C、.NETFramework/C、现有数据库.md)
    * [.NET Core（Windows、OSX、Linux 等等）](./2、入门指南/D、.NETCore/A、.NETCore.md)
        * [.NET Core - 新数据库](./2、入门指南/D、.NETCore/B、新数据库.md)
    * [ASP.NET Core](./2、入门指南/E、ASP.NETCore/A、ASP.NETCore.md)
        * [ASP.NET Core - 新数据库](./2、入门指南/E、ASP.NETCore/B、新数据库.md)
        * [ASP.NET Core - 现有数据库](./2、入门指南/E、ASP.NETCore/C、现有数据库.md)
        * [ASP.NET Core 网站上的 EF Core 教程](https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-mvc/intro)
    * [通用 Windows 平台（UWP）](./2、入门指南/F、通用Windows平台（UWP）/A、通用Windows平台（UWP）.md)
        * [UWP - 新数据库](./2、入门指南/F、通用Windows平台（UWP）//B、新数据库.md)
* [3、创建模型](./3、创建模型/A、创建模型.md)
    * [包含和排除类型](./3、创建模型/B、包含和排除类型.md)
    * [包含和排除属性](./3、创建模型/C、包含和排除属性.md)
    * [键（主键）](./3、创建模型/D、键（主键）.md)
    * [生成值](./3、创建模型/E、生成值.md)
    * [必须/可选属性](./3、创建模型/F、必须的和可选的属性.md)
    * [最大长度](./3、创建模型/G、最大长度.md)
    * [并发标记](./3、创建模型/H、并发标记.md)
    * [影子属性](./3、创建模型/I、影子属性.md)
    * [关系](./3、创建模型/J、关系.md)
    * [索引](./3、创建模型/K、索引.md)
    * [替代键（备用关键字）](./3、创建模型/L、替代键（备用关键字）.md)
    * [继承](./3、创建模型/M、继承.md)
    * [支持字段](./3、创建模型/N、支持字段.md)
    * [同一DbContext中的模型交替](./3、创建模型/O、同一DbContext中的模型交替.md)
    * [关系数据库建模](./3、创建模型/P、关系数据库建模/A、关系数据库建模.md)
        * [数据表映射](./3、创建模型/P、关系数据库建模/B、数据表映射.md)
        * [数据列映射](./3、创建模型/P、关系数据库建模/C、数据列映射.md)
        * [数据类型](./3、创建模型/P、关系数据库建模/D、数据类型.md)
        * [主键](./3、创建模型/P、关系数据库建模/E、主键.md)
        * [默认模式](./3、创建模型/P、关系数据库建模/F、默认模式.md)
        * [计算列](./3、创建模型/P、关系数据库建模/G、计算列.md)
        * [序列](./3、创建模型/P、关系数据库建模/H、序列.md)
        * [默认值](./3、创建模型/P、关系数据库建模/I、默认值.md)
        * [索引](./3、创建模型/P、关系数据库建模/J、索引.md)
        * [外键约束](./3、创建模型/P、关系数据库建模/K、外键约束.md)
        * [替代键（唯一约束）](./3、创建模型/P、关系数据库建模/L、替代键（唯一约束）.md)
        * [继承（关系数据库）](./3、创建模型/P、关系数据库建模/M、继承（关系数据库）.md)
* [4、查询数据](./4、查询数据/A、查询数据.md)
    * [基础查询](./4、查询数据/B、基础查询.md)
    * [加载关联数据](./4、查询数据/C、加载关联数据.md)
    * [客户端VS服务端评估](./4、查询数据/D、客户端VS服务端评估.md)
    * [跟踪VS不跟踪](./4、查询数据/E、跟踪VS不跟踪.md)
    * [原生SQL查询](./4、查询数据/F、原生SQL查询.md)
    * [异步查询](./4、查询数据/G、异步查询.md)
    * [查询的原理](./4、查询数据/H、查询的原理.md)
* [5、保存数据](./5、保存数据/A、保存数据.md)
    * [基础保存](./5、保存数据/B、基础保存.md)
    * [关系数据](./5、保存数据/C、关系数据.md)
    * [级联删除](./5、保存数据/D、级联删除.md)
    * [并发冲突](./5、保存数据/E、并发冲突.md)
    * [事务](./5、保存数据/F、事务.md)
    * [异步保存](./5、保存数据/G、异步保存.md)
    * [离线实体](./5、保存数据/H、离线实体.md)
    * [显式设置生成值属性的值](./5、保存数据/I、显式设置生成值属性的值.md)
* [6、平台支持](./6、平台支持/A、平台支持.md)
* [7、数据库提供程序](./7、数据库提供程序/A、数据库提供程序.md)
    * [Microsoft SQL Server](./7、数据库提供程序/B、MicrosoftSQLServer/A、MicrosoftSQLServer.md)
        * [内存优化表](./7、数据库提供程序/B、MicrosoftSQLServer/B、内存优化表.md)
    * [SQLite](./7、数据库提供程序/C、SQLite/A、SQLite.md)
        * [SQLite 局限性](./7、数据库提供程序/C、SQLite/B、SQLite局限性.md)
    * [PostgreSQL（Npgsql）](./7、数据库提供程序/D、PostgreSQL（Npgsql）.md)
    * [IBM Data Server](./7、数据库提供程序/E、IBMDataServer（DB2）.md)
    * [MySQL（官方）](./7、数据库提供程序/F、MySQL（官方）.md)
    * [MySQL（柚子）](./7、数据库提供程序/G、MySQL（柚子）.md)
    * [Microsoft SQL Server Compact Edition](./7、数据库提供程序/H、MicrosoftSQLServer精简版.md)
    * [内存数据库（用于测试）](./7、数据库提供程序/I、内存数据库（用于测试）.md)
    * [Devart（MySQL、Oracle、PostgreSQL、SQLite、DB2、更多）](./7、数据库提供程序/J、Devart（MySQL、Oracle、PostgreSQL、SQLite、DB2、更多）.md)
    * [Oracle（尚不可用）](./7、数据库提供程序/K、Oracle（尚不可用）.md)
    * [MyCat](./7、数据库提供程序/L、MyCat.md)
    * [编写自己的数据库提供程序](./7、数据库提供程序/M、自己编写数据库提供程序.md)
* [API 引用](https://docs.microsoft.com/dotnet/api/?view=efcore-2.0)
* [命令行引用](./9、命令行引用/A、命令行引用.md)
    * [程序包管理控制台（Visual Studio）](./9、命令行引用/B、程序包管理控制台（VisualStudio）.md)
    * [.NET Core CLI](./9、命令行引用/C、.NETCoreCLI.md)
* [工具集 & 扩展](./10、工具集&扩展/A、工具集&扩展.md)
    * [LLBLGen Pro](./10、工具集&扩展/B、LLBLGenPro.md)
    * [Devart Entity Developer](./10、工具集&扩展/C、DevartEntityDeveloper.md)
    * [EFSecondLevelCache.Core](./10、工具集&扩展/D、EFSecondLevelCache.Core.md)
    * [EntityFrameworkCore.Detached](./10、工具集&扩展/E、EntityFrameworkCore.Detached.md)
    * [EntityFrameworkCore.Triggers](./10、工具集&扩展/F、EntityFrameworkCore.Triggers.md)
    * [EntityFrameworkCore.Rx](./10、工具集&扩展/G、EntityFrameworkCore.Rx.md)
    * [EntityFrameworkCore.PrimaryKey](./10、工具集&扩展/H、EntityFrameworkCore.PrimaryKey.md)
    * [EntityFrameworkCore.TypedOriginalValues](./10、工具集&扩展/I、EntityFrameworkCore.TypedOriginalValues.md)
    * [EFCore.Practices](./10、工具集&扩展/J、EFCore.Practices.md)
    * [LinqKit.Microsoft.EntityFrameworkCore](./10、工具集&扩展/K、LinqKit.Microsoft.EntityFrameworkCore.md)
    * [Microsoft.EntityFrameworkCore.AutoHistory](./10、工具集&扩展/L、Microsoft.EntityFrameworkCore.AutoHistory.md)
    * [Microsoft.EntityFrameworkCore.DynamicLinq](./10、工具集&扩展/M、Microsoft.EntityFrameworkCore.DynamicLinq.md)
    * [Microsoft.EntityFrameworkCore.UnitOfWork](./10、工具集&扩展/N、Microsoft.EntityFrameworkCore.UnitOfWork.md)
* 其他
    * [连接字符串](./11、其他/B、连接字符串.md)
    * [日志记录](./11、其他/C、日志记录.md)

## Entity Framework Core 快速预览

Entity Framework(EF) Core 是 当前流行的 Entity Framework 数据访问技术的一个轻量级、可扩展、跨平台版本。

EF Core 是一个对象关系映射（O/RM）框架，它允许 .NET 开发者使用 .NET 对象来做数据库相关的事情。它消除了大部分开发者本来要编写的数据访问代码。EF Core 支持多种数据库引擎，详细信息参见 [数据库提供程序](./7、数据库提供程序/A、数据库提供程序.md)。

如果你喜欢通过敲代码来学习，我们建议你通过我们的 [入门指南](./2、入门指南/A、入门指南.md) 来开始学习 EF Core。

### 最新版本：EF Core 2.0

如果你们熟悉 EF Core 并且想要直接跳到新版本的内容细节上，请查阅：

* [EF Core 2.0 新特性](./1、EFCore2.0新特性/A、EFCore2.0新特性.md)
* [将现有应用程序升级到 EF Core 2.0](./11、其他/I、升级到EFCore2.0.md)

### 获取 Entity Framework Core

为你想要使用的数据库提供程序安装[相应的 NeGet 程序包](https://docs.microsoft.com/zh-cn/nuget/quickstart/use-a-package)。比如在跨平台开发中安装 SQL Server 提供程序，可以在命令行中使用 `dotnet` 工具：

```console
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

或者在 Visual Studio 的程序包管理控制台运行：

```PowerShell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

查看 [数据库提供程序](./7、数据库提供程序/A、数据库提供程序.md) 以了解关于如何获取提供程序的信息， [安装 EF Core](./2、入门指南/B、安装EFCore.md) 以了解详细的安装步骤。

### 模型

在 EF Core 中，数据访问是通过模型来实现的。一个模型由实体类型和一个表示一个数据库会话的派生上下文构成，你可以通过模型来查询和保存数据。查看 [创建模型](./3、创建模型/A、创建模型.md) 以了解更多。

你可以从现有数据库生成模型，手动编写模型来匹配你的数据库，或者使用 EF 迁移来从你的模型创建数据库（并在你的模型变更时推进它）。

```C#
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Intro
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MyDatabase;Trusted_Connection=True;");
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set;}
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
```

## 查询

你的实体类型的实例是使用 LINQ（Language Integrated Query，语言集成查询）从数据库中遍历出来的。查看 [查询数据](./4、查询数据/A、查询数据.md) 以了解更多。

```C#
using(var db = new BloggingContext())
{
    var blogs = db.Blogs
        .Where(b => b.Rating > 3)
        .OrderBy(b => b.Url)
        .ToList();
}
```

## 保存数据

数据库中的数据通过你的实体类型实例来进行创建、删除和修改。查看 [保存数据](./5、保存数据/A、保存数据.md) 以了解更多。

```C#
using(var db = new BloggingContext())
{
    var blog = new Bolg { Url = "http://sample.com" };
    db.Blogs.Add(blog);
    db.SaveChanges();
}
```