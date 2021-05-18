using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Migrations
{
    [Migration(202105061506)]
    public class TaskTrigger : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Execute.Sql(@"
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
GO");
        }
    }
}
