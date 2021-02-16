using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202102021117)]
    public class FileGoodId : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
EXECUTE sp_rename @objname = N'[dbo].[Scaner_File].[BoxId]', @newname = N'GoodId', @objtype = N'COLUMN';");
        }
    }
}
