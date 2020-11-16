CREATE TABLE [dbo].[BarCodes]
(
	[Id] INT NOT NULL ,
	[BarCode] [nvarchar](200) NOT NULL,
	[isMain] [bit] NULL,
	[BarCodeTypeId] [int] NULL,
	[DateAdd] [datetime] NULL DEFAULT getdate(), 
    CONSTRAINT [PK_BarCodes] PRIMARY KEY ([Id]),
)
