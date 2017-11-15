# 使用 InMemory 测试

当你想要使用接近真实数据库的东西来测试组件，同时又不想造成实际数据库输入输出的操作开销时，内存（InMemory）提供程序会很有用。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Miscellaneous/Testing)。

## InMemory 不是关系数据库

EF Core 数据库提供程序并非就是关系数据库。InMemory 是设计用于测试的通用数据库，而不是模拟关系数据库。

与此相关的样例包括：

* InMemory将允许您保存在关系数据库中违反引用完整性约束的数据
* 如果在模型中对属性使用了 DefaultValueSql(string)，由于它是关系数据库的 API，所以针对 InMemory 运行时它会没有效果。

> 提示
>
> 对于大部分测试来说这样的差异并不重要。但是，如果你想要使用接近真实数据库的东西来测试一些东西，那么建议使用 [SQLite 的内存模式](./B、使用SQLite测试.md)。

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

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;");
    }
}
```

> 提示
>
> 如果你正在使用 ASP.NET Core，那么你就不需要这些代码，因为数据库提供程序是在 context 之外被配置的（在 Startup.cs 中）。

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

使用该提供程序进行测试的关键点是告知上下文要使用 InMemory 提供程序的能力，以及控制内存数据库范围的能力。通常你会想要为每个测试方法都清理数据库。

以下是一个使用 InMemory 数据库的测试类型样例。每个测试方法都指定了唯一的数据库名称，即意味着每个方法都具有其对应的内存数据库。

> 提示
>
> 需要添加 `Microsoft.EntityFrameworkCore.InMemory` 的 NuGet 程序包引用，才能使用 `.UseInMemoryDatabase()` 扩展方法。

```C#
using BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestProject.InMemory
{
    [TestClass]
    public class BlogServiceTests
    {
        [TestMethod]
        public void Add_writes_to_database()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // 针对一个 context 实例运行测试
            using (var context = new BloggingContext(options))
            {
                var service = new BlogService(context);
                service.Add("http://sample.com");
            }

            // 使用独立的 context 实例验证是否已将正确的数据保存到了数据库
            using (var context = new BloggingContext(options))
            {
                Assert.AreEqual(1, context.Blogs.Count());
                Assert.AreEqual("http://sample.com", context.Blogs.Single().Url);
            }
        }

        [TestMethod]
        public void Find_searches_url()
        {
            var options = new DbContextOptionsBuilder<BloggingContext>()
                .UseInMemoryDatabase(databaseName: "Find_searches_url")
                .Options;

            // 使用一个 context 实例将种子数据插入到数据库中
            using (var context = new BloggingContext(options))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            // 用于清理运行测试的 context 实例
            using (var context = new BloggingContext(options))
            {
                var service = new BlogService(context);
                var result = service.Find("cat");
                Assert.AreEqual(2, result.Count());
            }
        }
    }
}
```