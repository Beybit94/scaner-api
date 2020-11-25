using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010161349)]
    public class AddDefinitions : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
PRINT N'Выполняется создание [dbo].[hTaskStatus].[Id].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'hTaskStatus', @level2type = N'COLUMN', @level2name = N'Id';


GO
PRINT N'Выполняется создание [dbo].[Scaner_1cDocData].[Article].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Артикуль', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_1cDocData', @level2type = N'COLUMN', @level2name = N'Article';


GO
PRINT N'Выполняется создание [dbo].[Scaner_1cDocData].[Barcode].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Штрихкод', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_1cDocData', @level2type = N'COLUMN', @level2name = N'Barcode';


GO
PRINT N'Выполняется создание [dbo].[Scaner_1cDocData].[Id].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_1cDocData', @level2type = N'COLUMN', @level2name = N'Id';


GO
PRINT N'Выполняется создание [dbo].[Scaner_1cDocData].[PlanNum].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Номер планирования', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_1cDocData', @level2type = N'COLUMN', @level2name = N'PlanNum';


GO
PRINT N'Выполняется создание [dbo].[Scaner_1cDocData].[Quantity].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Количество', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_1cDocData', @level2type = N'COLUMN', @level2name = N'Quantity';


GO
PRINT N'Выполняется создание [dbo].[Scaner_1cDocData].[UserId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор пользователя', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_1cDocData', @level2type = N'COLUMN', @level2name = N'UserId';


GO
PRINT N'Выполняется создание [dbo].[Scaner_File].[BoxId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор короба', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_File', @level2type = N'COLUMN', @level2name = N'BoxId';


GO
PRINT N'Выполняется создание [dbo].[Scaner_File].[Id].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_File', @level2type = N'COLUMN', @level2name = N'Id';


GO
PRINT N'Выполняется создание [dbo].[Scaner_File].[Path].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Путь к файлу', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_File', @level2type = N'COLUMN', @level2name = N'Path';


GO
PRINT N'Выполняется создание [dbo].[Scaner_File].[TaskId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор задачи', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_File', @level2type = N'COLUMN', @level2name = N'TaskId';


GO
PRINT N'Выполняется создание [dbo].[Scaner_File].[TypeId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Тип файла', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_File', @level2type = N'COLUMN', @level2name = N'TypeId';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[BarCode].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Штрихкод', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'BarCode';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[BoxId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор короба', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'BoxId';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[CountQty].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Количество', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'CountQty';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[DamagePercentId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Процент повреждение', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'DamagePercentId';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[GoodArticle].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Артикуль товара', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'GoodArticle';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[GoodId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор товара', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'GoodId';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[GoodName].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Название товара', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'GoodName';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[Id].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'Id';


GO
PRINT N'Выполняется создание [dbo].[Scaner_Goods].[TaskId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор задачи', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Scaner_Goods', @level2type = N'COLUMN', @level2name = N'TaskId';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[BarCode].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Штрихкод (короб)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'BarCode';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[CreateDateTime].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Дата создания', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'CreateDateTime';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[DivisionId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор подразделение', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'DivisionId';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[EndDateTime].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Дата окончания', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'EndDateTime';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[Id].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'Id';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[ParentId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор родителя', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'ParentId';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[PlanNum].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Номер планирования', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'PlanNum';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[StatusId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Статус задачи', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'StatusId';


GO
PRINT N'Выполняется создание [dbo].[Tasks].[UserId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор пользователя', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'UserId';
");
        }
    }
}
