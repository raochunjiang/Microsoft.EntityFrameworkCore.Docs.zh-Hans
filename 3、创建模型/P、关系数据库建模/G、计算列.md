# 计算列

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

计算列是值其值是在数据库中被计算出来的列。计算列可以利用数据表中的其他列来计算它的值。

## 惯例

按照惯例不会在模型中创建计算列。

## 数据注解

不能使用数据注解来配置计算列。

## 流式 API

可以使用流式 API 来指定计算列及其算法。

```C#
class MyContext : DbContext
{
    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .Property(p => p.DisplayName)
            .HasComputedColumnSql("[LastName] + ', ' + [FirstName]");
    }
}

public class Person
{
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
}
```