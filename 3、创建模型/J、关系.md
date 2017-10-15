# 关系

关系定义了两个实体之间的关联方式。在关系数据库中，关系表现为外键约束。

> 注意
>
> 本文中的大部分样例使用了一对多的关系来展示相关概念。查看文末的 [其他关系模式](#其他关系模式) 可以了解一对一和多对多关系。

## 术语定义

描述关系需要用到大量术语

* **依赖实体**：包含外键属性的实体。有时被称作关系中的“子”。
* **主实体**：包含主键/替代键（备用关键字）属性的实体。有时被称作关系中的“父”。
* **外键**：依赖实体中用于存储关联实体的主键属性值的属性。
* **主键**：唯一标识主实体的属性。可以是主键或替代键（备用关键字）。
* **导航属性**：在主实体或依赖实体上定义的包含关联实体的引用的属性。
    _ **集合导航属性**：包含多个关联实体的引用的导航属性。
    _ **引用导航属性**：维持单一关联实体的引用的导航属性。
    _ **逆向导航属性**：当讨论一个特定的导航属性时，该术语指的是关系另一端的导航属性。

以下列出的代码显示了 `Blog` 和 `Post`之间的一对多关系。

* `Post` 是依赖实体
* `Post` 是主实体
* `Post.BlogId` 是外键
* `Blog.BlogId` 是主键（这种情况下它是主键，不是替代键）
* `Post.Bolg` 是引用导航属性
* `Blog.Posts` 是集合导航属性
* `Post.Blog` 是 `Blog.Posts` 的逆向导航属性（反之亦然）

```C#
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

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

## 惯例

按照惯例，当在一个类型上发现了导航属性时就会创建一个关系。如果一个属性指向的类型不能被当前数据库提供程序映射为标量类型，EF 就认为它是导航属性。

> 注意
>
> 按照惯例发现的关系总是指向主实体的主键的。要指向替代键的话，就要使用[流式 API](#流式 API) 做更多的配置。

### 完整定义的关系

关系最普遍的模式就是在关系的两端都定义导航属性，并且在依赖实体类型中定义一个外键属性。

* 如果在两个类型中找到一对导航属性，则他们将被配置为同一关系的逆向导航属性。

* 如果依赖实体包含一个名为 `<主键属性名>`、`<导航属性名><主键属性名>` 或者 `<主实体类名><主键属性名>` 的属性，则它被配置为外键。

```C#
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

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}
```

> 警告
>
> 如果两个类型之间定义了多个导航属性（也就是说不止一对指向对方的独立导航），则按照惯例不会创建任何一对关系，你需要手动配置他们以标识导航属性之间如何配对。

### 无外键属性

尽管建议要在依赖实体中定义外键属性，但是它不是必须的。如果没有找到外键属性，EF 就会引入名为 `<导航属性名><主键属性名>` 的影子属性（详见[影子属性](./I、影子属性.md)）。

```C#
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
```

### 单一导航属性

仅包含一个导航属性（没有逆向导航，也没有外键属性）就足够按照惯例定义关系了。所以你也可以定义单一的导航属性和一个外键属性。

```C#
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
}
```

### 级联删除

按照惯例，对于必须的关系，级联删除会被设置为 _Cascade_，而对于可选的关系则设置为 _ClientSetNull_。_Cascade_ 意味着依赖实体也将被删除；_ClientSetNull_ 意味着未加载到内存中的依赖实体将保持不变，必须手动删除它或更新为指向其他合法的实体。对于已加载到内存中的实体，EF Core 将尝试设置他们的外键属性为 null。

查看 [必须的和可选的关系](#必须的和可选的关系) 可详细了解必须的关系和可选的关系。

查看 [级联删除]()

## 数据注解

## 流式 API

## 其他关系模式