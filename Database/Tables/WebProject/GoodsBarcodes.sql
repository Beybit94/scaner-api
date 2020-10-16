﻿CREATE TABLE [dbo].[GoodsBarcodes]
(
	[GoodId] [int] NOT NULL,
	[BarcodeId] [int] NOT NULL, 
    CONSTRAINT [FK_GoodsBarcodes_Goods_GoodId] FOREIGN KEY ([GoodId]) REFERENCES [Goods]([Id]), 
    CONSTRAINT [FK_GoodsBarcodes_BarCodes_BarcodeId] FOREIGN KEY ([BarcodeId]) REFERENCES [BarCodes]([Id])
)
