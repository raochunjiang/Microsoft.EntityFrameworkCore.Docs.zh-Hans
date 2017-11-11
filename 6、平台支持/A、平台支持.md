# EF Core 支持的.NET 实现

我们希望在任何你可以编写 .NET 代码的地方都能获取到 EF Core，而我们也正在朝这个目标努力。以下列表为我们想要使用 EF Core 的每个 .NET 实现提供了引导。

EF Core 2.0 的目标是 [.NET Standard](https://docs.microsoft.com/zh-cn/dotnet/standard/net-standard)，因此它需要相应的 .NET Standard 实现来支持它。

|.NET 实现|支持状态|1.x的要求|2.x 的要求|
|:---:|:---:|:---:|:---:|
|.NET Core（[ASP.NET Core](../2、入门指南/E、ASP.NETCore/A、ASP.NETCore.md)，[控制台](../2、入门指南/D、.NETCore/A、.NETCore.md)等等）|**完全支持，推荐使用：** 已覆盖自动化测试并且已知的大部分应用程序都成功使用了它|[.NET Core SDK 1.x](https://www.microsoft.com/net/core/)|[.NET Core SDK 2.x](https://www.microsoft.com/net/core/)|
|.NET Framework（WinForm，WPF，ASP.NET，[控制台](https://docs.microsoft.com/en-us/ef/core/get-started/full-dotnet/index)等等）|**完全支持，推荐使用：** 已覆盖自动化测试并且已知的大部分应用程序都成功使用了它。该平台也可以获取 EF 6（查看 [EF Core 对比 EF6](https://docs.microsoft.com/zh-cn/ef/efcore-and-ef6/index) 以选择合适的技术。）|.NET Framework 4.5.1|.NET Framework 4.6.1|
|Mono & Xamarin|**正在完善 - 可能遇到的问题：** EF Core 团队和用户已经做了一些测试。早期采用者已经报告了一些成功案例，同时也提出了一些[遇到的问题](https://github.com/aspnet/entityframework/issues?q=is%3Aopen+is%3Aissue+label%3Aarea-xamarin)。随着后续测试可能会发现其他问题。尤需注意 Xamarin.IOS 中的一些限制，这些可能会导致使用 EF Core 2.0 开发的应用程序无法正常工作|Mono 4.6，Xamarin.iOS 10，Xamarin.Mac 3，Xamarin.Android 7|Mono 5.4，Xamarin.iOS 10.14，Xamarin.Mac 3.8，Xamarin.Android 7.5|
|Windows 通用（Universal Windows Platform，UWP）|**正在完善 - 可能遇到的问题：** EF Core 团队和用户已经做了一些测试。在使用 .NET Native 工具链编译时已报告了[一些问题](https://github.com/aspnet/entityframework/issues?utf8=%E2%9C%93&q=is%3Aopen%20is%3Aissue%20label%3Aarea-uwp%20)。（通常在发布构建时需要用到 .NET Native 工具链，在开发 Windows 应用商店应用时也需要用到该工具链。如果你没有使用 .NET Native，或者只想要体验一下，这些问题可能对你没什么影响）|[最新的 .NET UWP 5 程序包](https://www.nuget.org/packages/Microsoft.NETCore.UniversalWindowsPlatform/5.4.1)|[最新的 .NET UWP 6 程序包 [1]](https://www.nuget.org/packages/Microsoft.NETCore.UniversalWindowsPlatform/)|

[1] 该版本的 .NET UWP 添加了对 .NET Standard 2.0 的支持，同时包含 .NET Native 2.0，其中修复了目前报告的大部分兼容性问题，但测试显示 EF Core 2.0 仍然[遗留了一些问题](https://github.com/aspnet/EntityFrameworkCore/issues?q=is%3Aopen+is%3Aissue+milestone%3A2.0.1+label%3Aarea-uwp)，我们计划会在即将发布的补丁中解决这些问题。

对于任何与预期不符的组合，我们鼓励在 [EF Core 问题跟踪器](https://github.com/aspnet/entityframeworkcore/issues/new)中创建新到的问题，而对于 Xamarin 则在 [Xamarin 问题跟踪器](https://bugzilla.xamarin.com/newbug)中创建新问题。