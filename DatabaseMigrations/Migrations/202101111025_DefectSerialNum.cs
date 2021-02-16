using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202101111025)]
    public class DefectSerialNum : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
ALTER TABLE [dbo].[Defects]
    ADD [SerialNumber] NVARCHAR (50) NULL;
");
        }
    }
}
