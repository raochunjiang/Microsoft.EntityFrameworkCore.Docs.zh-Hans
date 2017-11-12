# SQLite EF Core 数据库提供程序局限性

SQLite 提供程序有一些迁移限制，其中大部分是底层 SQLite 数据库引擎的限制造成的，并不是 EF 特定的。

## 建模限制

通用的（由 Entity Framework 关系数据库提供程序共享的）关系库提供了建模相关的 API，这对于大部分关系数据库引擎是通用的。其中有几个概念是 SQLite 提供程序不支持的。

* 模式（Schemas）
* 序列（Sequences）

## 迁移的局限性

SQLite 数据库引擎不支持一些其他主要的关系数据库支持的模式操作。如果尝试将不被支持的操作应用到 SQLite 数据库，将会抛出 `NotSupportedException` 异常。

|操作|支持状态|
|:---:|:---:|
|AddColumn（添加数据列）|✔|
|AddForeignKey（添加外键）|✗|
|AddPrimaryKey（添加主键）|✗|
|AddUniqueConstraint（添加唯一约束）|✗|
|AlterColumn（修改数据列）|✗|
|CreateIndex（创建索引）|✔|
|CreateTable（创建数据表）|✔|
|DropColumn（删除数据列）|✗|
|DropForeignKey（删除外键）|✗|
|DropIndex（删除索引）|✔|
|DropPrimaryKey（删除主键）|✗|
|DropTable（删除数据表）|✔|
|DropUniqueConstraint（删除唯一约束）|✗|
|RenameColumn（重命名数据列）|✗|
|RenameIndex（重命名索引）|✗|
|RenameTable（重命名表名）|✔|

## 迁移局限性的解决方案

可以在迁移中手动编写代码来执行数据表重建，这样可以解决一些局限性问题。一个数据表的重建包括重命名已有的数据表、创建新表、复制数据到新表以及删除旧的数据表。这将需要使用 `Sql(string)` 方法来执行其中一些步骤。

查看 SQLite 文档中的 [其他类型的表模式更改](http://sqlite.org/lang_altertable.html#otheralter) 可以了解更详细的信息。

未来 EF 可能会通过其中的数据表重建的方式来支持上述的一些操作。你可以 [在我们的 GitHub 项目上跟踪这个功能](https://github.com/aspnet/EntityFramework/issues/329)。