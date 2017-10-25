# 处理并发

如果一个属性被配置为并发标记，那么在保存属性记录的期间 EF 将会制止其他用户修改该属性在数据库中的值。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Saving/Saving/Concurrency/)。

## EF Core 是如何处理并发的

关于 Entity Framework Core 是如何处理并发的详细描述请查阅 [并发标记](../3、创建模型/H、并发标记.md)。

## 解决并发冲突

解决并发冲突需要使用算法来将当前用户的挂起更改与数据库变更合并。具体的方法根据不同的应用程序有所不同，通用的办法是向用户显示这些值，让他们决定要存储到数据库中的值。

**有三组值集合可以用来协助解决并发冲突** 。

* **当前值（CurrentValue）** 是应用程序正在尝试写入数据库的值。
* **原始值（OriginalValue）** 是从数据库遍历出来的、没有作任何编辑的值。
* **数据库值（DatabaseValue）** 是当前存储在数据库中的值。

为了解决并发冲突，需要在 `SaveChanges()` 期间捕捉 `DbUpdateConcurrencyException` 异常，然后使用 `DbUpdateConcurrencyException.Entries` 来准备受影响的实体的新变更集，然后重新尝试 `SaveChanges()` 操作。

在以下代码样例中，`Person.FirstName` 和 `Person.LastName` 被设置成了变更标记。其中有一个 `// TODO:` 注释，在这里你可以包含应用程序指定的逻辑，选择合适的值来存储到数据。

```C#
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EFSaving.Concurrency
{
    public class Sample
    {
        public static void Run()
        {
            // 确保已经创建了数据库，并且包含一个 person 数据。
            using (var context = new PersonContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.People.Add(new Person { FirstName = "John", LastName = "Doe" });
                context.SaveChanges();
            }

            using (var context = new PersonContext())
            {
                // 从数据库提取 person 数据并更改其电话号码 PhoneNumber
                var person = context.People.Single(p => p.PersonId == 1);
                person.PhoneNumber = "555-555-5555";

                // 更改更改数据库中 person 的名称 name（这会导致并发冲突）
                context.Database.ExecuteSqlCommand("UPDATE dbo.People SET FirstName = 'Jane' WHERE PersonId = 1");

                try
                {
                    // 尝试保存变更到数据库
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Person)
                        {
                            // 使用无跟踪查询意味着我们会获得实体，但它不受上下文跟踪
                            // 并且不会与上下文中的已有实体合并
                            var databaseEntity = context.People.AsNoTracking().Single(p => p.PersonId == ((Person)entry.Entity).PersonId);
                            var databaseEntry = context.Entry(databaseEntity);

                            foreach (var property in entry.Metadata.GetProperties())
                            {
                                var proposedValue = entry.Property(property.Name).CurrentValue;
                                var originalValue = entry.Property(property.Name).OriginalValue;
                                var databaseValue = databaseEntry.Property(property.Name).CurrentValue;

                                // TODO: 这里编写决定选取哪个值被写到数据库的逻辑
                                // entry.Property(property.Name).CurrentValue = <value to be saved>;

                                // 原始值更新为...
                                entry.Property(property.Name).OriginalValue = databaseEntry.Property(property.Name).CurrentValue;
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("无法处理并发冲突：" + entry.Metadata.Name);
                        }
                    }

                    // 重新尝试保存操作
                    context.SaveChanges();
                }
            }
        }

        public class PersonContext : DbContext
        {
            public DbSet<Person> People { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFSaving.Concurrency;Trusted_Connection=True;");
            }
        }

        public class Person
        {
            public int PersonId { get; set; }

            [ConcurrencyCheck]
            public string FirstName { get; set; }

            [ConcurrencyCheck]
            public string LastName { get; set; }

            public string PhoneNumber { get; set; }
        }

    }
}
```