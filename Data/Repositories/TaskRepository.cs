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

            var entity = UnitOfWork.Session.QueryFirst<int>($@"
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

            var entity = UnitOfWork.Session.QueryFirst<Tasks>($@"
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

            var entity = UnitOfWork.Session.QueryFirst<Tasks>($@"
select Id,
       PlanNum,
       BoxNum,
       TaskTypeId
from wms_tasks t 		  
WHERE t.Id = @taskid and t.WmsStatus = 1", new { @taskid = _query.TaskId });
            return entity;
        }

    }
}
