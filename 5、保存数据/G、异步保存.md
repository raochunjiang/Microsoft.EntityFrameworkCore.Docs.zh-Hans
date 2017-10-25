# 异步保存

异步保存能够在将变更写入到数据库时避免线程阻塞。这对于避免冻结胖客户端应用程序（thick-client application）的 UI 来说很有用。异步操作还能够提升 Web 应用程序的生产能力，在数据库执行查询时线程可以被空出来为其他请求服务。更多信息请查阅 [C#异步编程](https://docs.microsoft.com/zh-cn/dotnet/csharp/async)。
