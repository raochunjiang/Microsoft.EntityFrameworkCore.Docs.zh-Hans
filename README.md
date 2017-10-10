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


## Entity Framework Core 快速预览

Entity Framework(EF) Core 是 当前流行的 Entity Framework 数据访问技术的一个轻量级、可扩展、跨平台版本。

EF Core 是一个对象关系映射（O/RM）框架，它允许 .NET 开发者使用 .NET 对象来做数据库相关的事情。它消除了大部分开发者本来要编写的数据访问代码。EF Core 支持多种数据库引擎，详细信息参见 [数据库提供程序](./7、数据库提供程序/A、数据库提供程序.md)。

如果你喜欢通过敲代码来学习，我们建议你通过我们的 [入门指南](./2、入门指南/A、入门指南.md) 来开始学习 EF Core。

### 最新版本：EF Core 2.0

如果你们熟悉 EF Core 并且想要直接跳到新版本的内容细节上，请查阅：

* [EF Core 2.0 新特性](./1、EFCore2.0新特性/A、EFCore2.0新特性.md)
* [将现有应用程序升级到 EF Core 2.0](./11、其他/H、升级到EFCore2.0.md)

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