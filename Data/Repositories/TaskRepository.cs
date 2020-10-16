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

            UnitOfWork.Session.Execute(@"
IF NOT EXISTS (SELECT Id FROM Tasks WHERE PlanNum = @PlanNum)
BEGIN
	INSERT INTO Tasks (StatusId, 
                UserId, 
                DivisionId, 
                CreateDateTime, 
                PlanNum, 
                BarCode)
    VALUES((select Id from hTaskStatus where Code='Start'),
            @UserId,
            @DivisionId,
            GETDATE(),
            @PlanNum)	
END", new { @PlanNum = _query.PlanNum, @UserId = _query.UserId, @DivisionId = _query.DivisionId });

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
    join hTaskStatus ht on ht.Id = t.StatusId and ht.Code = 'Start'
	WHERE t.DivisionId = @DivisionId
END
ELSE
BEGIN
	SELECT TOP 1 t.*
	FROM Tasks t
    join hTaskStatus ht on ht.Id = t.StatusId and ht.Code = 'Start'
    WHERE t.UserId = @UserId
END", new { UserId = _query.UserId, DivisionId = _query.DivisionId });

            return entity;
        }

        public Tasks GetTaskById(Query query)
        {

            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirstOrDefault<Tasks>($@"
select * from Tasks t 		
join hTaskStatus ht on ht.Id = t.StatusId and ht.Code = 'Start'
WHERE t.Id = @TaskId", new { @TaskId = _query.TaskId });
            return entity;
        }

        public void EndTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
UPDATE Tasks SET StatusId = (select Id from hTaskStatus where Code='End'), 
                      EndDateTime = GETDATE()
WHERE Id = @TaskId", new { @TaskId = _query.TaskId });
        }

        public void SaveAct(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as TaskQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
INSERT INTO Scaner_File
VALUES (@TaskId, @BoxId, @Path,(select Id hFileType from where Code='Act_Photo'))", new { @TaskId = _query.TaskId, @BoxId = _query.BoxId, @Path = _query.Path });
        }
    }
}
