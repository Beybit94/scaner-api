using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010161211)]
    public class SeedDamagePercent : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
INSERT INTO hDamagePercent (Name, Code) VALUES (N'10%', N'10')
INSERT INTO hDamagePercent (Name, Code) VALUES (N'20%', N'20')
INSERT INTO hDamagePercent (Name, Code) VALUES (N'21-30%', N'30')
INSERT INTO hDamagePercent (Name, Code) VALUES (N'50%', N'50')
INSERT INTO hDamagePercent (Name, Code) VALUES (N'60%', N'60')
INSERT INTO hDamagePercent (Name, Code) VALUES (N'100%', N'100')");
        }
    }
}
