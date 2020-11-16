using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202011161017)]
    public class ScannerGoodsCreated : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"ALTER TABLE [dbo].[Scaner_Goods] ADD [Created] DATETIME DEFAULT getdate() NOT NULL;");
        }
    }
}
