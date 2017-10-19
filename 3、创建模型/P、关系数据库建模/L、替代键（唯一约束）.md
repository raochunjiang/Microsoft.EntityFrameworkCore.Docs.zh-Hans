# 替代键（唯一约束）

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

唯一约束是为模型中的替代键引入的。

## 惯例

按照惯例，为替代键引入的索引和约束都被命名为 `AX_<实体类型名称>_<属性名称>`。对于组合替代键，`<属性名>` 为用下划线分隔的属性名。

## 数据注解

不能使用数据注解来配置唯一索引。

## 流式 API

可以使用流式 API 来为替代键配置索引和约束的名称。

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasAlternateKey(c => c.LicensePlate)
            .HasName("AlternateKey_LicensePlate");
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