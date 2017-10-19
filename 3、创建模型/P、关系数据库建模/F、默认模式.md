# 默认模式

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

默认模式是在未显示配置对象的数据库模式时其将被创建到的数据库模式。

## 惯例

按照惯例，数据库提供程序将会选择最合适的默认模式。比如，Microsoft SQL Server 会使用 `dbo` 模式，而 SQLite 不使用模式（因为 SQLite 根本就不支持模式）。

## 数据注解

不能使用数据注解来配置默认模式。

## 流式 API

可以使用流式 API 来配置默认模式。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("blogging");
    }
}
```