# Microsoft SQL Server 精简版的 EF Core 数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问Microsoft SQL Server 精简版 。它被作为 [ErikEJ/EntityFramework.SqlServerCompact GitHub 项目](https://github.com/ErikEJ/EntityFramework.SqlServerCompact)的一部分来维护。

> 注意
>
> 该提供程序没有作为 Entity Framework Core 项目的一部分来维护。当考虑第三方提供程序的时候，一定要评估其质量、许可、支持情况等等以确保它们符合你的需求。

## 安装

使用 SQL Server 精简版  4.0 的话要安装 [`EntityFrameworkCore.SqlServerCompact40 NuGet 程序包`](https://www.nuget.org/packages/EntityFrameworkCore.SqlServerCompact40)。

```console
PM> Install-Package EntityFrameworkCore.SqlServerCompact40
```

使用 SQL Server 精简版  3.5 的话要安装 [‘EntityFrameworkCore.SqlServerCompact35`](https://www.nuget.org/packages/EntityFrameworkCore.SqlServerCompact35)。

```console
PM> Install-Package EntityFrameworkCore.SqlServerCompact35
```

## 入门

请查阅 [项目站点上的入门帮助文档](https://github.com/ErikEJ/EntityFramework.SqlServerCompact/wiki/Using-EF-Core-with-SQL-Server-Compact-in-Traditional-.NET-Applications)。

## 支持的数据库引擎

* SQL Server 精简版 3.5
* SQL Server 精简版 4.0

## 支持的平台

* .NET Framework（4.5.1 以上版本）