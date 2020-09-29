using Dapper;
using Data.Model;
using Data.Queries.Task;
using Data.Repositories.Base;
using ScanerApi.Data.Access;
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

        public int GetPlanNum(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirstOrDefault<int>($@"
SELECT count(pm.PlanNum)
FROM ROT1c1 pm (nolock)
join Inventory_Taskss it (nolock) on it.ROT = pm._Number
WHERE pm.[PlanNum] = @PlanNum ", new { _query.PlanNum });
            return entity;
        }

        public void UnloadTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute("wms_CreateTaskSscaner", new { @PlanNum = _query.PlanNum, @userid = _query.UserId }, commandType: CommandType.StoredProcedure);
        }

        public Tasks GetActiveTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirstOrDefault<Tasks>($@"
IF @UserId = 0
BEGIN
	SELECT TOP 1 Id,PlanNum,BoxNum,TaskTypeId 
    FROM wms_tasks t
	WHERE t.DivisionId = @DivisionId AND t.WmsStatus = 1
END
ELSE
BEGIN
	IF (SELECT COUNT(*) FROM wms_tasks WHERE UserId = @UserId AND WmsStatus = 1) > 0
	BEGIN
		SELECT TOP 1 Id,PlanNum,BoxNum,TaskTypeId
		FROM wms_tasks t
		WHERE t.UserId = @UserId AND t.WmsStatus = 1
		order by 1 desc
			
	END
	ELSE
	BEGIN
		SELECT TOP 1 Id,PlanNum,BoxNum,TaskTypeId
        FROM wms_tasks t
		WHERE t.DivisionId = @DivisionId AND t.WmsStatus = 1
		order by 1 desc
	END
END", new { UserId = _query.UserId, DivisionId = _query.DivisionId });

            return entity;
        }

        public Tasks GetTaskById(Query query)
        {

            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirstOrDefault<Tasks>($@"
select Id,
       PlanNum,
       BoxNum,
       TaskTypeId
from wms_tasks t 		  
WHERE t.Id = @taskid and t.WmsStatus = 1", new { @taskid = _query.TaskId });
            return entity;
        }

        public void EndTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
INSERT INTO wms_taskResult 
SELECT [WmsTaskId],
       [GoodId],
       [CountQty],
       [BarCode],
       [GoodArticle], 
       '0',
       [Favorite],
       [PlanNum] 
FROM Scaner_Goods where WmsTaskId = @TaskId

UPDATE wms_tasks SET [WmsStatus] = 2, CloseDate = GETDATE()
WHERE Id = @TaskId", new { @TaskId = _query.TaskId });
        }

        public List<Differences> Differences(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Differences>(@"
select sg.Id
	   , cd.NumberDoc
	   ,sg.[GoodId]
	   ,cd.Article as GoodArticle
	   ,g.GoodName
		, cd.Quantity as Quantity
		,[CountQty]
		,[ExcessQty]
		,WmsTaskId as TaskId
		, gg.GoodGroupName
		, wtr.Favorite
		, sg.Img
		, wt.Text1
		, (u.UserFirstName + ' ' + u.UserSecondName) as UserName 
		, wt.CreationDate
		, wt.WmsStatus as Status
from [Scaner_Goods]  sg
join WebProject.dbo.Goods g (nolock) on g.GoodId = sg.GoodId
join WebProject.dbo.GoodGroups gg (nolock) on gg.GoodGroupId = g.GoodGroupId
join [WebProject].[dbo].[wms_tasks] wt (nolock) on wt.Id = sg.WmsTaskId
join WebProject.dbo.Users u (nolock) on u.UserId = wt.CreationUserId 
left join [WebProject].[dbo].[wms_taskResult] wtr (nolock) on wtr.wms_taskId = sg.WmsTaskId and wtr.GoodArticle = sg.GoodArticle
left join [WebProject].[dbo].[Scaner_1cDocData] cd (nolock) on cd.PlanNum = @PlanNum
WHERE [WmsTaskId] = @id	", new { Id = _query.TaskId, @PlanNum = _query.PlanNum }).ToList();
            return entity;
        }

        public void SaveAct(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
INSERT INTO Scaner_Act VALUES (@TaskId, @BoxId, @Path)", new { @TaskId = _query.TaskId, @BoxId = _query.BoxId, @Path = _query.Path });
        }
    }
}
