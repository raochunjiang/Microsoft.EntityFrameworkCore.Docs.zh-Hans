# 同一 DbContext 类型中多个模型的交替

在 `OnModelCreating` 中构建的模型可以使用 context 上的一个属性来更改模型的构建方式。例如这可以用来排除一个特定的属性：

```C#
public class DynamicContext : DbContext
{
    public bool? IgnoreIntProperty { get; set; }

    public DbSet<ConfigurableEntity> Entities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseInMemoryDatabase("DynamicContext")
            .ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (IgnoreIntProperty.HasValue)
        {
            if (IgnoreIntProperty.Value)
            {
                modelBuilder.Entity<ConfigurableEntity>().Ignore(e => e.IntProperty);
            }
            else
            {
                modelBuilder.Entity<ConfigurableEntity>().Ignore(e => e.StringProperty);
            }
        }
    }
}
```

## `IModelCacheKeyFactory`

然而，如果你仅尝试上述操作而没有进一步更改，你将在每次创建新的上下文时获得同一个模型，尽管你设置了不同的 `IgnoreIntProperty`。这是由 EF 的模型创建缓存机制造成的，EF 只会调用一次 `OnModelCreating`，然后缓存模型，以此来提高性能。

默认情况下，EF 假设对于给定的上下文类型其模型都是相同的。为了实现这种效果，EF 中 `IModelCacheKeyFactory` 的默认实现返回的是仅包含上下文类型的 Key。要改变该行为，你需要替换 `IModelCacheKeyFactory` 服务。新的实现要返回一个对象，该对象可以通过 Equals 方法来与其他模型 Key 对比，该 Equals 方法应该将所有影响模型的变量考虑在内。

```C#
public class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
{
    public object Create(DbContext context)
    {
        if (context is DynamicContext dynamicContext)
        {
            return (context.GetType(), dynamicContext.IgnoreIntProperty);
        }
        return context.GetType();
    }
}
```