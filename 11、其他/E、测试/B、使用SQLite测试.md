# 使用 SQLite 测试

SQLite 有一个内存模式，它允许你使用 SQLite 来编写针对关系数据库的测试，并且不会造成实际数据库的操作开销。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Miscellaneous/Testing)。

## 样例测试场景

考虑以下服务，其允许应用程序代码执行一些 blog 相关的操作。其内部使用的是链接到 SQL Server 数据库的 `DbContext`。将上下文切换链接到内存 SQLite 数据库将会很有用，这样的话我们无需修改源代码或者做大量工作来重复为上下文创建测试，就可以对该服务编写高效的测试代码。

```C#
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class BlogService
    {
        private BloggingContext _context;

        public BlogService(BloggingContext context)
        {
            _context = context;
        }

        public void Add(string url)
        {
            var blog = new Blog { Url = url };
            _context.Blogs.Add(blog);
            _context.SaveChanges();
        }

        public IEnumerable<Blog> Find(string term)
        {
            return _context.Blogs
                .Where(b => b.Url.Contains(term))
                .OrderBy(b => b.Url)
                .ToList();
        }
    }
}
```

## 准备上下文

### 避免配置多个提供程序

在测试中你将在外部配置 context 为使用内存提供程序。如果你通过重写 context 的 `OnConfiguring` 来配置数据库提供程序，那么你就要添加一些条件代码才能确保在没有配置提供程序时才配置它。

> 提示
>
> 如果你正在使用 ASP.NET Core，那么你就不需要这些代码，因为数据库提供程序是在 context 之外被配置的（在 Startup.cs 中）。

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;");
    }
}
```

### 为测试添加构造方法

针对不同的数据库，启用测试的最简单方法是修改上下文类型以暴露一个接受 `DbContextOptions<TContext>` 参数的构造方法。

```C#
public class BloggingContext : DbContext
{
    public BloggingContext()
    { }

    public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
    { }
```

> 提示
>
> `DbContextOptions<TContext>` 用于传递上下文配置信息，比如链接到哪个数据库。这与运行 context 的 OnConfiguring 方法所构建的是同一个对象。

## 编写测试

使用该提供程序进行测试的关键点是告知上下文要使用 SQLite，并且控制内存数据库的范围。数据库范围是通过打开和关系链接来控制的。数据库会被限定为链接打开的期间。通常你会想要为每个测试方法都清理数据库。

```C#
using BusinessLogic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestProject.SQLite
{
    [TestClass]
    public class BlogServiceTests
    {
        [TestMethod]
        public void Add_writes_to_database()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new BloggingContext(options))
                {
                    context.Database.EnsureCreated();
                }

                // Run the test against one instance of the context
                using (var context = new BloggingContext(options))
                { 
                    var service = new BlogService(context);
                    service.Add("http://sample.com");
                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new BloggingContext(options))
                {
                    Assert.AreEqual(1, context.Blogs.Count());
                    Assert.AreEqual("http://sample.com", context.Blogs.Single().Url);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void Find_searches_url()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new BloggingContext(options))
                {
                    context.Database.EnsureCreated();
                }

                // Insert seed data into the database using one instance of the context
                using (var context = new BloggingContext(options))
                {
                    context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                // Use a clean instance of the context to run the test
                using (var context = new BloggingContext(options))
                {
                    var service = new BlogService(context);
                    var result = service.Find("cat");
                    Assert.AreEqual(2, result.Count());
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
```