using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations
{
    [Migration(202119061426)]
    public class GoodsDateFormat : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
DROP INDEX [IX_BarCode-20210618-123217]
    ON [dbo].[Scaner_1cDocData];


GO
PRINT N'Идет удаление Ограничение по умолчанию ограничение без названия для [dbo].[Scaner_Goods]…';


GO
ALTER TABLE [dbo].[Scaner_Goods] DROP CONSTRAINT [DF__Scaner_Go__Creat__5AEE82B9];


GO
PRINT N'Идет создание Ограничение по умолчанию ограничение без названия для [dbo].[Scaner_Goods]…';


GO
ALTER TABLE [dbo].[Scaner_Goods]
    ADD DEFAULT FORMAT(getdate(),'dd-MM-yyyy hh:mm:ss') FOR [Created];");
        }
    }
}
