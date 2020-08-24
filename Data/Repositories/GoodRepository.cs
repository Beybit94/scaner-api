using Dapper;
using Data.Model;
using Data.Queries.Good;
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
    public class GoodRepository : Repository<Goods>
    {
        public GoodRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Goods GetGoodByCode(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirstOrDefault<Goods>(@"
SELECT GoodId,
       [CountQty] as Count,
       isnull([GoodName], 'box'),
       isnull([GoodArticle], 'box'),
       BarCode
FROM Scaner_Goods
WHERE WmsTaskId = @TaskId 
and BarCode = @BarCode", new { @TaskId = _query.TaskId, @BarCode = _query.BarCode });
            return entity;
        }

        public List<Goods> GetGoodsByTask(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods>(@"
SELECT GoodId,
       [CountQty] as Count,
       isnull([GoodName], 'box'),
       isnull([GoodArticle], 'box'),
       BarCode
FROM Scaner_Goods
WHERE WmsTaskId = @TaskId", new { @TaskId = _query.TaskId });
            return entity.ToList();
        }

        public void UnloadGood(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"Scaner_SetGood", new { @PlanNum = _query.PlanNum, @GoodBarCode = _query.BarCode, @Id = _query.TaskId }, commandType: CommandType.StoredProcedure);
        }

        public void Update(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
update Scaner_Goods set CountQty = @CountQty 
where WmsTaskId = @TaskId and GoodArticle = @GoodArticle", new { CountQty = _query.CountQty, TaskId = _query.TaskId, @GoodBarCode = _query.GoodArticle });
        }

        public void Delete(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
delete from Scaner_Goods where WmsTaskId = @TaskId and GoodArticle = @GoodArticle", new { TaskId = _query.TaskId, @GoodBarCode = _query.GoodArticle });
        }
    }
}
