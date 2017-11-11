# Microsoft SQL Server EF Core 数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问 Microsoft SQL Server（包括 SQL Azure），它被作为 [Entity Framework GitHub 项目](https://github.com/aspnet/EntityFramework)的一部分来维护。

## 安装

安装 [Microsoft.EntityFrameworkCore.SqlServer NuGet 程序包](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/)。

```console
PM> Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

## 入门

以下资源有助于你入门使用该提供程序

* [.NET Framework（控制台、WinForm、WPF 等等）的 EF Core 入门指南](../../2、入门指南/C、.NETFramework/A、.NETFramework.md)
* [ASP.NET Core 的 EF Core 入门指南](../../2、入门指南/E、ASP.NETCore/A、ASP.NETCore.md)
* [UnicornStore 样例应用程序](https://github.com/rowanmiller/UnicornStore/tree/master/UnicornStore)

## 支持的数据库引擎

* Microsoft SQL Server （2008 以上版本）

## 支持的平台

* .NET Framework（4.5.1 以上版本）
* .NET Core
* Mono（4.2.0 以上版本）

    警告：在 Mono 上使用该提供程序将会用到 Mono SQL Client 实现，这会导致很多已知问题。比如说，不支持安全连接（SSL）。