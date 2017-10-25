# 显式设置生成值属性的值

生成值属性是在添加或更新实体时（由 EF 或者 数据库）为其生成属性值的属性。详见 [生成值](../3、创建模型/E、生成值.md) 。

在某些环境下你可能想要显式设置生成值属性的值，而不是使用生成的值。

> 提示
>
> 你可以[在 GitHub 上查阅当前文章涉及的代码样例](https://github.com/aspnet/EntityFramework.Docs/tree/master/samples/core/Saving/Saving/ExplicitValuesGenerateProperties/)。

## 模型

本文使用的模型只包含一个 `Employee` 实体。‘

```C#
public class Employee
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public DateTime EmploymentStarted { get; set; }
    public int Salary { get; set; }
    public DateTime? LastPayRaise { get; set; }
}
```

## 在添加时保存显式值

新实体的 `Employee.EmployentStarted` 属性被配置为由数据库生成值（使用默认值）。

```C#
modelBuilder.Entity<Employee>()
    .Property(b => b.EmploymentStarted)
    .HasDefaultValueSql("CONVERT(date, GETDATE())");
```

以下代码会将两条 employee 数据插入到数据库中。

* 对于第一条数据，不会为 `Employee.EmploymentStarted` 属性赋值，所以其依然会被设置为 `DateTime` 类型的运行时默认值。
* 对于第二条数据，我们显式将其值设置为 `1-Jan-2000`。

```C#
using (var context = new EmployeeContext())
{
    context.Employees.Add(new Employee { Name = "John Doe" });
    context.Employees.Add(new Employee { Name = "Jane Doe", EmploymentStarted = new DateTime(2000, 1, 1) });
    context.SaveChanges();

    foreach (var employee in context.Employees)
    {
        Console.WriteLine(employee.EmployeeId + ": " + employee.Name + ", " + employee.EmploymentStarted);
    }
}
```

输出显式为数据库为第一个 employee 生成的属性值和我们为第二个 employee 显式设置的值。

```console
1: John Doe, 1/26/2017 12:00:00 AM
2: Jane Doe, 1/1/2000 12:00:00 AM
```

### 显式指定 SQL Server 标识列（IDENTITY column）的值

按照惯例，`Employee.EmployeeId` 属性是数据库生成的 `IDENTITY` 列。

大多情况下，上述方法对键属性有效。然而，想要为 SQL Server `IDENTITY` 列显式指定值的话，你需要在调用 `SaveChanges()` 前手动启用 `IDENTITY_INSERT`。

> 注意
>
> 在我们的待办事项列表里有一个在 SQL Server 提供程序中自动完成该功能的 [功能请求](https://github.com/aspnet/EntityFramework/issues/703)。

```C#
using (var context = new EmployeeContext())
{
    context.Employees.Add(new Employee { EmployeeId = 100, Name = "John Doe" });
    context.Employees.Add(new Employee { EmployeeId = 101, Name = "Jane Doe" });

    context.Database.OpenConnection();
    try
    {
        context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Employees ON");
        context.SaveChanges();
        context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Employees OFF");
    }
    finally
    {
        context.Database.CloseConnection();
    }


    foreach (var employee in context.Employees)
    {
        Console.WriteLine(employee.EmployeeId + ": " + employee.Name);
    }
}
```

输出显式为已提供并保存到数据库的 id 值。

```console
100: John Doe
101: Jane Doe
```

## 在更新时设置显式值

`Employee.LastPayRaise` 属性被配置为在更新时由数据库生成值。

```C#
modelBuilder.Entity<Employee>()
    .Property(b => b.LastPayRaise)
    .ValueGeneratedOnAddOrUpdate();

modelBuilder.Entity<Employee>()
    .Property(b => b.LastPayRaise)
    .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
```

> 注意
>
> 默认情况下，如果尝试为已设置为更新时生成值的属性设置显式值，EF Core 会抛出异常。为了避免该问题，你需要钻入到底层元数据 API 并设置 `AfterSaveBehavior`（如上所示）。

> 注意
>
> **EF Core 2.0 中的变更：** 在之前的版本中，after-save 行为是通过 `IsReadOnlyAfterSave` 标签来控制的。该标签已被启用，取而代之应该使用 `AfterSaveBehavior`。

还需要一个数据库触发器来在 `UPDATE` 操作期间为 `LastPayRaise` 列生成值。

```SQL
CREATE TRIGGER [dbo].[Employees_UPDATE] ON [dbo].[Employees]
    AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF ((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN;

    IF UPDATE(Salary) AND NOT Update(LastPayRaise)
    BEGIN
        DECLARE @Id INT
        DECLARE @OldSalary INT
        DECLARE @NewSalary INT

        SELECT @Id = INSERTED.EmployeeId, @NewSalary = Salary
        FROM INSERTED

        SELECT @OldSalary = Salary        
        FROM deleted

        IF @NewSalary > @OldSalary
        BEGIN
            UPDATE dbo.Employees
            SET LastPayRaise = CONVERT(date, GETDATE())
            WHERE EmployeeId = @Id
        END
    END
END
```

以下代码会在数据库中对两个 employee 的 salary 进行递增。

* 对于第一个 employee，没有为 `Employee.LastPayRaise` 赋值，所以其依然会被设为 null。
* 对于第二个 employee，我们显式将其属性值设置为一周前。

```C#
using (var context = new EmployeeContext())
{
    var john = context.Employees.Single(e => e.Name == "John Doe");
    john.Salary = 200;

    var jane = context.Employees.Single(e => e.Name == "Jane Doe");
    jane.Salary = 200;
    jane.LastPayRaise = DateTime.Today.AddDays(-7);

    context.SaveChanges();

    foreach (var employee in context.Employees)
    {
        Console.WriteLine(employee.EmployeeId + ": " + employee.Name + ", " + employee.LastPayRaise);
    }
}
```

输出显式为数据库为第一个 employee 生成的属性值和我们为第二个 employee 显式设置的值。

```console
1: John Doe, 1/26/2017 12:00:00 AM
2: Jane Doe, 1/19/2017 12:00:00 AM
```