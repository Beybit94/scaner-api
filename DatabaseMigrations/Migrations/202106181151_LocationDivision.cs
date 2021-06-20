using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202106181151)]
    public class LocationDivision : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
PRINT N'Идет создание Таблица [dbo].[Divisions]…';


GO
CREATE TABLE [dbo].[Divisions] (
    [Id]               INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]             NVARCHAR (250)   NOT NULL,
    [Guid]             UNIQUEIDENTIFIER NOT NULL,
    [ParentId]         INT              NULL,
    [IsDeleted]        BIT              NOT NULL,
    [Code]             NVARCHAR (10)    NULL,
    [GeoX]             NVARCHAR (100)   NULL,
    [GeoY]             NVARCHAR (100)   NULL,
    [IDRRef]           BINARY (16)      NULL,
    [PriceTypeIDRRef]  BINARY (16)      NULL,
    [CategoryRatingId] INT              NULL,
    CONSTRAINT [PK_Divisions] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Идет создание Таблица [dbo].[Locations]…';


GO
CREATE TABLE [dbo].[Locations] (
    [LocationId]     INT              IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [LocationCode]   NVARCHAR (10)    NULL,
    [LocationName]   NVARCHAR (250)   NULL,
    [LocationPrefix] NVARCHAR (4)     NULL,
    [LocationGuid]   UNIQUEIDENTIFIER NULL,
    [DivisionId]     INT              NULL,
    [IDRRef]         BINARY (16)      NULL,
    [IsDeleted]      INT              NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([LocationId] ASC)
);


GO
PRINT N'Идет создание Ограничение по умолчанию [dbo].[DF_Divisions_IsDeleted]…';


GO
ALTER TABLE [dbo].[Divisions]
    ADD CONSTRAINT [DF_Divisions_IsDeleted] DEFAULT ((0)) FOR [IsDeleted];


GO
PRINT N'Идет создание Внешний ключ [dbo].[FK_Divisions_Divisions]…';


GO
ALTER TABLE [dbo].[Divisions] WITH NOCHECK
    ADD CONSTRAINT [FK_Divisions_Divisions] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[Divisions] ([Id]);

GO
ALTER TABLE [dbo].[Divisions] WITH CHECK CHECK CONSTRAINT [FK_Divisions_Divisions];");
        }
    }
}
