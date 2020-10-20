using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010201133)]
    public class GoodsBarcodesDropFK : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"GO
ALTER TABLE [dbo].[GoodsBarcodes] DROP CONSTRAINT [FK_GoodsBarcodes_BarCodes_BarcodeId];
ALTER TABLE [dbo].[GoodsBarcodes] DROP CONSTRAINT [FK_GoodsBarcodes_Goods_GoodId];");
        }
    }
}
