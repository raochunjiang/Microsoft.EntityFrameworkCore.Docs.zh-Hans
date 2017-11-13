# EFSecondLevelCache.Core 扩展

> 注意
>
> 该扩展没有被作为 Entity Framework Core 项目的一部分来维护。当考虑第三方提扩展的时候，一定要评估其质量、许可、支持情况等等以确保它们符合你的需求。

二级缓存库。二级缓存是一个查询缓存。EF 命令的执行结果会被存储在缓存中，这样的话相同的命令就只会遍历缓存中的数据，而不是再次针对数据库进行查询。

以下资源有助于你入门使用 EFSecondLevelCache.Core。

* [EFSecondLevelCache.Core GitHub 仓库](https://github.com/VahidN/EFSecondLevelCache.Core/)