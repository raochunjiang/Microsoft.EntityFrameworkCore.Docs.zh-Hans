# 替代键（备用关键字）

替代键充当了每个实体实例主键之外的备用唯一关键字。替代键可用于指定关系。在使用关系数据库的时候，替代键映射为备用关键字列上的唯一索引/约束这一概念，一个或者多个外键约束将引用这个（这些）列。

> 提示
>
> 如果你只想要实施某个列的唯一性，那么你想要的应该是唯一索引，而不是替代键，请查看 [索引](./K、索引.md)。在 EF 中，替代键比索引提供了更丰富的功能，因为它们可以用作外键的目标。

替代键通常在你需要的时候才被引入，并且你不需要手动配置他们。查看 [惯例](#惯例) 以了解更多信息。

## 惯例

按照惯例，当你标识了某个非主键的属性时就会引入替代键，它将被用作关系的目标属性。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .HasForeignKey(p => p.BlogUrl)
            .HasPrincipalKey(b => b.Url);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; set; }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public string BlogUrl { get; set; }
    public Blog Blog { get; set; }
}
```

## 数据注解

不能通过数据注解来配置替代键。

## 流式 API

可以使用流式 API 来将单一属性配置为替代键。

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasAlternateKey(c => c.LicensePlate);
    }
}

class Car
{
    public int CarId { get; set; }
    public string LicensePlate { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
}
```

也可以使用流式 API 来将多个属性配置为替代键（通常称为组合替代键）。

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasAlternateKey(c => new { c.State, c.LicensePlate });
    }
}

class Car
{
    public int CarId { get; set; }
    public string State { get; set; }
    public string LicensePlate { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
}
```