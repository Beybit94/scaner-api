using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202106181108)]
    public class LogDesc : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
IF OBJECT_ID ('Trigger_Tasks', 'TR') IS NOT NULL
   drop trigger [dbo].[Trigger_Tasks];

IF OBJECT_ID ('Trigger_Scaner_Goods', 'TR') IS NOT NULL
   drop trigger [dbo].[Trigger_Scaner_Goods];

GO
PRINT N'Идет удаление Индекс [dbo].[Scaner_1cDocData].[IX_BarCode-20210614-125314]…';


GO
DROP INDEX [IX_BarCode-20210614-125314]
    ON [dbo].[Scaner_1cDocData];


GO
PRINT N'Идет удаление Ограничение по умолчанию ограничение без названия для [dbo].[Logs]…';


GO
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [DF__tmp_ms_xx__Creat__02FC7413];


GO
PRINT N'Идет удаление Внешний ключ [dbo].[FK_Logs_hProcessType_ProcessTypeId]…';


GO
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId];


GO
PRINT N'Идет удаление Внешний ключ [dbo].[FK_Logs_Tasks_TaskId]…';


GO
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_Logs_Tasks_TaskId];


GO
PRINT N'Выполняется запуск перестройки таблицы [dbo].[Logs]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Logs] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [TaskId]        INT            NULL,
    [ProcessTypeId] INT            NOT NULL,
    [Created]       DATETIME       DEFAULT getdate() NOT NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [Ended]         DATETIME       NULL,
    [ParentId]      INT            NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Logs1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Logs])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Logs] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Logs] ([Id], [TaskId], [ProcessTypeId], [Created], [Ended], [ParentId])
        SELECT   [Id],
                 [TaskId],
                 [ProcessTypeId],
                 [Created],
                 [Ended],
                 [ParentId]
        FROM     [dbo].[Logs]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Logs] OFF;
    END

DROP TABLE [dbo].[Logs];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Logs]', N'Logs';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK_Logs1]', N'PK_Logs', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Идет создание Внешний ключ [dbo].[FK_Logs_hProcessType_ProcessTypeId]…';


GO
ALTER TABLE [dbo].[Logs] WITH NOCHECK
    ADD CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId] FOREIGN KEY ([ProcessTypeId]) REFERENCES [dbo].[hProcessType] ([Id]);


GO
PRINT N'Идет создание Внешний ключ [dbo].[FK_Logs_Tasks_TaskId]…';


GO
ALTER TABLE [dbo].[Logs] WITH NOCHECK
    ADD CONSTRAINT [FK_Logs_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);

GO
ALTER TABLE [dbo].[Logs] WITH CHECK CHECK CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId];
ALTER TABLE [dbo].[Logs] WITH CHECK CHECK CONSTRAINT [FK_Logs_Tasks_TaskId];");
        }
    }
}
