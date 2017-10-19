# 索引

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

关系数据库中的索引映射为与 Entity Framework Core 中相同概念的索引。

## 惯例

按照惯例，索引被命名为 `IX_<实体类型名称>_<属性名称>`。对于组合索引，`<属性名>` 为用下划线分隔的属性名。

## 数据注解

不能使用数据注解来配置索引。

## 流式 API

可以使用流式 API 来配置索引的名称。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasIndex(b => b.Url)
            .HasName("Index_Url");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```