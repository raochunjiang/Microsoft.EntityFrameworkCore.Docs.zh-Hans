# 包含和排除类型

将类型包含到模型中意味着 EF 将获得该类型的元数据，并且将尝试从数据库读取该类型的实例或将该类型的实例写入到数据库。

## 惯例

按照惯例，在上下文中通过 `DbSet` 属性暴露的类型都将包含在模型中。另外，在 `OnModelCreating` 方法中提及的类型也会被包含在模型中。最后，通过递归扫描已包含（在模型中的）类型的导航属性所找到的任何类型都将包含在模型中。

__比如，以下代码列出的三个类型都已包含在模型中：__

* `Blog` 由于上下文中的 `DbSet` 属性暴露而被包含在模型中
* `Post` 由于通过 `Blog.Posts` 导航属性发现而被包含在模型中
* `AuditEntry` 由于在 `OnModelCreating` 中提及而被包含在模型中

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditEntry>();
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

public class AuditEntry
{
    public int AuditEntryId { get; set; }
    public string Username { get; set; }
    public string Action { get; set; }
}
```

## 数据注解

可以使用数据注解将某个类型从模型中排除。

```C#
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public BlogMetadata Metadata { get; set; }
}

[NotMapped]
public class BlogMetadata
{
    public DateTime LoadedFromDatabase { get; set; }
}
```

## 流式 API

可以使用流式 API 将某个类型从模型中排除

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<BlogMetadata>();
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public BlogMetadata Metadata { get; set; }
}

public class BlogMetadata
{
    public DateTime LoadedFromDatabase { get; set; }
}
```