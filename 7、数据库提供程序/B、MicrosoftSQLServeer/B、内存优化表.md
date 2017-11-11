# SQL Server EF Core 数据库提供程序的内存优化表支持

> 注意
>
> 该功能从 EF Core 1.1 引入

[内存优化表](https://docs.microsoft.com/zh-cn/sql/relational-databases/in-memory-oltp/memory-optimized-tables) 是指 SQL Server 将整张数据表留存在内存中的功能，表数据的第二副本维持在磁盘上，但它仅用作持久化处理。内存优化表中的数据则在数据库恢复（比如数据库服务器重启）时从磁盘读取。

## 配置内存优化表

可以将实体所映射到的数据表指定为内存优化表，如此则可以在在使用 EF Core 创建和维护基于模型的数据库时（无论是通过迁移还是 `Database.EnsureCreated()`）为这些实体创建内存优化表。

```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .ForSqlServerIsMemoryOptimized();
}
```