CREATE TABLE [dbo].[Scaner_Goods]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[TaskId] INT NOT NULL, 
    [GoodId] INT NULL , 
    [GoodArticle] NVARCHAR(50) NULL , 
    [GoodName] NVARCHAR(500) NULL , 
    [CountQty] INT NULL , 
    [BarCode] NVARCHAR(50) NULL , 
    [BoxId] INT NULL , 
    [Created] DATETIME NOT NULL DEFAULT FORMAT(getdate(),'dd-MM-yyyy hh:mm:ss'), 
    [DefectId] INT NULL , 
    CONSTRAINT [FK_Scaner_Goods_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks]([Id]), 
    CONSTRAINT [FK_Scaner_Goods_Defects_DefectId] FOREIGN KEY ([DefectId]) REFERENCES [Defects]([Id]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор задачи',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = 'TaskId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор товара',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'GoodId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор короба',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'BoxId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Артикуль товара',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'GoodArticle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Название товара',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'GoodName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Количество',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'CountQty'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Штрихкод',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'BarCode'
GO


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Дефект',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = N'DefectId'