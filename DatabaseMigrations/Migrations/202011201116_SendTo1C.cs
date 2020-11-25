using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202011201116)]
    public class SendTo1C : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
ALTER TABLE [dbo].[Scaner_1cDocData] ADD [LocationGuid] NVARCHAR (50) NULL;
INSERT INTO hProcessType (Name, Code) VALUES (N'Отправка в 1С', N'SendTo1C')

GO
CREATE TABLE [dbo].[Logs] (
    [Id]            INT      IDENTITY (1, 1) NOT NULL,
    [TaskId]        INT      NOT NULL,
    [ProcessTypeId] INT      NOT NULL,
    [Created]       DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
ALTER TABLE [dbo].[Logs] ADD DEFAULT getdate() FOR [Created];
ALTER TABLE [dbo].[Logs] WITH NOCHECK ADD CONSTRAINT [FK_Logs_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);
ALTER TABLE [dbo].[Logs] WITH NOCHECK ADD CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId] FOREIGN KEY ([ProcessTypeId]) REFERENCES [dbo].[hProcessType] ([Id]);

GO
ALTER TABLE [dbo].[Logs] WITH CHECK CHECK CONSTRAINT [FK_Logs_Tasks_TaskId];
ALTER TABLE [dbo].[Logs] WITH CHECK CHECK CONSTRAINT [FK_Logs_hProcessType_ProcessTypeId];");
        }
    }
}
