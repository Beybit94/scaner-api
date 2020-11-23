using Dapper;
using Data.Access;
using Data.Model;
using Data.Queries.Data1c;
using Data.Repositories.Base;
using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class Data1cRepository: Repository<Differences>
    {
        public Data1cRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<Differences> Differences(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Differences>(@"
SELECT g.GoodId,
       g.GoodName,
       g.GoodArticle,
       g.CountQty,
       g.Quantity
from Tasks wt
CROSS APPLY(
    select  g.Id as GoodId,
        g.GoodName,
        g.GoodArticle, 
        ISNULL(sg.CountQty,0) as CountQty,
        ISNULL(dd.Quantity,0) as Quantity
    from Goods g
    left join Scaner_Goods sg on sg.GoodArticle = g.GoodArticle and sg.TaskId = wt.Id 
    left join Scaner_1cDocData dd on dd.Article = g.GoodArticle and dd.PlanNum = wt.PlanNum
) g
where wt.Id = @TaskId
and g.CountQty <> g.Quantity
group by    g.GoodId,
            g.GoodName,
            g.GoodArticle,
            g.CountQty,
            g.Quantity
order by g.GoodId", new { _query.TaskId }).ToList();
            return entity;
        }

        public List<Scaner_1cDocData> GetDataTo1c(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Scaner_1cDocData>(@"
select *
from
(
    select  t.Id as TaskId,
            t.PlanNum,
            dd.NumberDoc,
            dd.DateDoc, 
            dd.Barcode,
            dd.LocationGuid,
            isnull(dd.Quantity,0) as OrderQty,
            g.GoodArticle,
            g.CountQty
    from Tasks t
    join Scaner_Goods g (nolock) on g.TaskId = t.Id
    left join Scaner_1cDocData dd (nolock) on dd.PlanNum = t.PlanNum and g.GoodArticle = dd.Article
    where t.StatusId = @StatusId 
    and NOT EXISTS(select Id from Logs where TaskId = t.Id and ProcessTypeId = @ProcessTypeId)
) c
group by c.TaskId,
         c.PlanNum,
         c.NumberDoc,
         c.DateDoc, 
         c.Barcode,
         c.LocationGuid,
         c.OrderQty,
         c.GoodArticle,
         c.CountQty", new { _query.StatusId, _query.ProcessTypeId });

            return entity.ToList();
        }

        public void SetDataTo1c(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId) VALUES (@TaskId,@ProcessTypeId)",new { _query.TaskId, _query.ProcessTypeId });
        }
    }
}
