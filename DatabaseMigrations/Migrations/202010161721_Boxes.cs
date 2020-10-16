using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010161721)]
    public class Boxes : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
ALTER TABLE [dbo].[Scaner_File] DROP CONSTRAINT [FK_Scaner_File_Tasks_TaskId];
ALTER TABLE [dbo].[Scaner_Goods] DROP CONSTRAINT [FK_Scaner_Goods_Tasks_TaskId];
ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_Tasks_hTaskStatus_StatusId];

GO
BEGIN TRANSACTION;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Tasks] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [StatusId]       INT            NOT NULL,
    [UserId]         INT            NOT NULL,
    [DivisionId]     INT            NOT NULL,
    [CreateDateTime] DATETIME       NOT NULL,
    [EndDateTime]    DATETIME       NULL,
    [PlanNum]        NVARCHAR (150) NOT NULL,
    [BarCode]        NVARCHAR (50)  NULL,
    [ParentId]       INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Tasks])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Tasks] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Tasks] ([Id], [StatusId], [UserId], [DivisionId], [CreateDateTime], [EndDateTime], [PlanNum], [BarCode], [ParentId])
        SELECT   [Id],
                 [StatusId],
                 [UserId],
                 [DivisionId],
                 [CreateDateTime],
                 [EndDateTime],
                 [PlanNum],
                 [BarCode],
                 [ParentId]
        FROM     [dbo].[Tasks]
        ORDER BY [Id] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Tasks] OFF;
    END

DROP TABLE [dbo].[Tasks];
EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Tasks]', N'Tasks';
COMMIT TRANSACTION;
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

GO
CREATE TABLE [dbo].[Boxes] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [BarCode] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


ALTER TABLE [dbo].[Scaner_File] WITH NOCHECK ADD CONSTRAINT [FK_Scaner_File_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);
ALTER TABLE [dbo].[Scaner_Goods] WITH NOCHECK ADD CONSTRAINT [FK_Scaner_Goods_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);
ALTER TABLE [dbo].[Tasks] WITH NOCHECK ADD CONSTRAINT [FK_Tasks_hTaskStatus_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[hTaskStatus] ([Id]);


EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'Id';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Статус задачи', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'StatusId';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор пользователя', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'UserId';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор подразделение', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'DivisionId';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Дата создания', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'CreateDateTime';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Дата окончания', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'EndDateTime';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Номер планирования', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'PlanNum';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Штрихкод (короб)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'BarCode';
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Идентификатор родителя', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Tasks', @level2type = N'COLUMN', @level2name = N'ParentId';
");
        }
    }
}
