CREATE TABLE [dbo].[Scaner_Goods]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[TaskId] INT NOT NULL, 
    [GoodId] INT NULL, 
    [GoodArticle] NVARCHAR(50) NULL, 
    [GoodName] NVARCHAR(500) NULL, 
    [CountQty] INT NULL, 
    [BarCode] NVARCHAR(50) NULL, 
    [BoxId] INT NULL, 
    [DamagePercentId] INT NULL, 
    [Created] DATETIME NOT NULL DEFAULT getdate(), 
    CONSTRAINT [FK_Scaner_Goods_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [Tasks]([Id]), 
    CONSTRAINT [FK_Scaner_Goods_hDamagePercent_DamagePercentId] FOREIGN KEY ([DamagePercentId]) REFERENCES [hDamagePercent]([Id]) 
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

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Процент повреждение',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Scaner_Goods',
    @level2type = N'COLUMN',
    @level2name = 'DamagePercentId'
GO

CREATE TRIGGER [dbo].[Trigger_Scaner_Goods]
    ON [dbo].[Scaner_Goods]
    FOR DELETE, INSERT, UPDATE
    AS 
    BEGIN
        SET NOCOUNT ON;

        DECLARE @TaskId int,
                @Barcode nvarchar(50), 
                @Article nvarchar(50),
                @DamagePercentId int,
                @ProcessTypedId int;

        IF EXISTS(SELECT * FROM DELETED)
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'DeleteGood')
            SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode FROM DELETED

            DELETE Tasks WHERE ParentId = @TaskId AND BarCode = @Barcode;
            INSERT INTO Logs (TaskId, ProcessTypeId, Response) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article);
        END
        ELSE IF EXISTS(SELECT * FROM INSERTED)
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'CreateGood')
            SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode FROM INSERTED

            IF EXISTS(SELECT * FROM Boxes WHERE BarCode = @Barcode)
            BEGIN
                INSERT INTO Tasks (StatusId,UserId,DivisionId,CreateDateTime,PlanNum,BarCode,ParentId)
                SELECT (SELECT TOP 1 Id FROM hTaskStatus WHERE Code = 'NotStarted'),
                        T.UserId,
                        T.DivisionId,
                        GETDATE(),
                        T.PlanNum,
                        @Barcode,
                        T.Id
                FROM Tasks T WHERE T.Id = @TaskId;
            END
            
            INSERT INTO Logs (TaskId, ProcessTypeId, Response) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article);
        END
        ELSE
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'UpdateGood')
            SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode, @DamagePercentId = DamagePercentId 
            FROM INSERTED
            JOIN DELETED ON DELETED.Id = INSERTED.Id

            IF ISNULL(@DamagePercentId,0) <> 0
            BEGIN
                INSERT INTO Logs (TaskId, ProcessTypeId, Response) 
                VALUES (@TaskId,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Defect'),'Артикуль:'+@Article);

                IF EXISTS(SELECT * FROM Boxes WHERE BarCode = @Barcode) AND 
                   NOT EXISTS(SELECT * FROM Tasks WHERE ParentId = @TaskId AND BarCode = @Barcode)
                BEGIN
                    INSERT INTO Tasks (StatusId,UserId,DivisionId,CreateDateTime,PlanNum,BarCode,ParentId)
                    SELECT (SELECT TOP 1 Id FROM hTaskStatus WHERE Code = 'NotStarted'),
                            T.UserId,
                            T.DivisionId,
                            GETDATE(),
                            T.PlanNum,
                            @Barcode,
                            T.Id
                    FROM Tasks T WHERE T.Id = @TaskId;
                END
            END
            ELSE
            BEGIN
                DELETE Tasks WHERE ParentId = @TaskId AND BarCode = @Barcode;

                INSERT INTO Logs (TaskId, ProcessTypeId, Response) 
                VALUES (@TaskId,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Undefect'),'Артикуль:'+@Article);
            END

            INSERT INTO Logs (TaskId, ProcessTypeId, Response) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article);
        END

    END