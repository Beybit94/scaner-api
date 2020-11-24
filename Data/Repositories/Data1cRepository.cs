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
    right join Scaner_Goods sg on sg.GoodId = g.Id and sg.TaskId = wt.Id 
    right join Scaner_1cDocData dd on dd.Article = g.GoodArticle and dd.PlanNum = wt.PlanNum
    where sg.CountQty <> dd.Quantity
    group by    g.Id,
                g.GoodName,
                g.GoodArticle,
                sg.CountQty,
                dd.Quantity
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

        public List<Receipt> GetNumberDocs(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Receipt>(@"
select  t.Id as TaskId,
        t.PlanNum,
        dd.NumberDoc,
        dd.DateDoc
from Tasks t
join Scaner_1cDocData dd (nolock) on dd.PlanNum = t.PlanNum
where t.StatusId = @StatusId 
and  dd.NumberDoc is not null 
and NOT EXISTS(select Id from Logs where TaskId = t.Id and ProcessTypeId = @ProcessTypeId)
group by t.Id,
         t.PlanNum,
         dd.NumberDoc,
         dd.DateDoc", new { _query.StatusId, _query.ProcessTypeId });

            return entity.ToList();
        }

        public List<ReceiptGood> GetDataByNumberDoc(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<ReceiptGood>(@"
select  dd.Barcode,
        dd.Article,
        g.CountQty as Quantity
from Scaner_1cDocData dd (nolock)
cross apply(select g.GoodArticle,g.CountQty
            from Tasks t 
            join Scaner_Goods g on g.TaskId = t.Id and g.GoodArticle = dd.Article
            where t.PlanNum = dd.PlanNum) g
where  dd.NumberDoc = @NumberDoc
group  by  dd.Barcode,
        dd.Article,
        g.CountQty", new { _query.NumberDoc });

            return entity.ToList();
        }

        public void SetDataTo1c(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId, Message) VALUES (@TaskId,@ProcessTypeId,@Message)",new { _query.TaskId, _query.ProcessTypeId, _query.Message });
        }
    }
}
