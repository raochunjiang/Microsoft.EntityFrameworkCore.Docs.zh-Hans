# 主键

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

主键约束是为每个实体类型的键引入的。

## 惯例

按照惯例，数据库中的主键会被命名为 `PK_<实体类型名称>`。

## 数据注解

不能使用数据注解来配置特定于关系数据库的主键。

## 流式 API

可以使用流式 API 来配置数据库中的主键约束名称。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasKey(b => b.BlogId)
            .HasName("PrimaryKey_BlogId");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

