using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202011231452)]
    public class LogMessage : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [DF__Logs__Created__7A672E12];
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_Logs_Tasks_TaskId];
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId];

GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Logs] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [TaskId]        INT            NOT NULL,
    [ProcessTypeId] INT            NOT NULL,
    [Created]       DATETIME       DEFAULT getdate() NOT NULL,
    [Message]       NVARCHAR (MAX) NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK_Logs1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Logs])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_Logs] ON;
        INSERT INTO [dbo].[tmp_ms_xx_Logs] ([Id], [TaskId], [ProcessTypeId], [Created])
        SELECT   [Id],
                 [TaskId],
                 [ProcessTypeId],
                 [Created]
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
ALTER TABLE [dbo].[Logs] WITH NOCHECK
    ADD CONSTRAINT [FK_Logs_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);

ALTER TABLE [dbo].[Logs] WITH NOCHECK
    ADD CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId] FOREIGN KEY ([ProcessTypeId]) REFERENCES [dbo].[hProcessType] ([Id]);



GO
ALTER TABLE [dbo].[Logs] WITH CHECK CHECK CONSTRAINT [FK_Logs_Tasks_TaskId];
ALTER TABLE [dbo].[Logs] WITH CHECK CHECK CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId];
");
        }
    }
}
