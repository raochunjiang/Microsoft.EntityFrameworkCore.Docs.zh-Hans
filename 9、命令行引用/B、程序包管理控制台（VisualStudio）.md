# EF Core 程序包管理控制台工具集

EF Core 程序包管理控制台（PMC）工具集是通过 NuGet 的 [程序包管理控制台](https://docs.microsoft.com/nuget/tools/package-manager-console) 在 Visual Studio 中运行的。这些工具可以用于 .NET Framework 和 .NET Core 项目。

> 提示
>
> 没有使用 Visual Studio？EF Core 命令行工具是跨平台的，它可以在命令提示符下运行！

## 安装工具集

安装 Microsoft.EntityFrameworkCore.Tools NuGet 程序包就可以安装 EF Core 程序包管理控制台工具集。可以在 [程序包管理控制台](https://docs.microsoft.com/nuget/tools/package-manager-console) 中执行以下命令来实现安装。

```PowerShell
Install-Package Microsoft.EntityFrameworkCore.Tools
```

如果一切工作正常，你应该能够运行这样的命令：

```PowerShell
Get-Help about_EntityFrameworkCore
```

> 提示
>
> 如果你的启动项目指向 .NET Standard，请在使用工具集前交叉指向已支持的框架。

> 重要提示
>
> 如果你正在使用 Windows 通用（Universal Windows） 或 Xamarin，请将你的 EF Core 代码移动到一个 .NET Standard 类库中，然后在使用工具集前交叉指向已支持的框架。将这个类型库指定为你的启动项目。

## 使用工具集

任何时候只要调用命令，都会牵涉到两个项目：

目标项目是任何文件添加到的地方（一些情况是移除文件）。默认情况下目标项目就是程序包管理控制台中选中的默认项目，但是可以使用 -Project 参数来临时指定另一个项目。

启动项目是执行项目代码时由工具模拟的，默认就是解决方案资源管理器中通过 **设置为启动项目** 设置的项目，但是可以使用 -StartupProject 参数来临时指定另一个项目。

通用的参数：
|||
|:---:|:---:|
|`-Context <String>`|使用的 DbContext|
|`-Project <String>`|使用的项目|
|`-StartupProject <String>`|使用的启动项目|
|`-Verbose`|显式详细输出|

使用 `Get-Help` PowerShell 命令可以显式某个命令的帮助信息。

> 提示
>
> Context、Project 和 StartupProject 参数支持 标签扩展（tab-expansion，这里说的应该是快捷键）。

> 提示
>
> 在运行前设置 **env:ASPNETCORE_ENVIRONMENT** 可以指定 ASP.NET Core 环境。

## 命令

### Add-Migration

添加新的迁移。

参数：

|||
|:---:|:---:|
|_**`-Name <String>`**_|迁移的名称|
|`-OutputDir <String>`|使用的目录（及其子名称空间）。路径是相对于项目目录的。默认是 “Migrations”。|

> 注意
>
> **加粗** 的参数是必须的，_斜体_ 的参数则是可选的

### Drop-Database

删除数据库。

参数：

|||
|:---:|:---:|
|`-WhatIf`|显式将要删除的数据库，但还不会删除它|

### Get-DbContext

获取 DbContext 类型的信息。

### Remove-Migration

移除最近一个迁移。

参数：

|||
|:---:|:---:|
|`-Force`|移除迁移前通常需要验证其是否已经应用到了数据库。使用该参数可跳过验证参数可跳过验证。|

### Scaffold-DbContext

为数据库搭建 DbContext 和实体类型的基架。

参数：

|||
|:---:|:---:|
|_**`-Connection <String>`**_|数据库的连接字符串|
|_**`-Provider <String>`**_|使用的提供程序。（比如 Microsoft.EntityFrameworkCore.SqlServer）|
|`-OutputDir <String>`|放置基架文件的目录。路径是相对于项目目录的。|
|`-Context <String>`|生成 DbContext 的名称|
|`-Schemas <String[]>`|仅生成指定数据库模式中的表对应的实体类型|
|`-Tables <String[]>`|仅生成指定数据表对应的实体类型|
|`-DataAnnotations`|生成的模型尽可能使用特性（数据注解）配置。缺省时仅生成流式 API 配置代码。|
|`-UseDatabaseNames`|直接使用数据库中的表名和列名生成相关名称。|
|`-Force`|覆盖已有的文件。|

### Script-Migration

根据迁移（段）生成 SQL 脚本

参数：

|||
|:---:|:---:|
|_`-From <String>`_|起始迁移。默认是 0（初始化数据库）。|
|_`To <String>`_|结束迁移。默认是最近一个迁移。|
|`-Idempotent`|为涉及到的迁移生成一个可以在数据库上使用的脚本。|
|`-Output <String>`|脚本输出到的文件。|

> 提示
>
> To、From 和 Output 参数都支持标签扩展（tab-extension）。

### Update-Database

|||
|:---:|:---:|
|_`-Migration <String>`_|要应用的迁移。设置为 “0” 时应用所有迁移。默认为最近一个迁移。|

> 提示
>
> 迁移参数支持选项扩展（tab-extension）。