CREATE TABLE [dbo].[hTaskStatus]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(255) NOT NULL, 
    [Code] NVARCHAR(250) NOT NULL UNIQUE
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'hTaskStatus',
    @level2type = N'COLUMN',
    @level2name = N'Id'