# 安装 EF Core 

## 先决条件

为了开发 .NET Core 2.0 应用程序（包括目标为 .NET Core 的 ASP.NET Core 2.0 应用程序），你需要下载和安装与你的平台相应的 [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core)。**这是必须的，即使你已经安装了 Visual Studio 2017 version 15.3。**

为了使用 EF Core 2.0 或 .NET Core 2.0 平台之外的其他 .NET Standard 2.0 库（比如 .NET Framework 4.6.1 或更高版本），你需要某个能辨识 .NET Standard 2.0 及其兼容框架的 NuGet 版本。这里为你提供一些方式来获得它：

* 安装 Visual Studio 2017 version 15.3
* 如果你正在使用 Visual Studio 2015，[下载和升级 NuGet 客户端到版本 3.6.0](https://www.nuget.org/downloads)

由早起版本的 Visual Studio 创建的指向 .NET Framework 的项目可能需要其他更改才能与 .NET Standard 2.0 库兼容：

* 编辑项目文件，确保以下条目出现在初始属性组中：

```XML
<AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
```

* 对于测试项目，还要确保有以下条目：

```XML
<GenerateBindingRedirectsOutputType>true</GenerateBindingRedirectsOutputType>
```

## 获取二进制文件

建议通过 NuGet 安装 EF Core 数据库提供程序以将 EF Core 运行时库添加到一个应用程序中。

除了运行时库之外，你还可以安装一些工具以在你的项目设计时更轻松地执行一些 EF Core 相关的任务（比如创建和应用迁移，或者基于现有的数据库创建模型）。

>**提示**
>
>如果你要升级一个使用第三方数据库提供程序的应用程序，记得要及时检查提供程序的更新以确保其与你要使用的 EF Core 版本兼容。比如，旧版本的数据库提供程序可能与 EF Core 2.0 运行时是不兼容的。

>**提示**
>
>ASP.NET Core 2.0 应用程序可以直接使用 EF Core 2.0，无需依赖于额外的第三方数据库提供程序。而旧版本的 ASP.NET Core 应用程序则需要升级到 ASP.NET Core 2.0 以使用 EF Core 2.0

### 使用 .NET Core 命令行接口（CLI）进行跨平台开发

你可以选择结合使用你喜欢的编辑器和 `dotnet` [CLI 命令](https://docs.microsoft.com/en-us/dotnet/core/tools/) 来开发 .NET Core 应用程序，或者选择使用一个 IDE（Integrated Development Environment，集成开发环境），比如 Visual Studio，Visual Studio For Mac 或者 Visual Studio Code。

>**重要提示**
>
>目标为 .NET Core 的应用程序需要指定版本的 Visual Studio。比如 .NET Core 1.x 开发要求使用 Visual Studio 2017，而 .NET Core 2.0 开发则要求使用 Visual Studio 2017 version 15.3。

为了安装或升级 .NET Core 跨平台应用程序中的 SQL Server 提供程序，你可以打开应用程序目录，然后在命令行中运行以下命令：

```console
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

你可以在 `dotnet add package` 命令中指定要装的版本，使用 `-v` 修改版本号即可。比如安装 EF Core 2.0 程序包，将 `-v 2.0.0` 追加到命令后面即可。

EF Core 包含了一组[额外的`dotnet` CLI 命令](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)，由 `dotnet ef` 开头。为了使用 `dotnet ef` 命令行命令，应用程序的 `.csproj` 文件需要包含以下条目：

```xml
<ItemGroup>
  <DotNetCliToolReference Include="Microsoft.EntityFramework.Tools.DotNet" version="2.0.0"/>
</ItemGroup>
```

EF Core 的 .NET Core CLI 命令工具还需要一个独立的程序包，这个程序包叫做 `Microsoft.EntityFramewrokCore.Design`。你可以通过以下命令轻松地将它添加到项目中：

```console
dotnet add package Microsoft.EntityFrameworkCore.Design
```

>**重要提示**
>
>请确保所上述工具包的版本总是与运行时程序包的主版本匹配。

### Visual Studio 开发

你可以使用 Visual Studio 开发各种不同类型的 .NET Core、.NET Framework 或其他 EF Core 所支持平台的应用程序。

在 Visual Studio 中你还可以通过另外两种方式来将 EF Core 数据库提供程序添加到应用程序中：

#### 使用 [NuGet 程序包管理器用户接口](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui)

* 选择菜单 **项目 > 管理 NuGet 程序包**
* 点击 **浏览** 或 **更新** 标签
* 选择 `Microsoft.EntityFrameworkCore.SqlServer` 程序包及其目标版本，然后确定

#### 使用 [NuGet 程序包管理控制台（PMC）](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console)

* 选择菜单 **工具 >  NuGet 包管理器 > 程序包管理器控制台**
* 在 PMC 中输入和运行以下命令：

```PowerShell
Install-Package Microsoft.EntityFrameworkCore.SqlServer
```

* 如果已经安装过程序包，你可以改用 `Update-Package` 命令来更新程序包到最近的版本。
* 你还可以使用 `-Version` 来修改程序包版本。比如安装 EF Core 2.0 程序包，将 `-v 2.0.0` 追加到命令后面即可。

### 工具

在 Visual Studio 中还有相应版本的 [可以在 PMC 中运行的 EF Core 命令](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/powershell)，其功能类似于 `dotnet ef` 命令，使用程序包管理器 UI 或 PMC 安装 `Microsoft.EntityFrameworkCore.Tools` 程序包即可使用它。

> **重要提示**
>
> 请确保所上述工具包的版本总是与运行时程序包的主版本匹配。

> **提示**
>
> 尽管在 Visual Studio 中可以使用 `dotnet ef` 命令，但这远不及相应的 PowerShell 版本方便：
> * 无需手动打开应用程序目录，它会自动基于 PMC 中选定的项目来工作。
> * 命令完成后，它会自动打开 Visual Studio 中由命令生成的文件。

> **重要提示**
>
> EF Core 2.0 弃用的程序包：如果你在现有的应用程序中升级 EF Core 到 2.0 版本，你可能需要手动移除一些旧的 EF Core 程序包。特别是数据库提供程序的设计时工具包，比如在 EF Core 2.0  中不再需要或者不再支持`Microsoft.EntityFrameworkCore.SqlServer.Design`，但是在更新其他程序包时他们不会被自动移除。