CREATE TABLE [dbo].[Scaner_File]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [TaskId] INT NOT NULL, 
    [BoxId] INT NULL, 
    [Path] NVARCHAR(500) NOT NULL, 
    [TypeId] INT NOT NULL, 
    CONSTRAINT [FK_Scaner_File_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks]([Id]), 
    CONSTRAINT [FK_Scaner_File_hFileType_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [hFileType]([Id])
)
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Путь к файлу',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_File',
    @level2type = N'COLUMN',
    @level2name = N'Path'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор короба',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_File',
    @level2type = N'COLUMN',
    @level2name = N'BoxId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор задачи',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_File',
    @level2type = N'COLUMN',
    @level2name = N'TaskId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_File',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Тип файла',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_File',
    @level2type = N'COLUMN',
    @level2name = N'TypeId'