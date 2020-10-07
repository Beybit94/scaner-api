CREATE TABLE [dbo].[Scaner_1cDocData]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PlanNum] NVARCHAR(50) NULL, 
    [DateDoc] DATETIME NULL, 
    [GUID_PlanWMSNumber] UNIQUEIDENTIFIER NULL, 
    [NumberDoc] NVARCHAR(50) NULL, 
    [TypeDoc] NVARCHAR(50) NULL, 
    [Article] NVARCHAR(50) NULL, 
    [Barcode] NVARCHAR(50) NULL, 
    [Quantity] INT NULL, 
    [UserId] INT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_1cDocData',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Номер планирования',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_1cDocData',
    @level2type = N'COLUMN',
    @level2name = N'PlanNum'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Артикуль',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_1cDocData',
    @level2type = N'COLUMN',
    @level2name = N'Article'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Штрихкод',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_1cDocData',
    @level2type = N'COLUMN',
    @level2name = N'Barcode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Количество',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_1cDocData',
    @level2type = N'COLUMN',
    @level2name = N'Quantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор пользователя',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_1cDocData',
    @level2type = N'COLUMN',
    @level2name = N'UserId'