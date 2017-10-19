# 序列

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

序列用于在数据库中生成连续的数值。序列与数据表无关联。

## 惯例

按照惯例不会在模型中引入序列。

## 数据注解

不能使用数据注解来配置序列。

## 流式 API

可以使用流式 API 来在模型中创建序列。

```C#
class MyContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("OrderNumbers");
    }
}

public class Order
{
    public int OrderId { get; set; }
    public int OrderNo { get; set; }
    public string Url { get; set; }
}
```

还可以配置序列的附加方面，比如其模式，开始值以及增量 等等。

```C#
class MyContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("OrderNumbers", schema: "shared")
            .StartsAt(1000)
            .IncrementsBy(5);
    }
}
```

引入序列之后，就可以使用它来为你模型中的属性生成值。例如，可以使用 [默认值](./I、默认值.md) 来在数据库中插入序列的下一个值。

```C#
class MyContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence<int>("OrderNumbers", schema: "shared")
            .StartsAt(1000)
            .IncrementsBy(5);

        modelBuilder.Entity<Order>()
            .Property(o => o.OrderNo)
            .HasDefaultValueSql("NEXT VALUE FOR shared.OrderNumbers");
    }
}

public class Order
{
    public int OrderId { get; set; }
    public int OrderNo { get; set; }
    public string Url { get; set; }
}
```