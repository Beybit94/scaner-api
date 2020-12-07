using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202012021158)]
    class Scaner_Goods_Triger : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
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
            INSERT INTO Logs (TaskId, ProcessTypeId, Message) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article);
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
            
            INSERT INTO Logs (TaskId, ProcessTypeId, Message) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article);
        END
        ELSE
        BEGIN
            SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'UpdateGood')
            SELECT @TaskId = I.TaskId, @Article = I.GoodArticle, @Barcode = I.BarCode, @DamagePercentId = I.DamagePercentId 
            FROM INSERTED I
            JOIN DELETED ON DELETED.Id = I.Id

            IF ISNULL(@DamagePercentId,0) <> 0
            BEGIN
                INSERT INTO Logs (TaskId, ProcessTypeId, Message) 
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

                INSERT INTO Logs (TaskId, ProcessTypeId, Message) 
                VALUES (@TaskId,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Undefect'),'Артикуль:'+@Article);
            END

            INSERT INTO Logs (TaskId, ProcessTypeId, Message) VALUES (@TaskId,@ProcessTypedId,'Артикуль:'+@Article);
        END

    END
GO");
        }
    }
}
