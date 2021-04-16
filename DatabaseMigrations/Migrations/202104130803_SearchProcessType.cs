using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202104130803)]
    public class SearchProcessType : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO hProcessType (Name, Code) VALUES (N'Поиск короба/товара по ШК', N'SearchGood')");
        }
    }
}
