using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202101201506)]
    public class TaskIndexV1 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
CREATE NONCLUSTERED INDEX [IX_Tasks_PlanNum]
    ON [dbo].[Tasks]([PlanNum] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Tasks_PlanNum_All]
    ON [dbo].[Tasks]([PlanNum] ASC) WHERE (StatusId in (1,2,3,4));

");
        }
    }
}
