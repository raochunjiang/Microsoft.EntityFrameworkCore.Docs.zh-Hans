# 关系

关系定义了两个实体之间的关联方式。在关系数据库中，关系表现为外键约束。

> 注意
>
> 本文中的大部分样例使用了一对多的关系来展示相关概念。查看文末的 [其他关系模式](#其他关系模式) 可以了解一对一和多对多关系。

## 术语定义

描述关系需要用到大量术语

* **依赖实体**：包含外键属性的实体。有时被称作关系中的“子”。
* **主实体**：包含主键/替代键（备用关键字）属性的实体。有时被称作关系中的“父”。
* **外键**：依赖实体中用于存储关联实体的主键属性值的属性。
* **主键**：唯一标识主实体的属性。可以是主键或替代键（备用关键字）。
* **导航属性**：在主实体或依赖实体上定义的包含关联实体的引用的属性。
    - **集合导航属性**：包含多个关联实体的引用的导航属性。
    - **引用导航属性**：维持单一关联实体的引用的导航属性。
    - **逆向导航属性**：当讨论一个特定的导航属性时，该术语指的是关系另一端的导航属性。

以下列出的代码显示了 `Blog` 和 `Post`之间的一对多关系。

* `Post` 是依赖实体
* `Blog` 是主实体
* `Post.BlogId` 是外键
* `Blog.BlogId` 是主键（这种情况下它是主键，不是替代键）
* `Post.Bolg` 是引用导航属性
* `Blog.Posts` 是集合导航属性
* `Post.Blog` 是 `Blog.Posts` 的逆向导航属性（反之亦然）

```C#
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

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

## 惯例

按照惯例，当在一个类型上发现了导航属性时就会创建一个关系。如果一个属性指向的类型不能被当前数据库提供程序映射为标量类型，EF 就认为它是导航属性。

>提示
>
> 标量数据类型是内部没有分量的值类型，通常包括数字类型、字符类型、日期类型和布尔类型等等。

> 注意
>
> 按照惯例发现的关系总是指向主实体的主键的。要指向替代键的话，就要使用 [流式 API](#流式 API) 做更多的配置。

### 完整定义的关系

关系最普遍的模式就是在关系的两端都定义导航属性，并且在依赖实体类型中定义一个外键属性。

* 如果在两个类型中找到一对导航属性，则他们将被配置为同一关系的逆向导航属性。

* 如果依赖实体包含一个名为 `<主键属性名>`、`<导航属性名><主键属性名>` 或者 `<主实体类名><主键属性名>` 的属性，则它被配置为外键。

```C#
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

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

> 警告
>
> 如果两个类型之间定义了多个导航属性（也就是说不止一对指向对方的独立导航），则按照惯例不会创建任何一对关系，你需要手动配置他们以标识导航属性之间如何配对。

### 无外键属性

尽管建议要在依赖实体中定义外键属性，但是它不是必须的。如果没有找到外键属性，EF 就会引入名为 `<导航属性名><主键属性名>` 的影子属性（详见[影子属性](./I、影子属性.md)）。

```C#
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

    public Blog Blog { get; set; }
}
```

### 单一导航属性

仅包含一个导航属性（没有逆向导航，也没有外键属性）就足够按照惯例定义关系了。你也可以定义单一的导航属性和一个外键属性。

```C#
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
}
```

### 级联删除

按照惯例，对于必须的关系，级联删除会被设置为 _Cascade_，而对于可选的关系则设置为 _ClientSetNull_。_Cascade_ 意味着依赖实体也将被删除；_ClientSetNull_ 意味着未加载到内存中的依赖实体将保持不变，必须手动删除它或更新为指向其他合法的实体。对于已加载到内存中的实体，EF Core 将尝试设置他们的外键属性为 null。

查看 [必须的和可选的关系](#必须的和可选的关系) 可详细了解必须的关系和可选的关系。

查看 [级联删除](../5、保存数据/D、级联删除.md) 可详细了解不同的删除行为和惯例使用的默认行为。

## 数据注解

有两个数据注解可用于配置关系：`[ForeignKey]` 和 `[InverseProperty]`。

### [ForeignKey]

可以使用数据注解来配置指定哪个属性应该用作给定关系的外键属性。通常在按照惯例没有找到外键属性时才这样做。

```C#
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

    public int BlogForeignKey { get; set; }

    [ForeignKey("BlogForeignKey")]
    public Blog Blog { get; set; }
}
```

> 提示
>
> 可以将 [ForeignKey] 标注同时放到关系中的两个导航属性上。也可以不放在依赖实体类型的导航属性上。

### [InverseProperty]

可以使用数据注解来配置依赖实体和主实体上的导航属性的配对方式。通常在两个实体类型之间有多对导航属性时才这么做。

```C#
public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int AuthorUserId { get; set; }
    public User Author { get; set; }

    public int ContributorUserId { get; set; }
    public User Contributor { get; set; }
}

public class User
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [InverseProperty("Author")]
    public List<Post> AuthoredPosts { get; set; }

    [InverseProperty("Contributor")]
    public List<Post> ContributedToPosts { get; set; }
}
```

## 流式 API

要使用流式 API 配置关系的话，首先需要识别构成关系的导航属性。`HasOne` 或者 `HasMany` 能够标识出你要开始配置的实体类型的导航属性。然后链式调用 `WithOne` 或者 `WithMany` 来分辨逆向导航。`HasOne`/ `WithOne` 用于引用导航属性，`HasMany`/`WithMany` 用于集合导航属性。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts);
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

    public Blog Blog { get; set; }
}
```

### 单一导航属性

如果你只有一个导航属性，那么可以使用 `WithOne` 和 `WithMany` 的无参数重载。这表明在关系的另一端有一个概念上的引用或集合，但其实体类型不包含导航属性的定义。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasMany(b => b.Posts)
            .WithOne();
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
}
```

### 外键

可以使用流式 API 来配置哪个属性被用作给定关系的外键属性。

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
            .HasForeignKey(p => p.BlogForeignKey);
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

    public int BlogForeignKey { get; set; }
    public Blog Blog { get; set; }
}
```

以下列出的代码显示了如何配置一个组合键。

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasKey(c => new { c.State, c.LicensePlate });

        modelBuilder.Entity<RecordOfSale>()
            .HasOne(s => s.Car)
            .WithMany(c => c.SaleHistory)
            .HasForeignKey(s => new { s.CarState, s.CarLicensePlate });
    }
}

public class Car
{
    public string State { get; set; }
    public string LicensePlate { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }

    public List<RecordOfSale> SaleHistory { get; set; }
}

public class RecordOfSale
{
    public int RecordOfSaleId { get; set; }
    public DateTime DateSold { get; set; }
    public decimal Price { get; set; }

    public string CarState { get; set; }
    public string CarLicensePlate { get; set; }
    public Car Car { get; set; }
}
```

可以使用 `HasForeignKey(...)` 的字符串参数重载来将影子属性配置为外键（详见 [影子属性](./I、影子属性.md)）。我们建议在将影子属性用作外键之前显式将其添加到模型中（如下所示）。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Add the shadow property to the model
        modelBuilder.Entity<Post>()
            .Property<int>("BlogForeignKey");

        // Use the shadow property as a foreign key
        modelBuilder.Entity<Post>()
            .HasOne(p => p.Blog)
            .WithMany(b => b.Posts)
            .HasForeignKey("BlogForeignKey");
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

    public Blog Blog { get; set; }
}
```

### 主键

如果想要让外键引用与主键不同的属性，可以使用流式 API 来配置关系中的主键属性。被配置为主键的属性将被自动设置为替代键（详见[替代键（备用关键字）](./L、替代键（备用关键字）.md)）

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecordOfSale>()
            .HasOne(s => s.Car)
            .WithMany(c => c.SaleHistory)
            .HasForeignKey(s => s.CarLicensePlate)
            .HasPrincipalKey(c => c.LicensePlate);
    }
}

public class Car
{
    public int CarId { get; set; }
    public string LicensePlate { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }

    public List<RecordOfSale> SaleHistory { get; set; }
}

public class RecordOfSale
{
    public int RecordOfSaleId { get; set; }
    public DateTime DateSold { get; set; }
    public decimal Price { get; set; }

    public string CarLicensePlate { get; set; }
    public Car Car { get; set; }
}
```

以下列出的代码显示了如何配置组合主键。

```C#
class MyContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecordOfSale>()
            .HasOne(s => s.Car)
            .WithMany(c => c.SaleHistory)
            .HasForeignKey(s => new { s.CarState, s.CarLicensePlate })
            .HasPrincipalKey(c => new { c.State, c.LicensePlate });
    }
}

public class Car
{
    public int CarId { get; set; }
    public string State { get; set; }
    public string LicensePlate { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }

    public List<RecordOfSale> SaleHistory { get; set; }
}

public class RecordOfSale
{
    public int RecordOfSaleId { get; set; }
    public DateTime DateSold { get; set; }
    public decimal Price { get; set; }

    public string CarState { get; set; }
    public string CarLicensePlate { get; set; }
    public Car Car { get; set; }
}
```

> 警告
>
> 指定主键属性的顺序必须与指定外键的顺序相匹配。

### 必须的和可选的关系

可以使用流式 API 来将一个关系配置为必须或者可选。从根本上说，这是将外键属性配置为必须或者可选。这在你使用影子状态外键时是很有用的。如果在你的实体类型中有一个外键属性，那么关系的必要性是基于外键属性的必要性来决定的（详见 [必须的和可选的属性](./F、必须的和可选的属性.md)）。

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
            .IsRequired();
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

    public Blog Blog { get; set; }
}
```

### 级联删除

可以使用流式 API 来显式配置给定关系的级联除行为。

查看 _保存数据_ 章节下的 [级联删除](../5、保存数据/D、级联删除.md) 可了解每个级联删除行为的详细论述。

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
            .OnDelete(DeleteBehavior.Cascade);
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

    public int? BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

## 其他关系模式

### 一对一

一对一关系在关系两端都有一个引用导航属性。他们遵循了与一对多关系相同的惯例，只是在外键属性上引入了唯一索引以确保只有一个依赖与彼此的主键关联。

```C#
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public BlogImage BlogImage { get; set; }
}

public class BlogImage
{
    public int BlogImageId { get; set; }
    public byte[] Image { get; set; }
    public string Caption { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

> 注意
>
> EF 会基于其检测外键属性的功能选择其中一个实体作为依赖实体。如果选择了错误的依赖实体，你可以使用流式 API 来修正它。

当使用流式 API 配置关系时，可以使用 `HasOne` 和 `WithOne` 方法。

在配置外键的时候，你需要指定依赖实体类型 - 注意以下列出代码中提供给 `HasForeignKey` 方法的泛型参数。在一对多关系中，引用导航指向的是依赖实体，集合导航指向的是主实体，这是很清晰的。但在一对一关系中却并非如此 - 因此需要显示定义它。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BlogImage> BlogImages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasOne(p => p.BlogImage)
            .WithOne(i => i.Blog)
            .HasForeignKey<BlogImage>(b => b.BlogForeignKey);
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public BlogImage BlogImage { get; set; }
}

public class BlogImage
{
    public int BlogImageId { get; set; }
    public byte[] Image { get; set; }
    public string Caption { get; set; }

    public int BlogForeignKey { get; set; }
    public Blog Blog { get; set; }
}
```

### 多对多

目前还不支持没有实体类型充当连接表的多对多关系。但是，你可以通过包含一个用于充当连接表的实体类型来描绘多对多关系，将其映射为两个独立的一对多关系。

```C#
class MyContext : DbContext
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostTag>()
            .HasKey(t => new { t.PostId, t.TagId });

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostTags)
            .HasForeignKey(pt => pt.PostId);

        modelBuilder.Entity<PostTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.PostTags)
            .HasForeignKey(pt => pt.TagId);
    }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public List<PostTag> PostTags { get; set; }
}

public class Tag
{
    public string TagId { get; set; }

    public List<PostTag> PostTags { get; set; }
}

public class PostTag
{
    public int PostId { get; set; }
    public Post Post { get; set; }

    public string TagId { get; set; }
    public Tag Tag { get; set; }
}
```