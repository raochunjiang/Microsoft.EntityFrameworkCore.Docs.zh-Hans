# 保存关系数据

除了独立的实体之外，你还可以保存模型中定义的关系。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Saving/Saving/RelatedData/)。

## 添加新的实体对象图

如果你创建了一些新的关联实体，将其中之一添加到上下文实例时其他关联实体也将被添加。

在以下代码样例中，blog 和三个关联的 post 都会被插入到数据库中。post 集合会被发现并添加，因为它们可以通过 `Blog.Posts` 导航属性被获取。

```C#
using (var context = new BloggingContext())
{
    var blog = new Blog
    {
        Url = "http://blogs.msdn.com/dotnet",
        Posts = new List<Post>
        {
            new Post { Title = "Intro to C#" },
            new Post { Title = "Intro to VB.NET" },
            new Post { Title = "Intro to F#" }
        }
    };

    context.Blogs.Add(blog);
    context.SaveChanges();
}
```

## 添加关联实体

一旦上下文实例跟踪实体的导航属性引用了新的实体，该实体也将被发现并插入到数据库中。

在以下代码样例中，`post` 实体会被插入，因为它被添加到了从数据库提取的 `blog` 实体的 `Posts` 属性.

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs.Include(b => b.Posts).First();
    var post = new Post { Title = "Intro to EF Core" };

    blog.Posts.Add(post);
    context.SaveChanges();
}
```

## 更改关联关系

如果实体实例的导航属性发生变更，数据库中的外键列也将发生相应的变更。

在以下代码样例中，`post` 实体会被更新为隶属于一个新的 `blog` 实体，因为它的 `Blog` 导航属性被设置为指向 `blog`。注意一下，`blog` 也将会被插入到数据库中，因为它是一个新的由已有实体导航属性引用的实体，而已有实体（`post`）已由上下文实例跟踪。

```C#
using (var context = new BloggingContext())
{
    var blog = new Blog { Url = "http://blogs.msdn.com/visualstudio" };
    var post = context.Posts.First();

    post.Blog = blog;
    context.SaveChanges();
}
```

## 移除关联关系

可以设置导航属性为 `null` ，或者直接从集合导航属性中移除关联实体，如此来移除关联关系。

根据关联关系中的级联删除行为配置，移除关联关系可能会对依赖实体造成负面影响。

默认情况下，对于必须的关联关系会配置为级联删除，子实体/依赖实体会同时从数据库中被删除。对于可选关系，默认不会配置为级联删除，此时外键属性会被设置为 `null`。

查看 [必须的和可选的关系](../3、创建模型/J、关系.md#必须的和可选的关系) 以学习如何配置必须的关联关系。

查看 [级联删除](./D、级联删除.md) 以详细了解级联删除的原理，了解如何显式配置它们以及它们是如何按照惯例被选取的。

在以下代码样例中，`Blog` 和 `Post` 关联关系被配置为级联删除，所以 `post` 实体会从数据库中被删除。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs.Include(b => b.Posts).First();
    var post = blog.Posts.First();

    blog.Posts.Remove(post);
    context.SaveChanges();
}
```