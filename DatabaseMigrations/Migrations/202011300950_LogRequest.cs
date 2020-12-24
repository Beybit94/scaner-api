using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202011300950)]
    public class LogRequest : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
ALTER TABLE [dbo].[Logs] ADD [Request] NVARCHAR (MAX) NULL;");
        }
    }
}
