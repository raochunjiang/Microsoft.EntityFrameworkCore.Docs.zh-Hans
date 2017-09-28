# EF Core 2.0 中的新功能

## 平台

### .NET Standard 2.0

包括 .NET Core 2.0 和 .NET Framework 4.6.1 支持。查看 [已支持的平台](../6、平台支持/A、平台支持.md) 以了解更多关于 EF Core 2.0 平台支持的详细信息。

## 建模

### 表隔离

现在我们已经可以将两个或多个实体类型映射到同一张数据表，他们将共享主键列，每一个数据行可以对应两个或多个实体对象。

为了使用表隔离，必须在共享同一张表的所有实体类型（引用主键的外键属性上）配置可识别的关系：

```C#
modelBuilder.Entity<Product>()
    .HasOne(e=> e.Details).WithOne(e => e.Id)
    .HasForeighKey<ProductDetails>(e => e.Id);
modelBuilder.Entity<Product>().ToTable("Products");
modelBuilder.Entity<ProductDetails>().ToTable)("Products");
```

### 附属类型

一个附属实体类型可以跟另一个附属实体类型共享同一个运行时类型。但是，由于无法通过运行时类型来识别该实体类型，所以必须从另一个实体类型导航到该实体类型。包含导航定义的实体类型为所有者。当针对所有者实体类型进行查询时，附属实体类型默认将被包含进来。

按照惯例，表隔离将为附属类型创建影子主键并且其将被映射到所有者类型的同一张表。使用附属类型跟 EF6 中使用复杂类型相似：

```C#
modelBuilder.Entity<Order>().OwnOne(p=> p.OrderDetails, cb=> 
    {
        cb.OwnOne(c => c.BillingAddress);
        cb.OwnOne(c => c.ShippingAddress);
    });

public class Order
{
    public int Id { get; set; }
    public OrderDetails OrderDetails { get; set; }
}

public class OrderDetails
{
    public StreetAddress BillingAddress { get; set; }
    public StreetAddress ShippingAddress { get; set; }
}

public class StreetAddress
{
    public string Street { get; set; }
    public string City { get; set; }
}
```

### 模型级查询过滤器

EF Core 2.0 包含一个我们称其为 **模型级查询过滤器** 的新功能。该功能允许在元数据模型（通常出现在 OnModelCreating 中）的实体类型上直接定义 LINQ 查询谓词（一个布尔表达式，通常被传递给 LINQ 查询操作）。这样的过滤器会自动被应用到一个 LINQ 查询涉及的任何一个实体类型上，包括被直接引用的实体类型，比如使用 Include 或者直接通过导航属性引用。该功能的一些通用应用场景包括：

* 软删除 - 一个定义了 IsDeleted 属性的实体类型。
* 多租户 - 一个定义了 TenantId 属性的实体类型。

以下示例为上述两个应用场景演示了该功能：

```C#
public class BloggingContext : DbContext
{
    public DbSet<Blog> Bolgs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public int TenantId { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>().HasQueryFilter(
            p => !p.IsDeleted
            &&  p.TenantId == this.TenantId
        );
    }
}
```

我们定义了一个模型级过滤器以让实体类型 `Post` 的实例实现多租户和软删除。注意这里使用了 DbContext 级别的属性 `TenantId`。模型级过滤器将使用合适的上下文实例上的值（即正在执行查询的上下文实例）。

如果需要，可以通过 IgnoreQueryFIlters() 操作来在个别 LINQ 查询操作时禁用过滤器。

#### 局限性

* 导航引用不允许使用模型级过滤器。可能根据反馈添加该功能。
* 过滤器只能在层级结构中的根实体类型上定义。

### 数据库标量函数映射

> 标量函数：返回值类型为除 TEXT、NTEXT 、IMAGE、CURSOR、 TIMESTAMP 和 TABLE 类型外的其它数据类型的函数。

Preview 2 包含一个来自 [Paul Middleton](https://github.com/pmiddleton) 的重要的贡献：将数据库标量函数映射到存根方法以在 LINQ 查询中使用它们并将他们解析为 SQL。

以下是关于该功能如何使用的简要描述：

在 `DbContext` 上声明一个静态方法并对该方法标注 `DbFunctionAttribute` 特性：

```C#
public class BloggingContext : DbContext
{
    [DbFunction]
    public static int PostReadCount(int blogId)
    {
        throw new Exception();
    }
}
```

像这样的方法会自动被注册。一旦一个方法被注册，你就可以在你查询的任何地方使用它：

```C#
var query = 
    from p in context.Posts
    where BloggingContext.PostReadCount(p.Id) > 5
    select p;
```

需要注意一些事：

* 按照惯例，在生成 SQL 的时候方法的名称会与函数的名称一致（该案例中是一个用户定义的函数），但是你可以在注册方法时重写该名称及其对应的数据库模式。
* 现在只支持标量函数。
* 你必须在数据库中创建对应的函数，EF Core 迁移不会关心这种函数的创建。

### 代码优先时自包含类型的配置

在代码优先设计时， EF6 可以通过继承 EntityTypeConfiguration 类型来封装指定实体类型的配置。EF Core 2.0 中我们保留了该模式：

```C#
class CustomerConfiguration : IEntityTypeConfiguration<Costomer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.AlternateKey);
        builder.Property(c => c.Name).HasMaxLength(200);
    }
}

// ...
// OnModelCreating
builder.ApplyConfiguration(new CustomerConfiguration());
```

## 高性能

### DbContext 池

在 ASP.NET Core 应用程序中使用 EF Core 的基础模式通常涉及到将自定义的 DbContext 类型注册到依赖注入系统，然后后续编码中通过控制器的构造方法参数获取该类型的实例。这意味着每个请求都会创建该 DbContext 的实例。

在 2.0 版本中我们介绍了一个新的方式来将自定义 DbContext 类型注册到依赖注入系统，其中明显介绍了一个复用 DbContext 实例的对象池。要使用 DbContext 对象池，可以在注册服务的时候将 AddDbContext 改为 AddDbContextPool：

```C#
services.AddDbContextPool<BolggingContext>(
    options => options.UseSqlServer(connectionString)
);
```

使用该方法后，在控制器请求 DbContext 的实例时我们将首先检查对象池中是否有可用的实例。一旦请求处理完成，DbContext 实例上的所有状态会被重置，然后实例本身会被归还到对象池中。

这在概念上跟 ADO.NET 提供程序上的连接池操作相似，并且具有节省 DbContext 实例的初始化成本的优势。

### 局限性

新方法引入了对 DbContext 的 OnConfigure 方法的一些功能限制。

    警告：
    如果从 DbContext 继承的类型中你要自己维护不能跨请求共享的状态（比如私有字段），那么请避免使用 DbContext 池。EF Core 只会重置 DbContext 实例被添加到对象池之前所知道的状态。

### 显式编译查询

这是第二个选择性加入的性能相关的功能，该功能被设计来满足高可扩展的场景。

在之前的 EF 版本和 LINQ to SQL 中就已经可以使用手动或显式编译查询了，这允许应用程序对翻译后的查询进行缓存，如此就可以只对查询计算一次，重复执行。

尽管常规情况下 EF Core 会根据查询表达式的哈希散列表示自动对查询进行编译和缓存，该机制可以用来绕过哈希计算和缓存查找，允许应用程序调用一个委托以使用现有的编译好的查询，如此以获得较小的性能提升。

```C#
// 创建显式编译查询
private static Func<CustomerContext,int,Customer> _customerById = 
    EF.CompileQuery((CustomerContext db,int id) => 
        db.Customers
            .Include(c => c.Address)
            .Single(c => c.Id == id));

// 调用显式编译查询
using(var db = new CustomerContext())
{
    var customer = _customerById(db, 147);
}
```

## 变更跟踪

### Attach 可以跟踪由新实体和现有实体构成的对象图

EF Core 可通过各种机制支持主键值的自动生成。该功能会为具有运行时默认类型的主键属性生成值（通常为 0 或 null ）。这意味着一个实体对象图可以传递给 `DbContext.Attach` 或 `DbSet.Attach`，EF Core 会将已经设置了主键值的实体标记为 `Unchanged`，而将没有设置主键值的实体标记为 `Added`。这使得使用自动生成主键值的时候附加一个混合了新实体和现有实体的对象图变得简单。`DbContext.Update` 和 `DbSet.Update` 都是以相同的方式工作的，除非设置的主键值的实体被标记为 `Modified`，而不是 `Unchanged`。

## 查询

### 改进 LINQ 翻译

使更多的查询能够被成功执行，让查询逻辑能够在数据库中（而不是内存中）被评估，从而减少在数据库中检索不必要的数据。

### GroupJoin 改进

这项工作改进了group join （组查询）生成的 SQL。最常见的组查询是可选导航属性上的子查询。

### FromSql 和 ExecuteSqlCommand 中的字符串插值

C# 6 介绍了字符串插值，其中的一个功能是允许直接将 C# 表达式嵌入到字符串文本中，如此以在运行时提供良好的字符串构造过程。在 EF Core 2.0 中我们为两个接受合法 SQL 字符串的主要 API 添加了针对插值字符串的特殊支持：`FromSql` 和 `ExecuteSqlCommand`。这种特殊支持允许在“安全”模式下使用 C# 字符串插值，即以这种方式避免常见的SQL注入错误（在运行时动态构造 SQL 语句比较容易出现 SQL 注入错误）。

以下是示例代码：

```C#
var city = "London";
var contactTitle = "Sales Representative";
using(var context = CreateContext())
{
    context.Set<Customer>()
        .FromSql($@"
            SELECT * 
            FROM ""Customers""
            WHERE ""City"" = {city} AND
                ""ContactTitle"" = {contactTitle}")
            .ToArray();
}
```

该示例中有两个变量嵌入在 SQL 格式字符串中。EF Core 将会产生以下 SQL：

```SQL
@p0='London' (Size = 4000)
@p1='Sales Representative' (Size = 4000)

SELECT * 
FROM ""Customers""
FROM """City" = @p0
    AND ""ContactTitle"" = @p1
```

### EF.Functions.Like()

我们添加了 EF.Functions 属性，它可以被 EF Core 或相关的提供程序用来定义与数据库函数或操作对应的方法，这样就可以在 LINQ 查询中调用他们。这种方法的首个示例就是 Like()：

```C#
var aCustomers = 
    from c in context.Customers
    where EF.Functions.Like(c.Name, "a%");
    select c;
```

请注意 Like() 具有内存实现，这在操作内存数据库或者在客户端评估谓词时会很方便。

## 数据库管理

### DbContext 基架的多元化钩子

EF Core 2.0 介绍了一个新的 _IPluralizer_ 服务，它被用来单一化实体类型名称和多元化 DbSet 名称。其默认实现是一个空操作，所以这只是一个钩子，开发者可以很轻松地插入他们的多元化实现。

以下代码展示了开发者可以如何勾住他们的多元化器：

```C#
public class MyDesingTimeService : IDesignTimeServices
{
    public void ConfigureDesignTimeService(IServiceCollection services)
    {
        services.AddSingleton<IPluralizer,MyPluralizer>();
    }
}

public class MyPluralizer:IPluralizer
{
    public string Pluralize(string name)
    {
        return Inflector.Inflector.Pluralize(name) ?? name;
    }

    public string Singularize(string name)
    {
        return Inflector.Inflector.Singularize(name) ?? name;
    }
}
```

## 其他

### 将 ADO.NET SQLite 提供程序迁移到 SQLLitePCL.raw

这为我们在 Microsoft.Data.Sqlte 下降 SQLite 二进制发布到不同平台提供了更多健壮的解决方案。

### 每个模型只有一个提供程序

显著地增强了提供程序与模型的交互方式，并且简化了针对不同提供程序的惯例、注释和流式API工作方式。

EF Core 2.0 正在为每个不同的提供程序构建一个不同的 [IModel](https://github.com/aspnet/EntityFrameworkCore/blob/dev/src/EFCore/Metadata/IModel.cs)。通常这对于应用程序是透明的。这促进了底层元数据 API 的简化，使得对 _公共关系元数据概念_ 的任何访问总是通过调用 `.Relational` 来实现，而不是 `.SqlServer`，`.Sqlite` 等等。

### 统一的日志记录和诊断

现在，（基于 ILogger 的）日志记录和（基于 DiagnosticSource 的）诊断机制可以共享更多代码。

发送给某个 ILogger 的消息事件 ID 在 EF Core 2.0 中发生了改变。现在的事件 ID 对于整个 EF Core 代码来说是唯一的了。还有，这些消息现在遵循了 MVC 中使用的结构化日志的标准模式。

日志记录分类也发生了改变。现在公共已知的一组分类可以通过 [DbLoggerCategory](https://github.com/aspnet/EntityFramework/blob/dev/src/EFCore/DbLoggerCategory.cs) 来访问。

诊断事件现在使用与 `ILogger` 消息一致的事件 ID 名称。