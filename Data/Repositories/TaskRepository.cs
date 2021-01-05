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
where t.StatusId = @StatusId
and NOT EXISTS(select Id from Logs where TaskId = t.Id and ProcessTypeId = @ProcessTypeId)", new { _query.StatusId, _query.ProcessTypeId });
            return entity.ToList();
        }
        
        public Tasks GetTaskById(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            return UnitOfWork.Session.QueryFirstOrDefault<Tasks>(@"select * from Tasks where Id = @TaskId", new { _query.TaskId });
        }

        public void GetPlanNum(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute($@"
IF EXISTS (SELECT PlanNum FROM Scaner_1cDocData WHERE PlanNum = @PlanNum)
BEGIN
    INSERT INTO Logs (ProcessTypeId, Response) VALUES (1,@PlanNum)
END
ELSE
BEGIN
    INSERT INTO Logs (ProcessTypeId, Response) VALUES (1,'Документ с номером'+@PlanNum+'не найден')
    RAISERROR ( 'Документ с таким номером не найден',1,1)
END", new { _query.PlanNum });
        }

        public void UnloadTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
IF NOT EXISTS (SELECT Id FROM Tasks WHERE PlanNum = @PlanNum)
BEGIN
	INSERT INTO Tasks (StatusId, 
                UserId, 
                DivisionId, 
                CreateDateTime, 
                PlanNum)
    VALUES( @StatusId,
            @UserId,
            @DivisionId,
            GETDATE(),
            @PlanNum)	
END", new { _query.PlanNum, _query.UserId, _query.DivisionId, _query.StatusId });

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
    WHERE t.StatusId = @StatusId AND t.DivisionId = @DivisionId
END
ELSE
BEGIN
	SELECT TOP 1 t.*
	FROM Tasks t
    WHERE t.StatusId = @StatusId AND t.UserId = @UserId
END", new { _query.UserId, _query.DivisionId, _query.StatusId });

            return entity;
        }

        public void EndTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
UPDATE Tasks SET StatusId = @StatusId, EndDateTime = GETDATE()
WHERE Id = @TaskId", new { _query.TaskId, _query.StatusId });
        }

        public void CloseTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
DELETE FROM Logs WHERE TaskId = @TaskId
DELETE FROM Scaner_File WHERE TaskId = @TaskId
DELETE FROM Scaner_Goods WHERE TaskId = @TaskId 
DELETE FROM Tasks WHERE ParentId = @TaskId
DELETE FROM Tasks WHERE Id = @TaskId", new { _query.TaskId });
        }

        public void SaveAct(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
INSERT INTO Scaner_File
VALUES (@TaskId, @BoxId, @Path,@TypeId)", new { _query.TaskId, _query.BoxId, _query.Path, _query.TypeId });
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
    }
}
