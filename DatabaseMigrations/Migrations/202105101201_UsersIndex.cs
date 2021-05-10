using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202105101201)]
    public class UsersIndex : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
CREATE NONCLUSTERED INDEX [IX_Users_UserName]
    ON [dbo].[Users]([UserName] ASC);");
        }
    }
}
