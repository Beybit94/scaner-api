using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202011161745)]
    public class GoodsBarcodes : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
drop table GoodsBarcodes
drop table BarCodes
drop table Goods


GO
CREATE TABLE [dbo].[BarCodes] (
    [Id]            INT            NOT NULL,
    [BarCode]       NVARCHAR (200) NOT NULL,
    [isMain]        BIT            NULL,
    [BarCodeTypeId] INT            NULL,
    [DateAdd]       DATETIME       NULL,
    CONSTRAINT [PK_BarCodes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[Goods] (
    [Id]                                INT              NOT NULL,
    [GoodName]                          NVARCHAR (100)   NULL,
    [GoodGroupId]                       INT              NULL,
    [GoodBrandId]                       INT              NULL,
    [GoodParametersId]                  INT              NULL,
    [GoodParametersConfirmed]           BIT              NULL,
    [GoodParametersConfirmationDate]    SMALLDATETIME    NULL,
    [GoodWidth]                         NUMERIC (10, 2)  NULL,
    [GoodHeight]                        NUMERIC (10, 2)  NULL,
    [GoodVolume]                        NUMERIC (5, 4)   NULL,
    [GoodWeight]                        NUMERIC (10, 2)  NULL,
    [GoodBoxQuantity]                   INT              NULL,
    [GoodBoxWeight]                     NUMERIC (10, 2)  NULL,
    [GoodPalletQuantity]                INT              NULL,
    [GoodLength]                        NUMERIC (10, 2)  NULL,
    [GoodsParametersConfirmationUserId] INT              NULL,
    [GoodArticle]                       NVARCHAR (20)    NULL,
    [GoodGUID]                          UNIQUEIDENTIFIER NULL,
    [NDS]                               NUMERIC (10, 2)  NULL,
    [Description]                       NTEXT            NULL,
    [IsService]                         BIT              NOT NULL,
    [IsPackage]                         BIT              NOT NULL,
    [IsSerialNumberNeeded]              BIT              NULL,
    [RoznCategory]                      NUMERIC (2)      NULL,
    [GuarantyPeriodDesc]                NVARCHAR (50)    NULL,
    [IsMarked]                          BIT              NULL,
    [IDRRef]                            BINARY (16)      NULL,
    [Podlozhka]                         BINARY (16)      NULL,
    [PodlozhkaChanged]                  DATETIME         NULL,
    [Cennik_Template]                   INT              NULL,
    [CennikStarDate]                    DATETIME         NULL,
    [CennikEndDate]                     DATETIME         NULL,
    [SellersDirectionId]                INT              NULL,
    [_Version]                          TIMESTAMP        NULL,
    CONSTRAINT [PK_Goods] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[GoodsBarcodes] (
    [GoodId]    INT NOT NULL,
    [BarcodeId] INT NOT NULL
);


GO
ALTER TABLE [dbo].[BarCodes] ADD DEFAULT getdate() FOR [DateAdd];
ALTER TABLE [dbo].[Goods] ADD DEFAULT 0 FOR [IsService];
ALTER TABLE [dbo].[Goods] ADD DEFAULT 0 FOR [IsPackage];


GO
ALTER TABLE [dbo].[GoodsBarcodes] WITH NOCHECK
    ADD CONSTRAINT [FK_GoodsBarcodes_Goods_Id] FOREIGN KEY ([GoodId]) REFERENCES [dbo].[Goods] ([Id]);

ALTER TABLE [dbo].[GoodsBarcodes] WITH NOCHECK
    ADD CONSTRAINT [FK_GoodsBarcodes_BarCodes_Id] FOREIGN KEY ([BarcodeId]) REFERENCES [dbo].[BarCodes] ([Id]);

GO
ALTER TABLE [dbo].[GoodsBarcodes] WITH CHECK CHECK CONSTRAINT [FK_GoodsBarcodes_Goods_Id];
ALTER TABLE [dbo].[GoodsBarcodes] WITH CHECK CHECK CONSTRAINT [FK_GoodsBarcodes_BarCodes_Id];");
        }
    }
}
