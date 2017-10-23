# 基础保存

学习如何使用上下文实例和实体类型添加、修改和删除数据。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Saving/Saving/Basics/)。

## 添加数据

使用 `DbSet.Add` 方法可以添加实体类型的新实例，当你调用 `SaveChanges` 的时候，数据会被插入到数据库。

```C#
using (var context = new BloggingContext())
{
    var blog = new Blog { Url = "http://sample.com" };
    context.Blogs.Add(blog);
    context.SaveChanges();

    Console.WriteLine(blog.BlogId + ": " +  blog.Url);
}
```

## 更新数据

EF 会自动检测对上下文跟踪的已有实体的变更。这包括你从数据库加载/查询的实体，以及之前已添加和保存到数据库的实体。

对属性进行简单地指派以给它赋值，然后调用 `SaveChanges`。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs.First();
    blog.Url = "http://sample.com/blog";
    context.SaveChanges();
}
```

## 删除数据

使用 `DbSet.Remove` 方法可以删除实体类型实例。

如果实体已存在于数据中，则会在 `SaveChanges` 期间被删除。如果实体还没被保存到数据库（也就是作为新增项被跟踪），那么将从上下文中移除它，调用 `SaveChanges` 时就不会再将其插入到数据库。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs.First();
    context.Blogs.Remove(blog);
    context.SaveChanges();
}
```

## SaveChanges 中的多操作

可以在单次 `SaveChanges` 调用中组合使用 `Add` / `Update` / `Remove` 操作。

> 注意
>
> 对于大部分数据库提供程序，`SaveChanges` 是事务性的。这意味着所有操作不是全部成功就是全部失败，不会部分有效。

```C#
using (var context = new BloggingContext())
{
    // seeding database
    context.Blogs.Add(new Blog { Url = "http://sample.com/blog" });
    context.Blogs.Add(new Blog { Url = "http://sample.com/another_blog" });
    context.SaveChanges();
}

using (var context = new BloggingContext())
{
    // add
    context.Blogs.Add(new Blog { Url = "http://sample.com/blog_one" });
    context.Blogs.Add(new Blog { Url = "http://sample.com/blog_two" });

    // update
    var firstBlog = context.Blogs.First();
    firstBlog.Url = "";

    // remove
    var lastBlog = context.Blogs.Last();
    context.Blogs.Remove(lastBlog);

    context.SaveChanges();
}
```