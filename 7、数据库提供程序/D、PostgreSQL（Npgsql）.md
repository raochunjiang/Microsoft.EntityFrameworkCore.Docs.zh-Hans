# PostgreSQL 的 Npgsql EF Core 数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问 PostgreSQL。它被作为 [Npgsql 项目](http://www.npgsql.org/)的一部分来维护。

> 注意
>
> 该提供程序没有作为 Entity Framework Core 项目的一部分来维护。当考虑第三方提供程序的时候，一定要评估其质量、许可、支持情况等等以确保它们符合你的需求。

## 安装

安装 `Npgsql.EntityFrameworkCore.PostgreSQL NuGet 程序包`。

```console
PM> Install-Package Npgsql.EntityFrameworkCore.PostgreSQL
```

## 入门

查看 [Npgsql 帮助文档](http://www.npgsql.org/efcore/index.html) 以开始使用该提供程序。

## 支持的数据库引擎

* PostgreSQL

## 支持的平台

* .NET Framework（4.5.1 以上版本）
* .NET Core
* Mono（4.2.0 以上版本）