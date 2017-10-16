# 索引

索引是在大多数数据存储中通用的概念。尽管其在不同的数据存储中的实现可能有所不同，但它们都是用来提高基于数据列（或者数据列集合）的查询效率的。

## 惯例

按照惯例，每个（或者每一组）被用作外键的属性都会创建对应的索引。

## 数据注解

不能使用数据注解创建索引。

## 流式 API

可以使用流式 API 来在单一属性上指定索引。默认情况下，这些不是唯一索引。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasIndex(b => b.Url);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

还可以将索引指定为唯一索引，这意味在给定的属性上两个实体不会具有相同的值。

```C#
        modelBuilder.Entity<Blog>()
            .HasIndex(b => b.Url)
            .IsUnique();
```

还可以在多个数据列上指定索引。

```C#
class MyContext : DbContext
{
    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasIndex(p => new { p.FirstName, p.LastName });
    }
}

public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

> 提示
>
> 每个独立属性组上只能有一个索引。如果你使用流式 API在已定义了索引的属性组上配置索引，不管是惯例定义还是前面所述的配置，那么你就更改了索引的定义。如果你在已按照惯例创建的索引上做更多配置，这会很有用。