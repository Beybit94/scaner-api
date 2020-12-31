CREATE TABLE [dbo].[Defects]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Damage] INT NOT NULL, 
    [Description] NVARCHAR(MAX) NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Процент повреждения',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Defects',
    @level2type = N'COLUMN',
    @level2name = N'Damage'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Описание',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Defects',
    @level2type = N'COLUMN',
    @level2name = N'Description'