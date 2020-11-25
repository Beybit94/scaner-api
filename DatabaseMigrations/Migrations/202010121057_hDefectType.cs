using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010121057)]
    public class hDefectType : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
ALTER TABLE [dbo].[Scaner_Goods] DROP COLUMN [IsDefect];

GO
ALTER TABLE [dbo].[Scaner_Goods] ADD [DefectTypeId] INT NULL;

GO
CREATE TABLE [dbo].[hDefectType] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (255) NOT NULL,
    [Code] NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Code] ASC)
);

GO
ALTER TABLE [dbo].[Scaner_Goods] WITH NOCHECK
    ADD CONSTRAINT [FK_Scaner_Goods_hDefectType_DefectTypeId] FOREIGN KEY ([DefectTypeId]) REFERENCES [dbo].[hDefectType] ([Id]);

GO
ALTER TABLE [dbo].[Scaner_Goods] WITH CHECK CHECK CONSTRAINT [FK_Scaner_Goods_hDefectType_DefectTypeId];");
        }
    }
}
