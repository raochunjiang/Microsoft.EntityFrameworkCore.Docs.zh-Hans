# MySQL 的 柚子 EF Core 数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问 MySQL。它被作为 [柚子基础项目](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql)的一部分来维护。

> 注意
>
> 该提供程序没有作为 Entity Framework Core 项目的一部分来维护。当考虑第三方提供程序的时候，一定要评估其质量、许可、支持情况等等以确保它们符合你的需求。

## 安装

安装 [`Pomelo.EntityFrameworkCore.MySql NuGet 程序包`](https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql)。

```console
PM> Install-Package Pomelo.EntityFrameworkCore.MySql
```

## 入门

以下资源有助于你入门使用该提供程序

* [入门帮助文档](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/blob/master/README.md#getting-started)
* [Yuuko 博客样例应用程序](https://github.com/PomeloFoundation/YuukoBlog)

## 支持的数据库引擎

* MySQL
* MariaDB

## 支持的平台

* .NET Framework（4.5.1 以上版本）
* .NET Core
* Mono（4.2.0 以上版本）