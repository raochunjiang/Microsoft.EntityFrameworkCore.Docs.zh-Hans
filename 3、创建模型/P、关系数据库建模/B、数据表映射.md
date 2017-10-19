# 数据表映射

> 注意
>
> 当前章节中涉及的配置一般适用于关系数据库。这里展示的扩展方法在你安装了关系数据库提供程序之后就能获得（由`Microsoft.EntityFrmeworkCore.Relational` 程序包共享）。

表映射用于标识应该从数据库查询或写入哪些表数据。

## 惯例

按照惯例，一旦在派生的上下文中通过 `DbSet<TEntity>` 属性暴露了实体，该实体就会被设置为映射到与该属性同名的数据表上。如果给定的实体不是通过 `DbSet<TEntity>` 包含到模型中的，则使用该实体的类型名称。

## 数据注解

可以使用数据注解来配置类型所映射的数据表。

```C#
using System.ComponentModel.DataAnnotations.Schema;
```

```C#
[Table("blogs")]
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

还可以指定数据表所属的模式。

```C#
[Table("blogs",Schema = "blogging")]
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

## 流式 API

可以使用流式 API 来配置类型所映射的数据表。

```C#
using Microsoft.EntityFrameworkCore;
```

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .ToTable("blogs");
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
}
```

也可以通过这种方式来指定数据表所属的模式。

```C#
        modelBuilder.Entity<Blog>()
            .ToTable("blogs", schema: "blogging");
```