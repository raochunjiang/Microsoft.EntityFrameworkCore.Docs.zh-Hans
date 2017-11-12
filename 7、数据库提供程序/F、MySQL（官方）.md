# MySQL EF Core 数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问 MySQL。它被作为 [MySQL 项目](http://dev.mysql.com/)的一部分来维护。

> 警告
>
> 该提供程序处于预发布状态

> 注意
>
> 该提供程序没有作为 Entity Framework Core 项目的一部分来维护。当考虑第三方提供程序的时候，一定要评估其质量、许可、支持情况等等以确保它们符合你的需求。

## 安装

安装 [`MySql.Data.EntityFrameworkCore NuGet 程序包`](http://insidemysql.com/howto-starting-with-mysql-ef-core-provider-and-connectornet-7-0-4/)。

```console
PM> Install-Package MySql.Data.EntityFrameworkCore -Pre
```

## 入门

请查阅 [开始使用 MySQL EF Core 提供程序和连接器/NET 7.0.4](http://insidemysql.com/howto-starting-with-mysql-ef-core-provider-and-connectornet-7-0-4/)

## 支持的数据库引擎

* MySQL

## 支持的平台

* .NET Framework（45.1 以上版本）
* .NET Core