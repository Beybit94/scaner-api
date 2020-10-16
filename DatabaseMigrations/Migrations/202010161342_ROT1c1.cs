using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010161342)]
    public class ROT1c1 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
CREATE TABLE [dbo].[ROT1c1] (
    [_IDRRef]                BINARY (16)     NULL,
    [_Date_Time]             DATETIME        NULL,
    [_NumberPrefix]          DATETIME        NULL,
    [_Number]                NVARCHAR (50)   NULL,
    [GoodMoveDocumentIDRRef] BINARY (16)     NULL,
    [DivisionIDRRef]         BINARY (16)     NULL,
    [LocationIDRRef]         BINARY (16)     NULL,
    [Volume]                 NUMERIC (12, 4) NULL,
    [PlanNum]                NVARCHAR (50)   NULL
);");
        }
    }
}
