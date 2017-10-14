# 键（主键）

键被作为每个实体实例主要唯一标识。当使用的是关系数据库时，键被映射为_主键_这个概念。你还可以将唯一标识配置为非主键（详见 [替代键（备用关键字）](./L、替代键（备用关键字）.md)）。

## 惯例

按照惯例，名为 `Id` 或者 `<当前类型名称>Id` 的属性都将被配置为实体的键。

```C#
class Car
{
    public string Id { get; set; }

    public string Make { get; set; }
    public string Model { get; set; }
}
```

```C#
class Car
{
    public string CarId { get; set; }

    public string Make { get; set; }
    public string Model { get; set; }
}
```

## 数据注解

可以使用数据注解将单一属性配置为实体的键。

```C#
class Car
{
    [Key]
    public string LicensePlate { get; set; }

    public string Make { get; set; }
    public string Model { get; set; }
}
```

## 流式 API

可以使用流式 API 将单一属性配置为实体的键。

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasKey(c => c.LicensePlate);
    }
}

class Car
{
    public string LicensePlate { get; set; }

    public string Make { get; set; }
    public string Model { get; set; }
}
```

还可以使用流式 API 将多个属性配置为实体的键（通常被称为_组合件_）。组合件只能通过流式 API 来配置 - 惯例不会设置组合件，数据注解也无法配置组合键。

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasKey(c => new { c.State, c.LicensePlate });
    }
}

class Car
{
    public string State { get; set; }
    public string LicensePlate { get; set; }

    public string Make { get; set; }
    public string Model { get; set; }
}
```