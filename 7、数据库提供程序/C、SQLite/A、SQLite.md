# SQLite EF Core 数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问 SQLite，它被作为 [Entity Framework GitHub 项目](https://github.com/aspnet/EntityFramework)的一部分来维护。

## 安装

安装 `Microsoft.EntityFrameworkCore.SQLite NuGet 程序包`。

```console
PM> Install-Package Microsoft.EntityFrameworkCore.SQLite
```

## 入门

以下资源有助于你入门使用该提供程序

* [本地 UWP 的 EF Core 入门指南](../../2、入门指南/F、通用Windows平台（UWP）/A、通用Windows平台（UWP）.md)
* [使用新 SQLite 数据库的 .NET Core 应用程序](../../2、入门指南/D、.NETCore/B、新数据库.md)
* [Unicorn Clicker 样例应用程序](https://github.com/rowanmiller/UnicornStore/tree/master/UnicornClicker/UWP)
* [Unicorn Packer 样例应用程序](https://github.com/rowanmiller/UnicornStore/tree/master/UnicornPacker)

## 支持的数据库引擎

* SQLite（3.7以上版本）

## 支持的平台

* .NET Framework（4.5.1 以上版本）
* .NET Core
* Mono（4.2.0 以上版本）
* Universal Windows Platform

## 局限性

查看 [SQLite 局限性](./B、SQLite局限性.md) 以了解 EF Core SQLite 提供程序的一些重要局限性。