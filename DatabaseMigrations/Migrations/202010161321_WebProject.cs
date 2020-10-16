using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202010161321)]
    public class WebProject : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
CREATE TABLE [dbo].[BarCodes] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [BarCode]       NVARCHAR (200) NOT NULL,
    [isMain]        BIT            NULL,
    [BarCodeTypeId] INT            NULL,
    [DateAdd]       DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[Goods] (
    [Id]                                INT              IDENTITY (1, 1) NOT NULL,
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
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE TABLE [dbo].[GoodsBarcodes] (
    [GoodId]    INT NOT NULL,
    [BarcodeId] INT NOT NULL
);

GO
CREATE TABLE [dbo].[Inventory_Tasks] (
    [Id]             NVARCHAR (50)    NULL,
    [DocNum]         NVARCHAR (50)    NULL,
    [OrderDate]      DATETIME         NULL,
    [LocationFrom]   NVARCHAR (50)    NULL,
    [LocationFromId] INT              NULL,
    [LocationTo]     NVARCHAR (50)    NULL,
    [LocationToId]   INT              NULL,
    [DivisionaName]  NVARCHAR (50)    NULL,
    [Quantity]       NUMERIC (18)     NULL,
    [Volume]         NUMERIC (18, 4)  NULL,
    [OperType]       UNIQUEIDENTIFIER NULL,
    [LocationTO1c]   NVARCHAR (50)    NULL,
    [LocationTO1cc]  BINARY (50)      NULL,
    [ROT]            NVARCHAR (50)    NULL,
    [PlanNum]        NVARCHAR (50)    NULL
);

GO
CREATE TABLE [dbo].[Users] (
    [Id]                 INT              IDENTITY (1, 1) NOT NULL,
    [UserName]           NVARCHAR (50)    NULL,
    [UserGuid]           UNIQUEIDENTIFIER NULL,
    [User1cTNumber]      INT              NULL,
    [UserFirstName]      NVARCHAR (50)    NULL,
    [UserSecondName]     NVARCHAR (50)    NULL,
    [UserMiddleName]     NVARCHAR (50)    NULL,
    [UserStatusId]       INT              NULL,
    [UserPositionId]     INT              NULL,
    [UserPassword]       NVARCHAR (50)    NULL,
    [UserLocationId]     INT              NULL,
    [UserIsDeleted]      BIT              NOT NULL,
    [UserDivisionId]     INT              NULL,
    [UserBarcodeId]      INT              NOT NULL,
    [UserRNN]            BIGINT           NULL,
    [Category]           BINARY (16)      NULL,
    [StartWorkDate]      DATETIME         NULL,
    [IDRRef]             BINARY (16)      NULL,
    [PasswordChangeDate] DATETIME         NULL,
    [ChangPassPeriod]    INT              NULL,
    [UserSulCoin]        DECIMAL (18, 2)  NULL,
    [UserSpentSulcoin]   DECIMAL (18, 2)  NULL,
    [Sex]                NVARCHAR (50)    NULL,
    [DateOfBirth]        NVARCHAR (50)    NULL,
    [Education]          NVARCHAR (MAX)   NULL,
    [UserCat]            NVARCHAR (50)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


ALTER TABLE [dbo].[BarCodes] ADD DEFAULT getdate() FOR [DateAdd];
ALTER TABLE [dbo].[Goods] ADD DEFAULT 0 FOR [IsService];
ALTER TABLE [dbo].[Goods] ADD DEFAULT 0 FOR [IsPackage];
ALTER TABLE [dbo].[GoodsBarcodes] WITH NOCHECK ADD CONSTRAINT [FK_GoodsBarcodes_Goods_GoodId] FOREIGN KEY ([GoodId]) REFERENCES [dbo].[Goods] ([Id]);
ALTER TABLE [dbo].[GoodsBarcodes] WITH NOCHECK ADD CONSTRAINT [FK_GoodsBarcodes_BarCodes_BarcodeId] FOREIGN KEY ([BarcodeId]) REFERENCES [dbo].[BarCodes] ([Id]);

ALTER TABLE [dbo].[GoodsBarcodes] WITH CHECK CHECK CONSTRAINT [FK_GoodsBarcodes_Goods_GoodId];
ALTER TABLE [dbo].[GoodsBarcodes] WITH CHECK CHECK CONSTRAINT [FK_GoodsBarcodes_BarCodes_BarcodeId];");
        }
    }
}
