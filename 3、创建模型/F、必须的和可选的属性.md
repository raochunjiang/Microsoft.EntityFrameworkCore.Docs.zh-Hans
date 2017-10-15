# 必须的和可选的属性

如果属性值可以设置为 `null`，则其被认为是可选的（可空的）。相反，如果属性值设置为 `null` 是非法的，那么它就被认为是必须的属性。

## 惯例

按照惯例，值可以是 null 的运行时类型（`string`、`int?`、`byte[]` 等等）属性将被配置为可选。值不能为 `null` 的运行时类型（`int`、`decimal`、`bool` 等等）属性则将被配置为必须。

> 注意
>
> 值不能为 `null` 的运行时类型属性是无法被配置为可选的。这样的属性将总是被 Entity Framework 认为是必须的。

## 数据注解

可以使用数据注解将属性标注为必须。

```C#
public class Blog
{
    public int BlogId { get; set; }
    [Required]
    public string Url { get; set; }
}
```

## 流式 API

可以使用流式 API 将属性标注为必须。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .IsRequired();
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```