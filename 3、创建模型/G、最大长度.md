# 最大长度

配置最大长度为数据存储提供了有关示意，示意其为给定属性使用合适的数据类型。最大长度仅被应用于数组数据类型，比如 `string` 和 `byte[]`。

> 注意
>
> Entity Framework 在将数据传递给数据库提供程序之前不会做最大长度验证。是否合适是由数据库提供程序或数据储存验证的。比如，使用的是 SQL Server 时，超出最大长度将由于底层数据列的数据类型不允许数据超出而导致异常。

## 惯例

按照惯例，由数据库提供程序来为属性选择一个合适的数据类型。对于具有长度的属性，数据库提供程序通常会选择一个允许数据最长长度的数据类型。比如，如果属性对应的列被用作键，那么 Microsoft  SQL  Server 将为其使用 `nvarchar(max)`。

## 数据注解

可以使用数据注解来配置属性的最大长度。在该示例中，使用 SQL Server 的结果是对属性使用 `nvarchar(500)` 数据类型。

```C#
public class Blog
{
    public int BlogId { get; set; }
    [MaxLength(500)]
    public string Url { get; set; }
}
```

## 流式 API

可以使用流式 API 来配置属性的最大长度。在该示例中，使用 SQL Server 的结果是对属性使用 `nvarchar(500)` 数据类型。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .HasMaxLength(500);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}

```