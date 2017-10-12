# ASP.NET Core 下的 EF Core 现有数据库入门指南

> 重要提示
>
> [.NET Core SDK](https://www.microsoft.com/net/download/core) 已不再支持 `project.json` 或 Visual Studio 2015。我们建议你 [将 project.json 迁移到 csproj](https://docs.microsoft.com/dotnet/articles/core/migration/)。如果你正在使用 Visual Studio，我们建议你迁移到 [Visual Studio 2017](https://www.visualstudio.com/downloads/)

在这个演练中，你将构建一个使用 Entity Framework Core 进行基础数据访问的 ASP.NET Core MVC 应用程序。你将基于一个现有的数据库使用逆向工程来创建 Entity Framework 模型。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/GetStarted/AspNetCore/EFGetStarted.AspNetCore.ExistingDb)。

## 先决条件

以下是完成当前演练所需的先决条件：

* 具有以下工作负载的 [Visual Studio 2017 15.3](https://www.visualstudio.com/downloads/)：
    * **ASP.NET 和 Web 开发** （在 **Web 或 Cloud** 下）
    * **.NET Core 跨平台开发** （在 **其他工具集** 下 ）
* [.NET Core 2.0 SKD](https://www.microsoft.com/net/download/core)
* [博客数据库](https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/existing-db#blogging-database)

## 博客数据库

当前课程使用你 LocalDB 实例中的一个 **博客** 数据库来作为现有数据库。

> 提示
>
> 如果你在其他课程中已经创建了 **博客** 数据库，可以跳过该步骤

* 启动 Visual Studio
* 工具 > 连接到数据库...
* 选择 **Microsoft SQL Server 并点击 **继续**
* 输入 **(localdb)\mssqllocaldb** 作为服务器名称
* 输入 **master** 作为**数据库名称**并点击 **确定**
* 现在，master 数据库已经显示在 **服务器资源管理器** 中的 **数据连接** 之下了
* 右键点击 **服务器资源管理器** 中的数据库并选择 **新建查询**
* 将以下列出的脚本复制到新建的查询编辑器中
* 在查询编辑器中点击右键并选择 **执行（Execute）**

```SQL
CREATE DATABASE [Blogging];
GO

USE [Blogging];
GO

CREATE TABLE [Blog] (
    [BlogId] int NOT NULL IDENTITY,
    [Url] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Blog] PRIMARY KEY ([BlogId])
);
GO

CREATE TABLE [Post] (
    [PostId] int NOT NULL IDENTITY,
    [BlogId] int NOT NULL,
    [Content] nvarchar(max),
    [Title] nvarchar(max),
    CONSTRAINT [PK_Post] PRIMARY KEY ([PostId]),
    CONSTRAINT [FK_Post_Blog_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Blog] ([BlogId]) ON DELETE CASCADE
);
GO

INSERT INTO [Blog] (Url) VALUES
('http://blogs.msdn.com/dotnet'),
('http://blogs.msdn.com/webdev'),
('http://blogs.msdn.com/visualstudio')
GO
```

## 创建新项目

* 启动 Visual Studio 2017
* **文件** > **新建** > **项目...**
* 从左侧菜单开始选择 **已安装** > **模板** > **Visual C#** > **Web**
* 选择 **ASP.NET Core Web 应用程序** 项目模板
* 输入项目名称 **EFGetStarted.AspNetCore.ExistingDb** 后点击 **确定**
* 等待出现 **新建 ASP.NET Core Web 应用程序** 对话框
* 确保下拉选择了 **.NET Core** 和 **ASP.NET Core 2.0**
* 选择 **Web 应用程序（模型视图控制器）** 项目模板
* 确保已 **更改身份认证** 为 **不进行身份认证**
* 点击 **确定**

## 安装 Entity Framework

要使用 EF Core 的话，就要根据你的目标数据库提供程序安装相应的程序包。当前演练使用的是 SQL Server。查看 [数据库提供程序](../../7、数据库提供程序/A、数据库提供程序.md) 可获得可用提供程序的列表。

* 工具 > NuGet 包管理器 > 程序包管理控制台
* 运行 `Install-Package Microsoft.EntityFrameworkCore.SqlServer`

我们将使用一些 Entity Framework 工具来从你的数据库生成 EF Core  模型。所以我们还要安装工具包：

* 运行 `Install-Package Microsoft.EntityFrameworkCore.Tools`

我们稍后将使用一些 ASP.NET Core 基架工具来创建控制器和视图。所以我们还要安装以下设计工具包：

* 运行 `Install-Package Microsoft.VisualStudio.Web.CodeGeneration.Design`

## 逆向工程创建模型

现在，是时候基于现有数据库来创建你的 EF 模型了。

* **工具** > **NuGet 包管理器** > **程序包管理控制台**
* 运行以下命令以从现有数据库创建模型。如果输出错误 `无法将“Scaffold-DbContext”项识别为 cmdlet、函数、脚本文件或可运行程序的名称`，请关闭和重新打开 Visual Studio 后再试一次

```console
Scaffold-DbContext "Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
```

> 提示
>
> 你可以在上述命令中添加 `-Tables` 参数来指定哪些是你想要生成实体的数据表。比如 `-Tables Blog,Post`。

逆向工程进程会基于现有数据库的模式来创建实体类型（`Bolg.cs` 和 `Post.cs`）并派生一个上下文类型（`BloggingContext.cs`）。

实体类型是代表你将查询和保存的数据的简单 C# 对象。

```C#
using System;
using System.Collections.Generic;

namespace EFGetStarted.AspNetCore.ExistingDb.Models
{
    public partial class Blog
    {
        public Blog()
        {
            Post = new HashSet<Post>();
        }

        public int BlogId { get; set; }
        public string Url { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}
```

上下文则表示一个数据库会话，该会话允许你查询和保存上述实体类型的实例。

```C#
public partial class BloggingContext : DbContext
{
   public virtual DbSet<Blog> Blog { get; set; }
   public virtual DbSet<Post> Post { get; set; }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
       if (!optionsBuilder.IsConfigured)
       {
           #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
           optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;");
       }
   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       modelBuilder.Entity<Blog>(entity =>
       {
           entity.Property(e => e.Url).IsRequired();
       });

       modelBuilder.Entity<Post>(entity =>
       {
           entity.HasOne(d => d.Blog)
               .WithMany(p => p.Post)
               .HasForeignKey(d => d.BlogId);
       });
   }
}
```

## 将上下文注册到依赖注入

依赖注入的概念是 ASP.NET Core 核心。服务（比如 `BloggingContext`）将在应用程序启动时被注册到依赖注入，之后需要使用这些服务的组件（比如 MVC 控制器）就可以通过构造方法参数或属性来获取需要的服务。关于依赖注入的更多信息请查看 ASP.NET 站点上的[依赖注入](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection)

### 移除行内上下文配置

在 ASP.NET Core 中，配置通常是在 **Startup.cs** 中执行的。为了遵照这一模式，我们将要把数据库提供程序的配置移动到 **Startup.cs** 中。

* 打开 `Models\BloggingContext.cs`
* 删除 `OnConfiguring(...)`方法

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
    optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;");
}
```

* 添加以下构造方法，该构造方法允许通过依赖注入将配置传递给上下文实例

```C#
public BloggingContext(DbContextOptions<BloggingContext> options)
    : base(options)
{ }
```

### 在 Startup.cs 中注册和配置上下文

为了让我们的 MVC 控制器能够使用 `BloggingContext`，我们要将它注册为服务。

* 打开 **Startup.cs**
* 添加以下 `using` 声明：

```C#
using EFGetStarted.AspNetCore.ExistingDb.Models;
using Microsoft.EntityFrameworkCore;
```

现在我们可以通过 `AddDbContext` 方法将 `BloggingContext` 注册为服务：

* 添加以下代码到 ConfigureServices` 方法中：

```C#
// This method gets called by the runtime. Use this method to add services to the container.
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

    var connection = @"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;";
    services.AddDbContext<BloggingContext>(options => options.UseSqlServer(connection));
}
```

> 注意
>
> 真实的应用程序通常会将连接字符串放到配置文件中。为了简单起见，我们直接在代码中定义连接字符串。查看[连接字符串](https://docs.microsoft.com/zh-cn/ef/core/miscellaneous/connection-strings)可了解更多信息。

## 创建控制器

接下来我们将启用项目中的 MVC 基架：

* 在**解决方案资源管理器** 中右键点击 **Controllers** 目录，选择 **添加 > 控制器**。
* 选择 **MVC 依赖项** 后点击 **添加**，然后选择 **最小依赖项** 后点击 **添加**
* 你可以 `ScaffoldingReadMe.txt` 文件中的介绍

这样就启用了 MVC 基架。我们可以为 `Blog` 实体添加一个控制器了。

* 在**解决方案资源管理器** 中右键点击 **Controllers** 目录，选择 **添加 > 控制器**。
* 选择 **视图使用 Entity Framework 的控制器**，然后点击 **添加**。
* 将 **模型类** 设置为 **Blog**，**数据上下文类** 设置为 **BloggingContext**。
* 点击 **添加**

## 运行应用程序

按 F5 运行和测试应用程序。

* 将浏览器导航到 `/Blogs`
* 点击 **Create New** 连接，可以创建一些博客实体，然后就可以测试 **Details** 和 **Delete** 连接了。

![create.png](./create.png)

![index-new-db.png](./index-new-db.png)

## 其他资源

* [EF - SQLite 新数据库](../D、.NETCore/B、新数据库.md)  - 跨平台控制台应用程序 EF 教程。
* [Mac 或 Linux 上的 ASP.NET Core MVC 入门](https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/first-mvc-app-xplat/index)
* [基于 Visual Studio 使用  ASP.NET Core MVC 入门](https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/first-mvc-app/index)
* [基于 Visual Studio 使用 ASP.NET Core 和 Entity Framework Core 入门](https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-mvc/index)