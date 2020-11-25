using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010121114)]
    public class SeedDefectType : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO hDefectType (Name, Code) VALUES (N'Дефект короба', N'Defect_Box')");
        }
    }
}
