# 创建模型

Entity Framework 使用一组惯例来构建基于实体类型的形状的模型。你可以指定额外的配置来补充或者覆盖被惯例所发现的内容

本文介绍的配置可以应用于针对任何数据存储的模型，并且可以应用于任何关系数据库。提供程序还可以启用为特定数据存储指定的的配置。查看 [数据库提供程序](../../7、数据库提供程序/A、数据库提供程序.md) 可了解提供程序指定配置的相关内容。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples)。

## 方法和配置

### 流式API

你可以重写派生上下文类型中的 `OnModelCreating` 方法，使用 `ModelBuilder API` 来配置你的模型。这是模型配置的最强大的方法，它允许你在不修改实体类型的情况下为它们指定配置。流式 API 配置具有最高的优先级，所以它会覆盖掉惯例和数据注解所做的配置。

```C#
    class MyContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>()
                .Property(b => b.Url)
                .IsRequired();
        }
    }
```

### 数据注解

你还可以将特性（我们称其为 **数据注解**）应用到你的类型和属性上。数据注解会覆盖惯例，但会被流式 API 所做的配置给覆盖。

```C#
    public class Blog
    {
        public int BlogId { get; set; }
        [Required]
        public string Url { get; set; }
    }
```