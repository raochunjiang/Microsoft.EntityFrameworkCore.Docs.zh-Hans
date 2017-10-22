# 原生SQL查询

Entity Framework Core 允许你在使用关系数据库时下降为原生SQL查询。当你想要执行一些 LINQ 无法表达的查询，或者使用 LINQ 会导致发送到数据库的 SQL 效率低下时，这会很有用。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Querying)。

## 局限性

使用原生 SQL 查询有一些限制。

* SQL 查询只能用于返回模型中的实体类型实例。在我们的待办事项列表中有一个 [允许从 SQL 查询返回特定类型实例](https://github.com/aspnet/EntityFramework/issues/1862) 的优化工作。
* SQL 查询必须为实体类型的所有属性返回数据。
* 结果集中的列名必须与每个属性所映射到的列名相匹配。注意，这与 EF6 不同。在 EF6 中使用原生 SQL 查询时，属性/列映射会被忽略，结果集的列名必须与每个属性的名称匹配。
* SQL 查询无法包含关联数据。然而，大部分情况下你可以在查询的顶端组合使用 `Include` 操作来返回关联数据（详见 [包含关联数据](#包含关联数据)）。

## 原生 SQL 基础查询

可以使用 `FromSql` 扩展方法来开始基于原生 SQL 查询的 LINQ 查询。

```C#
var blogs = context.Blogs
    .FromSql("SELECT * FROM dbo.Blogs")
    .ToList();
```

原生 SQl 查询可以用于执行存储过程。

```C#
var blogs = context.Blogs
    .FromSql("EXECUTE dbo.GetMostPopularBlogs")
    .ToList();
```

## 传递参数

与任何接受 SQL 的 API 一样，参数化任何用户输入以防止 SQL 注入攻击是很重要的。你可以在 SQL 查询字符串中包含参数占位符，然后将参数值作为附加参数来提供。你提供的任何参数都会被自动转换为 `DbParameter`。

以下样例传递了一个参数给存储过程。这看上去与 `String.Format` 语法相似。提供的值会被包装为参数，而所生成的参数名称会被插到 `{0}` 占位符指定的位置上。

```C#
var user = "johndoe";

var blogs = context.Blogs
    .FromSql("EXECUTE dbo.GetMostPopularBlogsForUser {0}", user)
    .ToList();
```

以下查询与上面的查询是等效的，只是这里使用了 EF Core 2.0 或以上版本所支持的字符串插值语法。

```C#
var user = "johndoe";

var blogs = context.Blogs
    .FromSql($"EXECUTE dbo.GetMostPopularBlogsForUser {user}")
    .ToList();
```

还可以构造 DbParameter 并将其作为参数值来提供。这允许你在 SQL 查询字符串中使用命名参数。

```C#
var user = new SqlParameter("user", "johndoe");

var blogs = context.Blogs
    .FromSql("EXECUTE dbo.GetMostPopularBlogsForUser @user", user)
    .ToList();
```

## 与 LINQ 组合

如果你的 SQL 查询允许在数据库中被组合，那么你也就可以在初始原生 SQL 查询的顶端使用 LINQ 运算进行组合。可被组合的 SQL 查询都是以 `SELECT` 关键字开头的。

以下样例使用了一个原生 SQL 查询来从表值函数（Table-Valued Function，TVF）查询数据，然后使用 LINQ 来在其上组合执行筛选和排序。

```C#
var searchTerm = ".NET";

var blogs = context.Blogs
    .FromSql($"SELECT * FROM dbo.SearchBlogs({searchTerm})")
    .Where(b => b.Rating > 3)
    .OrderByDescending(b => b.Rating)
    .ToList();
```

### 包含关联数据

与 LINQ 操作组合可用于在查询中包含关联数据。

```C#
var searchTerm = ".NET";

var blogs = context.Blogs
    .FromSql($"SELECT * FROM dbo.SearchBlogs({searchTerm})")
    .Include(b => b.Posts)
    .ToList();
```

> 警告
>
> **在原生 SQL 查询中务必要使用参数化：** 接受 SQL 字符串的 API（比如 `FromSql` 和 `ExecuteSqlCommand`）能够轻松地将值作为参数来传递。除了验证用户输入以外，务必要对原生 SQL 查询/命令中用到的值进行参数化。如果你正在使用字符串串联来动态构建查询字符串的任何片段，那么你就要负责验证任何输入以防止 SQL 注入攻击。