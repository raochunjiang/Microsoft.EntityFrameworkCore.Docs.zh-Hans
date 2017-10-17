# 继承

EF 模型中的继承被用来控制实体类型继承在数据库中的表现方式。

## 惯例

按照惯例，由数据库提供程序决定继承在数据库中的表示。查看 [继承（关系数据库）](./P、关系数据库建模/M、继承（关系数据库）.md) 以了解关系数据库提供程序是如何处理继承的。

EF 将只会在两个或多个继承类型被显式包含到模型中时建立继承，它不会扫描未包含在模型中的基础或派生类型。可以通过为类层次结构中的每个类型暴露 `DbSet` 来将类型包含到模型中。

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

如果你不希望暴露类层次中的某个或某些类型的 `DbSet`，你可以使用流式 API 来确保他们包含在模型中。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RssBlog>();
    }
}
```

## 数据注解

不能使用数据注解来配置继承。

## 流式 API

用于配置继承的流式 API 依赖于你使用的数据库提供程序。查看  [继承（关系数据库）](./P、关系数据库建模/M、继承（关系数据库）.md) 可了解在关系数据库提供程序下你能做的配置。