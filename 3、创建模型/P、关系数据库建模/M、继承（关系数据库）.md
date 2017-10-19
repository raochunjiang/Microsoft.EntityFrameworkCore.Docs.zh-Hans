# 继承（关系数据库）

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

EF 模型中的继承被用来控制实体类型继承在数据库中的表现方式。

## 惯例

按照惯例，使用每个层次结构一张表（table-per-hierarchy，TPH）的模式来进行映射。TPH 使用单一的表来存储类层次结构中所有类型的数据，然后使用_识别列_来辨别每一行所对应的实体类型。

EF 将只会在两个或多个继承类型被显式包含到模型中时建立继承（详见 [继承](../M、继承.md)）。

以下样例展示了一个简单类层次结构的场景，其中的数据使用 TPH 模式被存储到关系数据库中。_识别列_ 标识了每一行所存储的 _Blog_ 类型。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<RssBlog> RssBlogs { get; set; }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}

public class RssBlog : Blog
{
    public string RssUrl { get; set; }
}
```

![类层次结构TPH数据](./inheritance-tph-data.png)

## 数据注解

不能使用数据注解来配置继承。

## 流式 API

可以使用流式 API 来配置_识别列_的类型和名称，以及用于辨别类层次结构中的类型的值。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasDiscriminator<string>("blog_type")
            .HasValue<Blog>("blog_base")
            .HasValue<RssBlog>("blog_rss");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}

public class RssBlog : Blog
{
    public string RssUrl { get; set; }
}
```