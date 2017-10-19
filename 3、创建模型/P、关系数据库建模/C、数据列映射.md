# 数据列映射

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

列映射用于标识应该从数据库查询或写入哪些列数据。

## 惯例

按照惯例，实体类型属性会被设置为映射到与该属性同名的数据列上。

## 数据注解

可以使用数据注解来配置实体类型属性所映射的数据列。

```C#
public class Blog
{
    [Column("blog_id")]
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

## 流式 API

可以使用流式 API 来配置实体类型属性所映射的数据列。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.BlogId)
            .HasColumnName("blog_id");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

