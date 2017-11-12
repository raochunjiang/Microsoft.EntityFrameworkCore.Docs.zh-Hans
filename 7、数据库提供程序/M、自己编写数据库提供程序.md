# 自己编写数据库提供程序

关于编写 Entity Framework Core 数据库提供程序的更多信息，请查阅  [Arthur Vickers](https://github.com/ajcvickers) 的 [你因此想要编写一个 EF Core 提供程序](https://blog.oneunicorn.com/2016/11/11/so-you-want-to-write-an-ef-core-provider/)。

EF Core 代码基础是开源的，其中包含一些可以以引用方式使用的数据库提供程序。你可以在 <https://github.com/aspnet/EntityFramework> 中找到源代码。

## provider-beware 标签

一旦你开始编写一个提供程序，就要监视我们在 GitHub 问题库上的 [`provider-beware`]((https://github.com/aspnet/EntityFramework/labels/providers-beware)) 标签并拉取请求（pull request）。我们通过这个标签来识别可能影响提供程序作者的变更。

## 建议的第三方提供程序命名规则

我们建议对你的提供程序 NuGet 程序包使用以下命名规则。这与 EF Core 团队发布的程序包名称是一致的。

`<可选的 公司/项目名称>.EntityFrameworkCore.<数据库引擎名称>`

比如：

* `Microsoft.EntityFrameworkCore.SqlServer`
* `Npgsql.EntityFrameworkCore.PostgreSQL`
* `EntityFrameworkCore.SqlServerCompact40`