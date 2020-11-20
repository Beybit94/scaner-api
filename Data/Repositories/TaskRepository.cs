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
FROM Scaner_1cDocData pm (nolock)
WHERE pm.[PlanNum] = @PlanNum", new { _query.PlanNum });
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
DELETE FROM Scaner_Goods WHERE TaskId = @TaskId 
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
    }
}
