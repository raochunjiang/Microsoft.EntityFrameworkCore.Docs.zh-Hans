# 包含和排除属性

将属性包含到模型中意味着 EF 将获得该属性的元数据，并且将尝试从数据库读取该属性的值或将该属性的值写入到数据库。

## 惯例

按照惯例，具有 getter 和 setter 访问器的公共（public）属性将被包含在模型中。

## 数据注解

可以使用数据注解将属性从模型中排除。

```C#
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    [NotMapped]
    public DateTime LoadedFromDatabase { get; set; }
}
```

## 流式 API

可以使用流式 API 将属性从模型中排除。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Ignore(b => b.LoadedFromDatabase);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public DateTime LoadedFromDatabase { get; set; }
}
```