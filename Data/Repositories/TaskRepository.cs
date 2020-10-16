﻿using Dapper;
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

        public void SaveDataFrom1c(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
insert into Scaner_Goods ([PlanNum], [DateDoc], [GUID_PlanWMSNumber],[NumberDoc],[TypeDoc],[Article],[Barcode],[Quantity],[UserId])
values ( isnull(@PlanNum,0),
         isnull(@DateDoc, '08.08.2020 0:00:00'),
         isnull(@Planguid,'0'),
         isnull(@NumberDoc,0),
         isnull(@TypeDoc,0),
         isnull(@Article,0),
         isnull(@Barcode,0),
         isnull(@Quantity,0),
         isnull(@userid,0))",
         new
         {
             @PlanNum = _query.PlanNum,
             @userid = _query.UserId,
             @DateDoc = _query?.DateDoc,
             @Planguid = _query.Planguid,
             @NumberDoc = _query.NumberDoc,
             @TypeDoc = _query.TypeDoc,
             @Article = _query.Article,
             @Barcode = _query.Barcode,
             @Quantity = _query.Quantity
         });
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
SELECT g.Id,
       g.GoodId,
       g.GoodName,
       g.GoodArticle,
       g.CountQty,
       g.ExcessQty,
       dd.NumberDoc,
       dd.Quantity
from Scaner_Goods g
join wms_tasks wt (nolock) on wt.Id = g.WmsTaskId
join Scaner_1cDocDataNew dd (nolock) on dd.PlanNum = wt.PlanNum
where g.WmsTaskId = @Id", new { Id = _query.TaskId, @PlanNum = _query.PlanNum }).ToList();
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
