CREATE TABLE [dbo].[BarCodes]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[BarCode] [nvarchar](200) NOT NULL,
	[isMain] [bit] NULL,
	[BarCodeTypeId] [int] NULL,
	[DateAdd] [datetime] NULL DEFAULT getdate(),
)
