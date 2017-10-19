# 数据类型

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

数据类型指的是实体类型属性所映射到的数据列的特定于数据库的类型。

## 惯例

按照惯例，数据库提供程序会基于属性的运行时类型选择相应的数据类型。提供程序还会考虑其他元数据，比如已配置的 [最大长度（Maximum Length）](../G、最大长度.md)、属性是否主键的一部分等等。

例如，SQL Server 为 `DateTime` 属性使用 `datetime2(7)` ，为 `string` 属性使用 `nvarchar(max)`（`string` 主键属性使用的是 `nvarchar(450)`）。

## 数据注解

可以使用数据注解来为数据列指定具体的数据类型。

```C#
public class Blog
{
    public int BlogId { get; set; }
    [Column(TypeName = "varchar(200)")]
    public string Url { get; set; }
}
```

```C#
public class Blog
{
    public int BlogId { get; set; }
    [Column(TypeName = "varchar(200)")]
    public string Url { get; set; }
}
```

## 流式 API

可以使用流式 API 来为数据列指定具体的数据类型。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .HasColumnType("varchar(200)");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```