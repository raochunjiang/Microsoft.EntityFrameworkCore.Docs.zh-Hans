# 安装 EF Core 

## 先决条件

为了开发 .NET Core 2.0 应用程序（包括目标为 .NET Core 的 ASP.NET Core 2.0 应用程序），你需要下载和安装与你的平台相应的 [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core)。**这是必须的，即使你已经安装了 Visual Studio 2017 version 15.3。**

为了使用 EF Core 2.0 或 .NET Core 2.0 平台之外的其他 .NET Standard 2.0 库（比如 .NET Framework 4.6.1 或更高版本），你需要某个能辨识 .NET Standard 2.0 及其兼容框架的 NuGet 版本。这里为你提供一些方式来获得它：

* 安装 Visual Studio 2017 version 15.3
* 如果你正在使用 Visual Studio 2015，[下载和升级 NuGet 客户端到版本 3.6.0]

由早起版本的 Visual Studio 创建的指向 .NET Framework 的项目可能需要其他更改才能与 .NET Standard 2.0 库兼容：

* 编辑项目文件，确保以下条目出现在初始属性组中：

```XML
<AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
```

* 对于测试项目，还要确保有以下条目：

```XML
<GenerateBindingRedirectsOutputType>true</GenerateBindingRedirectsOutputType>
```


## 获取