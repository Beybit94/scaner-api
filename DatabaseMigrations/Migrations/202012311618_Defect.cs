using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202012311618)]
    public class Defect : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
ALTER TABLE [dbo].[Scaner_Goods] DROP CONSTRAINT [FK_Scaner_Goods_hDamagePercent_DamagePercentId];


GO
PRINT N'Выполняется изменение [dbo].[Scaner_Goods]...';


GO
ALTER TABLE [dbo].[Scaner_Goods] DROP COLUMN [DamagePercentId];


GO
ALTER TABLE [dbo].[Scaner_Goods]
    ADD [DefectId] INT NULL;


GO
PRINT N'Выполняется создание [dbo].[Defects]...';


GO
CREATE TABLE [dbo].[Defects] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Damage]      INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Выполняется создание [dbo].[FK_Scaner_Goods_Defects_DefectId]...';


GO
ALTER TABLE [dbo].[Scaner_Goods] WITH NOCHECK
    ADD CONSTRAINT [FK_Scaner_Goods_Defects_DefectId] FOREIGN KEY ([DefectId]) REFERENCES [dbo].[Defects] ([Id]);");
        }
    }
}
