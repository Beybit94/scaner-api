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
SELECT Id,
       GoodId,
       [CountQty] as Count,
       [GoodName],
       [GoodArticle],
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
SELECT Id,
       GoodId,
       [CountQty] as Count,
       [GoodName],
       [GoodArticle],
       BarCode
FROM Scaner_Goods
WHERE WmsTaskId = @TaskId 
and BoxId is null", new { @TaskId = _query.TaskId });
            return entity.ToList();
        }

        public List<Goods> GetGoodsByBox(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods>(@"
SELECT Id,
       GoodId,
       [CountQty] as Count,
       [GoodName],
       [GoodArticle],
       BarCode
FROM Scaner_Goods
WHERE WmsTaskId = @TaskId 
and BoxId = @BoxId", new { @TaskId = _query.TaskId, @BoxId = _query.BoxId });
            return entity.ToList();
        }

        public void UnloadGood(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"Scaner_SetGood", new { @PlanNum = _query.PlanNum, @GoodBarCode = _query.BarCode, @Id = _query.TaskId, @BoxId = _query.BoxId }, commandType: CommandType.StoredProcedure);
        }

        public void Update(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
update Scaner_Goods set CountQty = @CountQty where Id = @Id", new { CountQty = _query.CountQty, Id = _query.Id });
        }

        public void Delete(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
delete from Scaner_Goods where Id = @Id", new { Id = _query.Id });
        }
    }
}
