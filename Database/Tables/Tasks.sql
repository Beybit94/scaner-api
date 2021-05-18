CREATE TABLE [dbo].[Tasks]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
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

GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Tasks_PlanNum_All]
ON [Tasks] ([PlanNum])
WHERE (StatusId in (1,2,3,4));

GO
CREATE INDEX [IX_Tasks_PlanNum] ON [dbo].[Tasks] ([PlanNum])

GO

CREATE TRIGGER [dbo].[Trigger_Tasks]
    ON [dbo].[Tasks]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON

        DECLARE @Id int,
                @PlanNum nvarchar(150), 
                @StatusId int,
                @ProcessTypedId int;

        IF EXISTS(SELECT * FROM INSERTED)
        BEGIN
            SELECT @Id = Id, @PlanNum = PlanNum, @StatusId = StatusId FROM INSERTED
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Task_Start');

            INSERT INTO Logs (TaskId, ProcessTypeId) VALUES (@Id, @ProcessTypedId);
        END
        ELSE
        BEGIN
            SELECT @Id = I.Id, @PlanNum = I.PlanNum, @StatusId = I.StatusId 
            FROM INSERTED I
            JOIN DELETED D ON D.Id = I.Id

            IF @StatusId IN (SELECT Id FROM hTaskStatus WHERE Code IN ('End'))
            BEGIN
                SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Task_End');
            END
            ELSE IF @StatusId IN (SELECT Id FROM hTaskStatus WHERE Code IN ('Deleted'))
            BEGIN
                SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Task_Close');
            END

            INSERT INTO Logs (TaskId, ProcessTypeId) VALUES (@Id, @ProcessTypedId);
        END
    END