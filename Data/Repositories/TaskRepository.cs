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
FROM [WebProject].[dbo].[ROT1c1] pm (nolock)
join [WebProject].[dbo].[Inventory_Taskss] it (nolock) on it.ROT = pm._Number
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

        public int GetTaskId(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirst<int>("WMS_GetActiveTaskListNew", new { UserId = _query.UserId, DivisionId = _query.DivisionId }, commandType: CommandType.StoredProcedure);
            return entity;
        }

        public List<Goods> GetActiveTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods>("WMS_GetActiveTaskNew", new { @taskid = _query.TaskId }, commandType: CommandType.StoredProcedure);
            return entity.ToList();
        }
    }
}
