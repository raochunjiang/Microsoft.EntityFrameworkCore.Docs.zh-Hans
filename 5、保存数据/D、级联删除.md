# 级联删除

级联删除通常被用作数据库术语，用来描述删除一个数据行时自动删除关联的数据行的特征。EF Core 实现了一些不同的删除行为，并且允许对关联关系的删除行为进行配置。EF Core 还实现了相关的惯例，它会基于关系的必要性为每个关系配置有用的默认删除行为。

## 删除行为

删除行为是在 `DeleteBehavior` 枚举中定义的，可以将它传递给 `OnDelete` 流式 API 来控制主实体/父实体的删除是否影响其关联的依赖实体/子实体。

有四种可配置的删除行为：

|行为名称|影响（内存跟踪的依赖实体）|影响（数据库存储的依赖实体）|关系必要性（默认配置为该行为）|
|:---:|:---:|:---:|:---:|
|Cascade|实体被删除|实体被删除|必须的（Required）|
|ClientSetNull|外键属性设为 null|无影响|可选的（Optional）|
|SetNull|外键属性设为 null|外键属性设为 null|无|
|Restrict|无影响|无影响|无|

> 重要提示
>
> **EF Core 2.0 中的变更：** 在之前的版本中，_Restrict_ 会造成可选关系中被跟踪依赖实体的外键属性设置为 null，并且这被配置为可选关联关系的默认删除行为。EF Core 2.0 中引入了 _ClientSetNull_ 来表示前述行为并作为可选关联关系的默认删除行为，而 _Restrict_ 被调整为对任何类型的依赖实体无影响。

当对必须的关联关系设置了 _ClientSetNull_ 或 _SetNull_ 时（也就是说由非可空外键属性来控制关联关系），如果主实体/父实体被删除了，而其对应的依赖实体/子实体还在，那么其外键属性不会被设置为 null（因为它是不可空的）。事实上将其设置为 null 会导致 _SaveChanges_ 失败，除非将依赖实体/子实体更改为指向新的主实体/父实体。

> 重要提示
>
> 在 EF Core 模型中配置的删除行为只有在主实体是用 EF Core 删除，且依赖实体已加载到内存中时被应用（也就是说只会应用到被跟踪的依赖实体上）。需要在数据库中设置相应的级联行为，这样才能确保不被上下文跟踪的数据被应用相关的行为。如果你采用 EF Core 来创建数据库，那么它会为你在数据库中设置相应的级联行为。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Saving/Saving/CascadeDelete/)。

## 被跟踪实体的级联行为

当你调用 _SaveChanges_ 的时候，级联删除规则会被应用到所有被上下文跟踪的实体上。

考虑一个简单的 _Blog_ 和 _Post_ 模型，他们之间的关联关系是必须的。按照惯例，这种关系的级联行为会被设置为 _Cascade_ 。

以下代码会从数据库加载一个 Blog 以及其关联的所有 Post（使用 _Include_ 操作）。随后代码会删除 Blog。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs.Include(b => b.Posts).First();
    context.Remove(blog);
    context.SaveChanges();
}
```

因为所有的 Post 都是被上下文跟踪的，所以在保存到数据库之前会对他们应用级联行为。因此 EF 会分别为它们发行一个 _DELETE_ 语句。

```SQL
   DELETE FROM [Post]
   WHERE [PostId] = @p0;
   DELETE FROM [Post]
   WHERE [PostId] = @p1;
   DELETE FROM [Blog]
   WHERE [BlogId] = @p2;
```

## 非跟踪实体的级联行为

以下代码与前述样例几乎是相同的，只是它不会从数据库加载关联的 Post 。

```C#
using (var context = new BloggingContext())
{
    var blog = context.Blogs.First();
    context.Remove(blog);
    context.SaveChanges();
}
```

由于 Post 没有被上下文跟踪，EF Core 只会发行 Blog 的 _DELETE_ 语句。这里依靠数据库中设置的级联行为来确保不被上下文跟踪的关联数据同时被删除。如果你采用 EF Core 来创建数据库，那么它会为你在数据库中设置相应的级联行为。

```SQL
    DELETE FROM [Blog]
    WHERE [BlogId] = @p0;
```