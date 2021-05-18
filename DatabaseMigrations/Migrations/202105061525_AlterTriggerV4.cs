using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202105061525)]
    public class AlterTriggerV4 : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
ALTER TRIGGER [dbo].[Trigger_Scaner_Goods]
    ON [dbo].[Scaner_Goods]
    FOR DELETE, INSERT, UPDATE
    AS 
    BEGIN
        SET NOCOUNT ON;

        DECLARE @TaskId int,
                @Barcode nvarchar(50), 
                @Article nvarchar(50),
                @DefectId int,
                @ProcessTypedId int;

        IF EXISTS(SELECT * FROM DELETED)
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'DeleteGood')
            SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode FROM DELETED

            DELETE Tasks WHERE ParentId = @TaskId AND BarCode = @Barcode;
            INSERT INTO Logs (TaskId, ProcessTypeId, Response) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article+', ШК:'+@Barcode);
        END
        ELSE IF EXISTS(SELECT * FROM INSERTED)
        BEGIN
            IF RTRIM(LTRIM(@Article)) = ''
            BEGIN
                 SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'CreateGood_Search')
            END
            ELSE
            BEGIN
                 SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'CreateGood')
            END

            SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode FROM INSERTED
            
            INSERT INTO Logs (TaskId, ProcessTypeId, Response) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article+', ШК:'+@Barcode);
        END
        ELSE
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'UpdateGood')
            SELECT @TaskId = I.TaskId, @Article = I.GoodArticle, @Barcode = I.BarCode, @DefectId = I.DefectId 
            FROM INSERTED I
            JOIN DELETED D ON D.Id = I.Id

            IF ISNULL(@DefectId,0) <> 0
            BEGIN
                INSERT INTO Logs (TaskId, ProcessTypeId, Response) 
                VALUES (@TaskId,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Defect'),'Артикуль:'+@Article+', ШК:'+@Barcode);

            END
            ELSE
            BEGIN
                INSERT INTO Logs (TaskId, ProcessTypeId, Response) 
                VALUES (@TaskId,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Undefect'),'Артикуль:'+@Article);
            END

            INSERT INTO Logs (TaskId, ProcessTypeId, Response) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article);
        END

    END
GO");
        }
    }
}
