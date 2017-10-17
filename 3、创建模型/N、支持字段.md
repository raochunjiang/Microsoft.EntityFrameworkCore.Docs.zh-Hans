# 支持字段

支持字段允许 EF 直接读写字段（而不是属性）。

## 惯例

按照惯例，以下字段将被发现为给定属性的支持字段（按列出顺序优先）。只有包含在模型中的属性才具有支持字段。关于属性如何包含在模型中，请查看 [包含和排除属性](./C、包含和排除属性.md)。

* `_<驼峰属性名>`
* `_<属性名>`
* `m_<驼峰属性名>`
* `m_<属性名>`

```C#
public class Blog
{
    private string _url;

    public int BlogId { get; set; }

    public string Url
    {
        get { return _url; }
        set { _url = value; }
    }
}
```

配置了支持字段后，EF 将会在从数据库填充实体实例时直接写入字段（而不是 setter 属性访问器），之后 EF 将会尽量使用属性来读写该值。例如，如果 EF 要为属性更新该值，它将使用已定义的 setter 属性访问器。当然，如果属性是只读的，EF 则会直接将值写入支持字段。

## 数据注解

不能使用数据注解来配置支持字段。

## 流式 API

可以使用流式 API 来为属性配置支持字段。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(b => b.Url)
            .HasField("_validatedUrl");
    }
}

public class Blog
{
    private string _validatedUrl;

    public int BlogId { get; set; }

    public string Url
    {
        get { return _validatedUrl; }
    }

    public void SetUrl(string url)
    {
        using (var client = new HttpClient())
        {
            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
        }

        _validatedUrl = url;
    }
}
```

### 支持字段使用控制

可以配置 EF 对支持字段或属性的使用模式。查看 [PropertyAccessMode 枚举](https://docs.microsoft.com/zh-cn/ef/core/api/microsoft.entityframeworkcore.metadata.propertyaccessmode) 以了解支持的选项。

```C#
modelBuilder.Entity<Blog>()
    .Property(b => b.Url)
    .HasField("_validatedUrl")
    .UsePropertyAccessMode(PropertyAccessMode.Field);
```

### 无属性字段

还可以在模型中创建一个在实体类型中没有对应属性的属性，它通过字段在实体中存储数据。这与 [影子属性](./I、影子属性.md) 不同，影子属性的数据存储在变更跟踪器里面。这通常被用于实体类型通过方法来获取/设置值的情况。

可以使用 `Property(...)` API 告诉 EF 字段的名称。如果不存在给定名称的属性，则 EF 就会查找该名称的字段。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property("_validatedUrl");
    }
}

public class Blog
{
    private string _validatedUrl;

    public int BlogId { get; set; }

    public string GetUrl()
    {
        return _validatedUrl; 
    }

    public void SetUrl(string url)
    {
        using (var client = new HttpClient())
        {
            var response = client.GetAsync(url).Result;
            response.EnsureSuccessStatusCode();
        }

        _validatedUrl = url;
    }
}
```

还可以选择给定属性名，而不是字段名。该名称将在创建模型时用到，尤其是用作映射到数据库中的数据列名称的时候。

```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .Property<string>("Url")
        .HasField("_validatedUrl");
}
```

当实体类型中没有对应的属性时，你可以在 LINQ 中使用 `EF.Property(...)` 方法来引用该属性，因为在概念上它是模型的一部分。

```C#
var blogs = db.blogs.OrderBy(b => EF.Property<string>(b, "Url"));
```
