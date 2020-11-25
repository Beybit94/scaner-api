using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202011241524)]
    public class Getfrom1c : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO hProcessType (Name, Code) VALUES (N'Получение данных с 1С', N'GetFrom1C')

GO
ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_Logs_Tasks_TaskId];
ALTER TABLE [dbo].[Logs] ALTER COLUMN [TaskId] INT NULL;

ALTER TABLE [dbo].[Logs] WITH NOCHECK
    ADD CONSTRAINT [FK_Logs_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [dbo].[Tasks] ([Id]);

ALTER TABLE [dbo].[Logs] WITH CHECK CHECK CONSTRAINT [FK_Logs_Tasks_TaskId];");
        }
    }
}
