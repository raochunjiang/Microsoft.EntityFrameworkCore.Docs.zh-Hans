# 客户端 VS 服务端评估

Entity Framework Core 支持在客户端评估查询的一部分，同时也将其一部分推送给服务端（数据库）评估。数据库提供程序会决定查询的哪些部分交由数据库评估。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Querying)。

## 客户端评估

