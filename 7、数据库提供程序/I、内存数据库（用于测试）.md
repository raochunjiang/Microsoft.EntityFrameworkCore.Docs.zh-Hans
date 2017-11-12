# EF Core 内存数据库提供程序

该数据库提供程序允许 Entity Framework Core 被用于访问内存数据库，这对于使用 Entity Framework Core 的测试代码来说会很有用。它被作为 [Entity Framework GitHub 项目](https://github.com/aspnet/EntityFramework)的一部分来维护。

## 安装

安装 [`Microsoft.EntityFrameworkCore.InMemory NuGet 程序包`](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/)。

```console
PM> Install-Package Microsoft.EntityFrameworkCore.InMemory
```

## 入门

以下资源有助于你入门使用该提供程序

* [使用内存数据库测试](../11、其他/E、测试/C、使用内存数据库测试.md)
* [UnicornStore 样例应用程序测试](https://github.com/rowanmiller/UnicornStore/blob/master/UnicornStore/src/UnicornStore.Tests/Controllers/ShippingControllerTests.cs)


## 支持的数据库引擎

* 内置的内存数据库（设计仅用于测试）

## 支持的平台

* .NET Framework（4.5.1 以上版本）
* .NET Core
* Mono（4.2.0 以上版本）
* Universal Windows Platform（UWP）