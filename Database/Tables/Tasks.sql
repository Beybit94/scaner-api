CREATE TABLE [dbo].[Tasks]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [StatusId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [DivisionId] INT NOT NULL, 
    [CreateDateTime] DATETIME NOT NULL, 
    [EndDateTime] DATETIME NULL, 
    [PlanNum] NVARCHAR(150) NOT NULL, 
    [BarCode] NVARCHAR(50) NULL, 
    [ParentId] INT NULL, 
    CONSTRAINT [FK_Tasks_hTaskStatus_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [hTaskStatus]([Id]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Статус задачи',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = 'StatusId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор пользователя',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'UserId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор подразделение',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'DivisionId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Дата создания',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'CreateDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Дата окончания',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'EndDateTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Номер планирования',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'PlanNum'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Идентификатор родителя',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'ParentId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Штрихкод (короб)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Tasks',
    @level2type = N'COLUMN',
    @level2name = N'BarCode'