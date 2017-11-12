# Entity Framework Core 工具集

Entity Framework Core 工具集能够在 EF Core 应用程序开发过程中对你提供帮助。它们主要用于通过数据库模式的逆向工程搭建 DbContext 和实体类型的基架，同时用于管理迁移。

在 Visual Studio 中，[EF Core 程序包管理控制台（Package Manager Console，PMC）工具集](https://docs.microsoft.com/zh-cn/ef/core/miscellaneous/cli/powershell) 提供了超棒的体验。通过 NuGet 的 [程序包管理控制台](https://docs.microsoft.com/nuget/tools/package-manager-console) 可以运行它们。这些工具可以用于 .NET Framework 和 .NET Core 项目。

[EF Core .NET 命令行工具集](https://docs.microsoft.com/zh-cn/ef/core/miscellaneous/cli/dotnet) 是 [.NET Core 命令行接口（Command-line Interface,CLI）工具集](https://docs.microsoft.com/dotnet/core/tools/) 的扩展，它是跨平台的，并且运行独立于 Visual Studio。这些工具需要一个 .NET Core SDK 项目（项目文件中类似于 `Sdk="Microsoft.NET.Sdk"` 的片段）。

上述两个工具集暴露了相似的功能。如果你正在使用 Visual Studio 进行开发，我们建议使用 PMC 工具集，因为它们提供了更完整的体验。

## 框架

工具集支持指向 .NET Framework 或 .NET Core 的项目。

如果你的项目指向其他框架（比如，Windows 通用（Universal Windows）或 Xamarin），我们建议你创建独立的 .NET Standard 项目来交叉指向其中一个已支持的框架。

比如说要交叉指向 .NET Core，右键点击项目后选择 **编辑*.csproj**，然后更新 `TargetFramework` 属性（如下所示。注意属性名改成了复数形式）。

```XML
<TargetFrameworks>netcoreapp2.0;netstandard2.0</TargetFrameworks>
```

如果你正在使用的是 .NET Standard 类库，同时启动项目指向 .NET Framework 或 .NET Core，就无需交叉指向。

## 启动和目标项目

任何时候你调用一个命令，都会牵涉到两个项目：目标项目和启动项目。

目标项目是任何文件添加到的地方（一些情况是移除文件）。

启动项目是执行项目代码时由工具模拟的。目标项目和启动项目可以是同一个项目。