using Dapper;
using Data.Model;
using Data.Queries.Task;
using Data.Repositories.Base;
using Data.Access;
using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class TaskRepository : Repository<Tasks>
    {
        public TaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<Tasks> GetTasksByStatus(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Tasks>(@"
select t.* 
from Tasks t
where t.StatusId = @End
and NOT EXISTS(select Id from Logs where TaskId = t.Id and ProcessTypeId = @ProcessTypeId)", new { _query.End, _query.ProcessTypeId });
            return entity.ToList();
        }

        public Tasks GetTaskById(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            return UnitOfWork.Session.QueryFirstOrDefault<Tasks>($@"
select * 
from Tasks 
where (Id = @TaskId or PlanNum = @PlanNum)
and StatusId not in (select Id from hTaskStatus where Code in ('Deleted'))", new { _query.TaskId, _query.PlanNum });
        }

        public void UnloadTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var trans = session.BeginTransaction();
                try
                {
                    var Id = session.QueryFirstOrDefault<int>(@"
IF NOT EXISTS (SELECT PlanNum FROM Scaner_1cDocData WHERE PlanNum = @PlanNum)
BEGIN
    INSERT INTO Logs (ProcessTypeId, Description) VALUES ((SELECT TOP 1 Id FROM hProcessType WHERE Code = 'NotFound'),'Документ с номером '+@PlanNum+'не найден')
    RAISERROR ('Документ с таким номером не найден',1,1)
    select 0;
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT Id FROM Tasks WHERE PlanNum = @PlanNum AND StatusId in (@Start,@InProcess,@End))
    BEGIN
	    INSERT INTO Tasks (StatusId, 
                    UserId, 
                    DivisionId, 
                    CreateDateTime, 
                    PlanNum)
        VALUES( @Start,
                @UserId,
                @DivisionId,
                GETDATE(),
                @PlanNum);

        select  SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        RAISERROR ('Планирование уже существует',1,1)
    END
END", new { _query.PlanNum, _query.UserId, _query.DivisionId, _query.Start, _query.InProcess, _query.End }, trans);

                    if(Id != 0)
                    {
                        session.Execute(@"
INSERT INTO Logs (TaskId,ProcessTypeId, Description) 
VALUES (@Id,(SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Task_Start'),@PlanNum)", new { Id, _query.PlanNum }, trans);
                    }
                    
                    trans.Commit();
                }catch(Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public Tasks GetActiveTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirstOrDefault<Tasks>($@"
IF @UserId = 0
BEGIN
	SELECT TOP 1 t.* 
    FROM Tasks t
    WHERE (t.StatusId = @Start or t.StatusId = @InProcess) 
    AND t.DivisionId = @DivisionId
    ORDER By CreateDateTime DESC
END
ELSE
BEGIN
	SELECT TOP 1 t.*
	FROM Tasks t
    WHERE (t.StatusId = @Start or t.StatusId = @InProcess) 
    AND t.UserId = @UserId
    ORDER By CreateDateTime DESC
END", new { _query.UserId, _query.DivisionId, _query.Start, _query.InProcess });

            return entity;
        }

        public void SetStaus(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var transaction = session.BeginTransaction();
                try
                {
                    session.Execute(@"
UPDATE Tasks SET StatusId = @StatusId, EndDateTime = @EndDateTime
WHERE Id = @TaskId", new { _query.TaskId, _query.StatusId, _query.EndDateTime }, transaction);

                    session.Execute(@"
DECLARE @ProcessTypedId int;
IF @StatusId IN (SELECT Id FROM hTaskStatus WHERE Code IN ('End','Deleted'))
BEGIN
    IF @StatusId IN (SELECT Id FROM hTaskStatus WHERE Code IN ('End'))
    BEGIN
        SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Task_End');
    END
    ELSE IF @StatusId IN (SELECT Id FROM hTaskStatus WHERE Code IN ('Deleted'))
    BEGIN
        SET @ProcessTypedId = (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Task_Close');
    END

    INSERT INTO Logs (TaskId, ProcessTypeId) VALUES (@TaskId, @ProcessTypedId);
END", new { _query.TaskId, _query.StatusId }, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
               
            }
        }

        public void CloseTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var transaction = session.BeginTransaction();
                try
                {
                    session.Execute(@"DELETE FROM Scaner_File WHERE TaskId = @TaskId", new { _query.TaskId }, transaction);
                    session.Execute(@"DELETE FROM Scaner_Goods WHERE TaskId = @TaskId", new { _query.TaskId }, transaction);
                    session.Execute(@"DELETE FROM Tasks WHERE ParentId = @TaskId", new { _query.TaskId }, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<ScanerFile> FilesByTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<ScanerFile>(@"select * from Scaner_File where TaskId = @TaskId", new { _query.TaskId });
            return entity.ToList();
        }

        public void SaveAct(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var trans = session.BeginTransaction();
                try
                {
                    session.Execute(@"
INSERT INTO Scaner_File
VALUES (@TaskId, @GoodId, @Path,@TypeId)", new { _query.TaskId, _query.GoodId, _query.Path, _query.TypeId }, trans);
                    session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, (select Id from hProcessType where Code = 'File'), @TypeName);", new { _query.TaskId, _query.TypeName }, trans);

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }           
            }
        }
    }
}
