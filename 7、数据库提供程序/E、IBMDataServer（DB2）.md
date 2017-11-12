# IBM Data Server（DB2） EF Core 数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问 IBM Data Server。它由 IBM 维护。

> 注意
>
> 该提供程序没有作为 Entity Framework Core 项目的一部分来维护。当考虑第三方提供程序的时候，一定要评估其质量、许可、支持情况等等以确保它们符合你的需求。

## 安装

在 Windows 上使用 IBM Data Server 的话要安装 [`IBM.EntityFrameworkCore NuGet 程序包`](https://www.nuget.org/packages/IBM.EntityFrameworkCore)。

```console
PM> Install-Package IBM.EntityFrameworkCore
```

在 Linux 上使用 IBM Data Server 的话要安装 [`IBM.EntityFrameworkCore-lnx NuGet 程序包`](https://www.nuget.org/packages/IBM.EntityFrameworkCore-lnx)。

```console
PM> Install-Package IBM.EntityFrameworkCore-lnx
```

## 入门

[开始在 .NET Core 中使用 IBM .NET 提供程序](https://www.ibm.com/developerworks/community/blogs/96960515-2ea1-4391-8170-b0515d08e4da/entry/DB2DotnetCore?lang=en)

## 支持的数据库引擎

* zOS
* LUW including IBM dashDB
* IBM I
* Informix

## 支持的平台

* .NET Framework（4.6 以上版本）
* .NET Core