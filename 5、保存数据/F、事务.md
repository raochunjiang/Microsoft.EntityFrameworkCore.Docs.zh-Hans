# 使用事务

事务允许以原子方式处理多个数据库操作。如果事务已提交，那么说明所有操作都已被成功地应用到数据库。如果事务被回滚，那么没有任何一个操作会被应用到了数据库。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Saving/Saving/Transactions/)。

## 默认的事务行为

默认情况下，如果数据库提供程序支持事务，单个 `SaveChanges()` 调用中的所有变更都会在一个事务中被提交。如果其中任何一个变更失败了，那么事务就会回滚，没有任何变更会被应用到数据库。这意味着 `SaveChanges()` 能够确保要么成功保存，要么在发生错误时不对数据库做任何修改。

对于大部分应用程序来说，默认的事务行为已经够用了。只有在应用程序需求认为有必要时你才需要手动去控制事务。

## 事务控制

可以使用 `DbContext.Database` API 来开启、提交 和 回滚事务。以下代码样例显示了两个 `SaveChanges()` 操作和一个在单一事务中执行的 LINQ 查询。

并不是所有的数据库提供程序都支持事务，一些提供程序会在你调用事务 API 时抛出异常或执行空操作（你可以顺带了解一下 [空对象模式](https://baike.baidu.com/item/NULL%20OBJECT/7825232?fr=aladdin)，[Null Object Patterns](https://en.wikipedia.org/wiki/Null_object_pattern)）。

```C#
        using (var context = new BloggingContext())
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/dotnet" });
                    context.SaveChanges();

                    context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/visualstudio" });
                    context.SaveChanges();

                    var blogs = context.Blogs
                        .OrderBy(b => b.Url)
                        .ToList();

                    // 所有命令都成功时提交事务
                    // 如果任何一个命令失败，则在事务被回收（Dispose）时会自动回滚
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // TODO: 处理失败
                }
            }
        }
```

## 跨上下文事务（仅关系数据库适用）

可以在多个上下文实例间共享事务。该功能只在使用关系数据库提供程序时可用，因为它需要使用 `DbTransaction` 和 `DbConnection`，这些都是特定于关系数据库的。

为了实现共享事务，上下文之间必须共享 `DbConnection` 和 `DbTransaction`。

### 允许外部提供链接

共享 `DbConnection` 要求能够在构造上下文实例时传入链接对象。

实现外部提供 `DbConnection` 的最简单方式是避免使用 `DbContext.OnConfiguring` 方法来配置上下文，并且在其外部创建 `DbConetxtOpions`，然后将其传递给上下文类型的构造方法。

> 提示
>
> `DbContextOptionsBuilder` 是你在 `DbContext.OnConfiguring` 中用来配置上下文实例的 API，现在你将在外部使用它来创建 `DbContextOptions`。

```C#
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }
    }
```

另一种替代方案是仍然使用 `DbContext.OnConfiguring`，但是接受并保存一个 `DbConnection`，然后在 `DbContext.OnConfiguring` 中使用它。

```C#
public class BloggingContext : DbContext
{
    private DbConnection _connection;

    public BloggingContext(DbConnection connection)
    {
      _connection = connection;
    }

    public DbSet<Blog> Blogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connection);
    }
}
```

### 共享连接和事务

现在可以创建共享同一个连接的多个上下文了。之后可以使用`DbContext.Database.UseTransaction(DbTransaction))` API 来在一个事务中收集这些上下文。

```C#
        var options = new DbContextOptionsBuilder<BloggingContext>()
            .UseSqlServer(new SqlConnection(connectionString))
            .Options;

        using (var context1 = new BloggingContext(options))
        {
            using (var transaction = context1.Database.BeginTransaction())
            {
                try
                {
                    context1.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/dotnet" });
                    context1.SaveChanges();

                    using (var context2 = new BloggingContext(options))
                    {
                        context2.Database.UseTransaction(transaction.GetDbTransaction());

                        var blogs = context2.Blogs
                            .OrderBy(b => b.Url)
                            .ToList();
                    }

                    // 所有命令都成功时提交事务
                    // 如果任何一个命令失败，则在事务被回收（Dispose）时会自动回滚
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // TODO: 处理失败
                }
            }
        }
```

## 使用外部 DbTransactions（仅关系数据库适用）

如果你正在使用多个数据访问技术来访问关系数据库，那么你可能会想要在这些不同技术执行的操作中共享事务。

以下代码样例显示了如何在同一个事务中执行一个 ADO.NET SqlClient 操作和一个 Entity Framework Core 操作。

```C#
        var connection = new SqlConnection(connectionString);
        connection.Open();

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                // 在事务中运行原生 ADO.NET 命令
                var command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM dbo.Blogs";
                command.ExecuteNonQuery();

                // 在事务中运行 EF Core 命令
                var options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlServer(connection)
                    .Options;

                using (var context = new BloggingContext(options))
                {
                    context.Database.UseTransaction(transaction);
                    context.Blogs.Add(new Blog { Url = "http://blogs.msdn.com/dotnet" });
                    context.SaveChanges();
                }

                // 所有命令都成功时提交事务
                // 如果任何一个命令失败，则在事务被回收（Dispose）时会自动回滚
                transaction.Commit();
            }
            catch (System.Exception)
            {
                // TODO: 处理失败
            }
```
