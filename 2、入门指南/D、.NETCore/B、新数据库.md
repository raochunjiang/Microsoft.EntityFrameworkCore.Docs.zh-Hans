# .NET Core 下的 EF Core 新数据库入门指南

在这个演练中，你将构建一个针对 SQLite 数据库执行基础数据问的 .NET Core 控制台应用程序。你将使用迁移来从模型创建数据库。查看 [ASP.NET Core - 新数据库](../E、ASP.NETCore/A、ASP.NETCore.md) 可了解相应的 Visual Studio 版本，其中使用的是 ASP.NET Core MVC。

> 注意
>
> [.NET Core SDK](https://www.microsoft.com/net/download/core) 已不再支持 `project.json` 或 Visual Studio 2015。我们建议你 [将 project.json 迁移到 csproj](https://docs.microsoft.com/dotnet/articles/core/migration/)。如果你正在使用 Visual Studio，我们建议你迁移到 [Visual Studio 20177](https://www.visualstudio.com/downloads/)

> 提示
> 
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/GetStarted/NetCore/ConsoleApp.SQLite)。

## 先决条件

以下是完成当前演练所需的先决条件：

* 支持 .NET Core 的操作系统
* [.NET Core SDK](https://www.microsoft.com/net/core) 2.0 （尽管通过少量修改就可以通过入门简介了解如何使用之前的版本创建应用程序）。

## 创建新项目

* 为你的项目新建一个 `ConsoleApp.SQLite` 目录，然后使用 `dotnet` 命令将 .NET Core 应用程序填充进去。

```console
mkdir ConsoleApp.SQLite
cd ConsoleApp.SQLite/
dotnet new console
```

## 安装 Entity Framework Core

要使用 EF Core 的话，就要根据你的目标数据库提供程序安装相应的程序包。当前演练使用的是 SQLite。查看 [数据库提供程序](../../7、数据库提供程序/A、数据库提供程序.md) 可获得可用提供程序的列表。

* 安装  Microsoft.EntityFrameworkCore.Sqlite 和 Microsoft.EntityFrameworkCore.Design

```console
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

* 手动编辑 ConsoleApp.SQLite.csproj，添加 DotNetCliToolReference 以将Microsoft.EntityFrameworkCore.Tools.DotNet 包含到项目中：

```XML
<ItemGroup>
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
</ItemGroup>
```

> 注意
>
> 未来版本的 `dotnet` 将能够通过 `dotnet add tool` 来添加 DotNetCliToolReference。

现在的 `ConsoleApp.SQLite.csproj` 看起来应该是这样的：

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="2.0.0" />
  </ItemGroup>
</Project>
```

> 注意
>
> 上面使用的版本号会在发布时已经修正。

* 运行 `dotnet restore` 以安装新的程序包

## 创建模型

现在，是时候定义构成你的模型的上下文和实体类型了。

* 创建新的 _Model.cs_ 文件并用以下代码填充它。

```C#
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ConsoleApp.SQLite
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
```

> 提示
>
> 在真实的应用程序里面你会将每个类型放到独立的文件中，并且将链接字符串放到配置文件中。为了简单起见，我们将当前课程中的所有代码都放到单一的文件中。

## 创建数据库

你已经有一个模型了，你可以使用[迁移](https://docs.microsoft.com/aspnet/core/data/ef-mvc/migrations#introduction-to-migrations)来创建数据库。

* 运行 `dotnet ef migrations add InitialCreate` 以搭建迁移基架和创建模型对应的初始表集合。
* 运行 `dotnet ef database update`  以将新的迁移应用到数据库。该命令会在应用迁移之前创建数据库。

> 注意
>
> 当使用了 SQLite 相对路径时，路径是相对于应用程序主程序集的。在当前样例中，主二进制文件是 `bin/Debug/netcoreapp2.0/ConsoleApp.SQLite.dll`，所以 SQLite 数据库将在 `bin/Debug/netcoreapp2.0/blogging.db` 目录下。

## 使用模型

现在，你可以使用模型进行数据访问了

* 打开 _Program.cs_
* 使用以下代码替换类文件中的内容：

```C#
using System;

namespace ConsoleApp.SQLite
{
    public class Program
    {
        public static void Main()
        {
            using (var db = new BloggingContext())
            {
                db.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                Console.WriteLine("All blogs in database:");
                foreach (var blog in db.Blogs)
                {
                    Console.WriteLine(" - {0}", blog.Url);
                }
            }
        }
    }
}
```

* 测试应用程序：

`dotnet run`

一个博客已经保存到了数据库，然后所有的 blog 详细信息都被打印到了 控制台。

```console
ConsoleApp.SQLite>dotnet run
1 records saved to database

All blogs in database:
 - http://blogs.msdn.com/adonet
```

### 更改模型

* 如果你更改了模型，可以使用 `dotnet ef migrations add` 命令搭建新的基架来确保相应的模式能变更到数据库。一旦你签出了基架代码（并根据需要做了任何变更），可以使用 `dotnet ef database update` 命令将变更应用到数据库。
* EF 在数据库中使用 `__EfMigrationHistory` 表来记录哪些迁移已经被应用到数据库。
* 由于[SQLite 本身的限制](https://docs.microsoft.com/zh-cn/ef/core/providers/sqlite/limitations)，SQLite 并不支持所有的迁移（模式变更）。对于全新的开发，建议在你的模型变更时删除数据库并创建新库，而不是使用迁移。

## 其他资源

* [.NET Core - 新数据库](./B、新数据库.md)  - 跨平台控制台应用程序 EF 教程。
* [Mac 或 Linux 上的 ASP.NET Core MVC 入门](https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/first-mvc-app-xplat/index)
* [基于 Visual Studio 使用  ASP.NET Core MVC 入门](https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/first-mvc-app/index)
* [基于 Visual Studio 使用 ASP.NET Core 和 Entity Framework Core 入门](https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-mvc/index)