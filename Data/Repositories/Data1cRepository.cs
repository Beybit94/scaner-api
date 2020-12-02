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
select g.GoodName,g.GoodArticle, g.Quantity
from (
    select  g.GoodName,g.GoodArticle,(ISNULL(sg.CountQty,0) - ISNULL(dd.Quantity,0)) as Quantity
    from Scaner_Goods sg
    join Goods g on g.GoodArticle = sg.GoodArticle
    outer apply(select dd.Quantity 
                from Scaner_1cDocData 
                dd where dd.Article = sg.GoodArticle 
                and dd.PlanNum = @PlanNum) dd
    where sg.TaskId = @TaskId
    UNION
    select g.GoodName,g.GoodArticle, (ISNULL(sg.CountQty,0) - ISNULL(dd.Quantity,0)) as Quantity
    from Scaner_1cDocData dd
    join Goods g on g.GoodArticle = dd.Article
    outer apply(select sg.CountQty 
                from Scaner_Goods sg 
                where sg.GoodArticle = dd.Article 
                and sg.TaskId = @TaskId) sg 
    where dd.PlanNum = @PlanNum
) g
where g.Cnt <> 0
group by g.GoodName,g.GoodArticle, g.Quantity", new { _query.TaskId }).ToList();
            return entity;
        }

        public List<Receipt> Save1cDocData(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Receipt>(@"
select  t.Id as TaskId,
        t.PlanNum,
        dd.NumberDoc,
        dd.DateDoc
from Tasks t (nolock)
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

        public List<Receipt> GetNumberDocs(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Receipt>(@"
select  t.Id as TaskId,
        t.CreateDateTime as DateBeginLoad,
        t.EndDateTime as DateEndLoad,
        t.PlanNum,
        dd.NumberDoc,
        dd.DateDoc,
        dd.LocationGuid as Location
from Tasks t (nolock)
join Scaner_1cDocData dd (nolock) on dd.PlanNum = t.PlanNum
where t.StatusId = @StatusId 
and  dd.NumberDoc is not null 
and NOT EXISTS(select Id from Logs where TaskId = t.Id and ProcessTypeId = @ProcessTypeId)
group by t.Id,
         t.CreateDateTime,
         t.EndDateTime,
         t.PlanNum,
         dd.NumberDoc,
         dd.DateDoc,
         dd.LocationGuid", new { _query.StatusId, _query.ProcessTypeId });

            return entity.ToList();
        }

        public List<ReceiptGood> GetDataByNumberDoc(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<ReceiptGood>(@"
select  dd.Article,
        g.GoodBarcode,
        g.CountQty as Quantity,
        g.Barcode
from Scaner_1cDocData dd (nolock)
cross apply(select  g.GoodArticle,
                    g.CountQty, 
                    g.BarCode as GoodBarcode,
                    b.BarCode
            from Tasks t 
            join Scaner_Goods (nolock) g on g.TaskId = t.Id and g.GoodArticle = dd.Article
            outer apply(select b.BarCode from Scaner_Goods (nolock) b where b.Id = g.BoxId)b
            where t.PlanNum = dd.PlanNum and ISNULL(g.GoodId,0)<>0) g
where  dd.NumberDoc = @NumberDoc
group by dd.Article,
         g.GoodBarcode,
         g.CountQty,
         g.Barcode", new { _query.NumberDoc });

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
