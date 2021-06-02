using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202106012147)]
    public class LogGood : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
GO
ALTER TABLE [dbo].[Logs]
    ADD [GoodId] INT NULL;


GO
PRINT N'Идет создание Таблица [dbo].[Requests]…';


GO
CREATE TABLE [dbo].[Requests] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CreateDateTime] DATETIME       NOT NULL,
    [EndDateTime]    DATETIME       NULL,
    [json]           NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Идет создание Таблица [dbo].[Responses]…';


GO
CREATE TABLE [dbo].[Responses] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CreateDateTime] DATETIME       NOT NULL,
    [EndDateTime]    DATETIME       NULL,
    [json]           NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Идет создание Внешний ключ [dbo].[FK_Logs_Scaner_Goods_GoodId]…';


GO
ALTER TABLE [dbo].[Logs] WITH NOCHECK
    ADD CONSTRAINT [FK_Logs_Scaner_Goods_GoodId] FOREIGN KEY ([GoodId]) REFERENCES [dbo].[Scaner_Goods] ([Id]);


GO
PRINT N'Идет изменение Триггер [dbo].[Trigger_Scaner_Goods]…';


GO

ALTER TRIGGER [dbo].[Trigger_Scaner_Goods]
    ON [dbo].[Scaner_Goods]
    FOR DELETE, INSERT, UPDATE
    AS 
    BEGIN
        SET NOCOUNT ON;

        DECLARE @TaskId int,
                @GoodId int,
                @Barcode nvarchar(50), 
                @Article nvarchar(50),
                @DefectId int,
                @ProcessTypedId int;

        IF EXISTS(SELECT * FROM INSERTED I
                  JOIN DELETED D ON D.Id = I.Id)
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'UpdateGood')
            SELECT @TaskId = I.TaskId, @GoodId = I.Id, @Article = I.GoodArticle, @Barcode = I.BarCode, @DefectId = I.DefectId 
            FROM INSERTED I
            JOIN DELETED D ON D.Id = I.Id

            IF ISNULL(@DefectId,0) <> 0
            BEGIN
                INSERT INTO Logs (TaskId, GoodId, ProcessTypeId, Response) 
                VALUES (@TaskId, @GoodId,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Defect'),'Артикуль:'+@Article+', ШК:'+@Barcode);

            END
            ELSE
            BEGIN
                INSERT INTO Logs (TaskId, GoodId, ProcessTypeId, Response) 
                VALUES (@TaskId, @GoodId,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Undefect'),'Артикуль:'+@Article+', ШК:'+@Barcode);
            END

            INSERT INTO Logs (TaskId, GoodId, ProcessTypeId, Response) VALUES (@TaskId, @GoodId,@ProcessTypedId,'Артикуль:'+@Article+', ШК:'+@Barcode);

        END
        ELSE IF EXISTS(SELECT * FROM INSERTED)
        BEGIN
            SELECT @TaskId = TaskId, @GoodId = Id, @Article = GoodArticle, @Barcode = BarCode FROM INSERTED
            
            IF RTRIM(LTRIM(@Article)) = ''
            BEGIN
                 SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'CreateGood_Search')
            END
            ELSE
            BEGIN
                 SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'CreateGood')
            END
            
            INSERT INTO Logs (TaskId, GoodId, ProcessTypeId, Response) VALUES (@TaskId, @GoodId,@ProcessTypedId,'Артикуль:'+@Article+', ШК:'+@Barcode);
        END
        ELSE
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'DeleteGood')
            SELECT @TaskId = TaskId, @GoodId = Id, @Article = GoodArticle, @Barcode = BarCode FROM DELETED

            DELETE Tasks WHERE ParentId = @TaskId AND BarCode = @Barcode;
            INSERT INTO Logs (TaskId, GoodId, ProcessTypeId, Response) VALUES (@TaskId, @GoodId,@ProcessTypedId,'Артикуль:'+@Article+', ШК:'+@Barcode);
        END

    END
GO");
        }
    }
}
