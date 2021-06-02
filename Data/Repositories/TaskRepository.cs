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

            //var entity = UnitOfWork.Session.Query<Tasks>(@"
            //select t.* 
            //from Tasks t
            //where t.Id in (770)"); // 755
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
                session.Execute(@"
IF NOT EXISTS (SELECT top 1 PlanNum FROM Scaner_1cDocData with(nolock) WHERE PlanNum = @PlanNum)
BEGIN
    INSERT INTO Logs (ProcessTypeId, Response) VALUES (18,'Документ с номером '+ @PlanNum+'не найден');
    RAISERROR ('Планирование не найдено!',17,1);
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
                @PlanNum)	
    END
    ELSE
    BEGIN
        RAISERROR ('Планирование уже существует',17,1);
    END
END", new { _query.PlanNum, _query.UserId, _query.DivisionId, _query.Start, _query.InProcess, _query.End });
            

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
                session.Execute(@"
UPDATE Tasks SET StatusId = @StatusId, EndDateTime = @EndDateTime
WHERE Id = @TaskId", new { _query.TaskId, _query.StatusId, _query.EndDateTime });
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
INSERT INTO Logs (TaskId, ProcessTypeId, Response) 
VALUES (@TaskId, (select Id from hProcessType where Code = 'File'), @TypeName);", new { _query.TaskId, _query.TypeName }, trans);

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }           
            }
        }

        public List<Differences> Differences(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Differences>($@"
select g.GoodName,g.GoodArticle,g.Quantity,g.CountQty
from (
    select  g.GoodName,
            g.GoodArticle, 
            dd.Quantity,
            ISNULL(sg.CountQty,0) as CountQty
    from Scaner_1cDocData dd
    join Goods g on g.GoodArticle = dd.Article
    outer apply(select sg.CountQty as CountQty from Scaner_Goods sg where sg.GoodArticle = dd.Article and sg.TaskId = @TaskId) sg
    where dd.PlanNum = '{_query.PlanNum.Trim()}'
    union
    select  g.GoodName,
            g.GoodArticle, 
            ISNULL(dd.Quantity,0) as Quantity,
            sg.CountQty
    from Scaner_Goods sg
    join Goods g on g.Id = sg.GoodId
    outer apply(select dd.Quantity from Scaner_1cDocData dd where sg.GoodArticle = dd.Article and dd.PlanNum = '{_query.PlanNum.Trim()}') dd
    where sg.TaskId = @TaskId
) g
where g.Quantity <> g.CountQty
group by g.GoodName,g.GoodArticle,g.Quantity,g.CountQty", new { _query.TaskId, _query.PlanNum }).ToList();
            return entity;
        }

        public List<Logs> LogsByTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            return UnitOfWork.Session.Query<Logs, Task, Goods, Logs>(@"
select hp.Name as ProcessName, 
       l.Response as Response,
       l.Request as Request,
       l.Created as Created,
       t.*,
       g.*
from Logs l
join hProcessType hp on hp.Id = l.ProcessTypeId
left join Tasks t on t.Id = l.TaskId
left join Scaner_Goods g on g.Id = l.GoodId
where l.TaskId = @TaskId
order by l.Created", (l, t,g) =>
            {
                l.Task = t;
                l.Good = g;
                return l;
            }, new { _query.TaskId }).ToList();
        }

        /// <summary>
        /// Получение адреса по задаче
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public string GetAddressLocationByTask(int taskId)
        {
            try
            {
                var procedure = "[GetAddressLocationByTask]";
                var values = new { taskId = taskId };
                var entity = UnitOfWork.Session.Query<string>(procedure, values, commandType: CommandType.StoredProcedure);
                return entity.FirstOrDefault().Trim();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            

        }
    }
}
