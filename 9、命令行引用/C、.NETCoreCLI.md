# EF Core .NET 命令行工具集

Entity Framework Core .NET 命令行工具集是跨平台 **dotnet** 命令的扩展。后者是 [.NET Core SDK](https://www.microsoft.com/net/core) 的一部分。

> 提示
>
> 如果你正在使用 Visual Studio，我们建议你使用 PMC 工具集，因为它提供了更加完整的体验。

## 安装工具集

使用以下步骤可以安装 EF Core .NET 命令行工具集：

1. 编辑项目文件，将 Microsoft.EntityFrameworkCore.Tools.DotNet 作为 DotNetCliToolReference 项目添加到其中（如下所示）。

2. 在命令提示符下运行以下命令。

```cmd
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet restore
```

最终的项目文件看起来应该是这样的：

```XML
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp2.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design"
                      Version="2.0.0"
                      PrivateAssets="All" />
  </ItemGroup>
  <ItemGroup>
    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet"
                            Version="2.0.0" />
  </ItemGroup>
</Project>
```

> 注意
>
> 带有 `PrivateAssets="All"` 的程序包引用意味着它不会被暴露到引用当前项目的其他项目中。这对于通常只在开发时使用的程序包来说尤为有用。

正常情况下，应该能够在命令提示符下成功运行以下命令了。

```cmd
dotnet ef
```

## 使用工具集

任何时候只要调用命令，都会牵涉到两个项目：

目标项目是任何文件添加到的地方（一些情况是移除文件）。默认情况下目标项目就是当前目录下的项目，但是可以使用 -project 参数来临时指定另一个项目。

启动项目是执行项目代码时由工具模拟的，同样，默认情况下启动项目也是当前目录下的项目，但是可以使用 -startup-project 参数来临时指定另一个项目。

通用的参数：
||||
|:---:|:---:|:---:|
||`--json`|显示 json 输出|
|`-c`|`--context <DBCONTEXT>`|使用的 DbContext|
|`-p`|`--project <PROJECT>`|使用的项目|
|`-s`|`--startup-project <PROJECT>`|使用的启动项目|
||`--framework <FRAMEWORK>`|目标框架|
||`--configuration <CONFIGURATION>`|使用的配置|
||`--runtime <IDENTIFIER>`|使用的运行时|
|`-h`|`--help`|显示帮助信息|
|`-v`|`--Verbose`|显式详细输出|
||`--no-color`|单色输出|
||`--prefix-output`|输出时添加等级前缀|

> 提示
>
> 在运行前设置 **env:ASPNETCORE_ENVIRONMENT** 可以指定 ASP.NET Core 环境。

## 命令

### dotnet ef database drop

删除数据库。

选项：

||||
|:---:|:---:|:---:
|`-f`|`--force`|跳过确认|
||`--dry-run`|显式将要删除的数据库，但还不会删除它|

### dotnet ef database update

将数据库更新到指定的迁移。

参数：

|||
|:---:|:---:|
|`<MIGRATION>`|更新到的迁移。0 表示将遍历所有迁移。默认为最近一个迁移。|

### dotnet ef dbcontext info

获取 DbContext 类型的信息。

### dotnet ef dbcontext list

列出可用的 DbContext 类型。

### dotnet ef dbcontext scaffold

为数据库搭建 DbContext 和实体类型的基架。

参数：

|||
|:---:|:---:|
|`<CONNECTION>`|数据库的连接字符串|
|`<PROVIDER>`|使用的提供程序。（比如 Microsoft.EntityFrameworkCore.SqlServer）|

选项：

||||
|:---:|:---:|:---:|
|`-d`|`--data-annotations`|生成的模型尽可能使用特性（数据注解）配置。缺省时仅生成流式 API 配置代码。|
|`-c`|`--context <NAME>`|生成 DbContext 的名称。|
|`-f`|`--force`|覆盖已有的文件。|
|`-o`|`--output-dir <PATH>`|放置基架文件的目录。路径是相对于项目目录的。|
||`--schema <SCHEMA_NAME> ...`|仅生成指定数据库模式中的表对应的实体类型。|
|`-t`|`--table <TABLE_NAME> ...`|仅生成指定数据表对应的实体类型。|
||`--use-database-names`|直接使用数据库中的表名和列名生成相关名称。|

### dotnet ef migrations add

添加新的迁移。

参数：

|||
|:---:|:---:|
|`<NAME>`|迁移的名称|

选项：

||||
|:---:|:---:|:---:|
|`-o`|`--output-dir <PATH>`|使用的目录（及其子名称空间）。路径是相对于项目目录的。默认是 “Migrations”。|

### dotnet ef migrations list

列出可用的迁移。

### dotnet ef migrations remove

移除最近一个迁移。

选项：

||||
|:---:|:---:|:---:|
|`-f`|`--force`|移除迁移前通常需要验证其是否已经应用到了数据库。使用该参数可跳过验证。|

### dotnet ef migrations script

根据迁移（段）生成 SQL 脚本

参数：

|||
|:---:|:---:|
|`<FROM>`|起始迁移。默认是 0（初始化数据库）。|
|`TO`|结束迁移。默认是最近一个迁移。|

选项：

||||
|:---:|:---:|:---:|
|`-o`|`--output <FILE>`|脚本输出到的文件。|
|`-i`|`--idempotent`|为涉及到的迁移生成一个可以在数据库上使用的脚本。|