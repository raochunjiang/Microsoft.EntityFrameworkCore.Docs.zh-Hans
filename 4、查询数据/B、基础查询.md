# 基础查询

学习如何使用集成语言查询（Language Integrate Query，LINQ）从数据库中加载实体。

> 提示
> 
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Querying)。

## 101 个 LINQ 样例

该页面展示了一些样例以使用 Entity Framework Core 来完成普通的任务。关于 LINQ 所能做的更多事情，请查阅 [101 个 LINQ 样例](https://code.msdn.microsoft.com/101-LINQ-Samples-3fb9811b/) 

## 加载所有数据

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs.ToList();
}
```

## 加载单一实体

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs
        .Single(b => b.BlogId == 1);
}
```

## 筛选（数据过滤）

```C#
using (var context = new BloggingContext())
{
    var blogs = context.Blogs
        .Where(b => b.Url.Contains("dotnet"))
        .ToList();
}
```