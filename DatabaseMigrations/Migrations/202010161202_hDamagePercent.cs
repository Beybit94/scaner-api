using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010161202)]
    public class hDamagePercent : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
ALTER TABLE [dbo].[Scaner_Goods] DROP CONSTRAINT [FK_Scaner_Goods_hDefectType_DefectTypeId];
ALTER TABLE [dbo].[Scaner_Goods] DROP COLUMN [DefectTypeId];
ALTER TABLE [dbo].[Scaner_Goods] ADD [DamagePercentId] INT NULL;

GO
CREATE TABLE [dbo].[hDamagePercent] (
    [Id]   INT  IDENTITY (1, 1)  NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    [Code] NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Code] ASC)
);

GO
ALTER TABLE [dbo].[Scaner_Goods] WITH NOCHECK
    ADD CONSTRAINT [FK_Scaner_Goods_hDamagePercent_DamagePercentId] FOREIGN KEY ([DamagePercentId]) REFERENCES [dbo].[hDamagePercent] ([Id]);
GO
ALTER TABLE [dbo].[Scaner_Goods] WITH CHECK CHECK CONSTRAINT [FK_Scaner_Goods_hDamagePercent_DamagePercentId];
");
        }
    }
}
