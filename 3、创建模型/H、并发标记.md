# 并发标记（Concurrency Tokens）

如果属性被配置为并发标记，则 EF 会在将记录变更保存到数据库时制止其他用户修改该属性的数据库值。EF 使用的是乐观并发模式，就是说它会假设值没有发生变更，并尝试将它保存到数据库，但是如果它发现值已经发生变更，则抛出异常。

比如我们可能想要将 `Person` 实体上的 `LastName` 配置为并发标记。这意味着如果一个用户尝试将变更保存到 `Person`，但另一个用户已经更改了 `LastName`，那么 EF 将抛出异常。 这可能就是你想要的，因为应用程序能够提示用户在保存他们的变更之前该记录仍然代表着同一个实际 Person。

## EF 中并发标记是如何工作的

对于被更新或删除的、指派了并发标记的属性，数据存储能够通过检查其值与上下文初次从数据库加载的值是否仍然相同来强制执行并发标记。

例如，关系数据库是通过在任何 `UPDATE` 或 `DELETE` 命令的  `WHERE` 子句中包含并发标记，然后检查受影响的行数来实现的。如果并发标记仍然匹配，则更新一个数据行。如果数据库中的值已经发生变更，则不更新任何数据行。

```SQL
UPDATE [Person] SET [FirstName] = @p1
WHERE [PersonId] = @p0 AND [LastName] = @p2;
```

## 惯例

按照惯例，属性不会被配置为并发标记。

## 数据标注

可以使用数据标注来将一个属性配置为并发标记。

```C#
public class Person
{
    public int PersonId { get; set; }
    [ConcurrencyCheck]
    public string LastName { get; set; }
    public string FIrstName { get; set; }
}
```

## 流式 API

可以使用流式 API 来将一个属性配置为并发标记。

```C#
class MyContext : DbContext
{
    public DbSet<Person> People { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .Property(p => p.LastName)
            .IsConcurrencyToken();
    }
}

public class Person
{
    public int PersonId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}
```

## 时间戳/行版本

时间戳是一个属性，该属性在相应的数据行被插入或更新到数据库时都由数据库重新生成。这样的属性也将被作为并发标记来对待。这样的话，如果从你查询数据到你尝试更新数据行期间有其他人修改了改行数据，就能确保你会获得一个异常。

时间戳/行版本的实现取决于所使用的数据库提供程序。对于 SQL Server，时间戳通常被用在将在数据库中被设置为 `ROWVERSION` 列的 `byte[]` 属性上。

### 惯例

按照惯例，属性不会被配置为时间戳。

### 数据标注

可以使用数据标注来将属性配置为时间戳。

```C#
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    [Timestamp]
    public byte[] Timestamp { get; set; }
}
```

### 流式 API

可以使用流式 API 来将属性配置为时间戳。

```C#
class MyContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .Property(p => p.Timestamp)
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken();
    }
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public byte[] Timestamp { get; set; }
}
```
