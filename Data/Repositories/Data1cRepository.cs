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
    public class Data1cRepository : Repository<Differences>
    {
        public Data1cRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public List<Receipt> GetNumberDocs(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute($@"
INSERT INTO Logs (ProcessTypeId, Response) VALUES (1,'{_query.TaskId},{_query.PlanNum}')");
            var entity = UnitOfWork.Session.Query<Receipt>($@"
select  t.PlanNum,
        t.Id as TaskId,
        t.CreateDateTime as DateBeginLoad,
        t.EndDateTime as DateEndLoad,
        g.LocationGuid as Location,
        g.NumberDoc as NumberDoc,
        g.Article as Article,    
        g.DateDoc as DateDoc,  
        g.BarCode as BarCode,
        g.Quantity as Quantity,
        g.GoodBarcode as GoodBarcode
from Tasks t (nolock)
cross apply(
    select  sg.TaskId,
            sg.Barcode as GoodBarcode,
            sg.CountQty as Quantity,
            b.Barcode,   
            sg.GoodArticle as Article,        
            ISNULL(dd.NumberDoc,(select TOP 1 dd.NumberDoc from Scaner_1cDocData dd where dd.PlanNum = '{_query.PlanNum}')) as NumberDoc,
            ISNULL(dd.DateDoc,(select TOP 1 dd.DateDoc from Scaner_1cDocData dd where dd.PlanNum = '{_query.PlanNum}')) as DateDoc,
            ISNULL(dd.LocationGuid,(select TOP 1 dd.LocationGuid from Scaner_1cDocData dd where dd.PlanNum = '{_query.PlanNum}')) as LocationGuid
    from Scaner_Goods sg
    left join Scaner_1cDocData dd on dd.Article = sg.GoodArticle and dd.PlanNum = '{_query.PlanNum}'
    outer apply(select b.BarCode from Scaner_Goods (nolock) b where b.Id = sg.BoxId)b
    where sg.TaskId = t.Id
    group by sg.TaskId,sg.Barcode,sg.CountQty,b.Barcode,sg.GoodArticle,dd.NumberDoc,dd.DateDoc,dd.LocationGuid
    UNION
    select  t.Id as TaskId,
            sg.Barcode as GoodBarcode,
            ISNULL(sg.CountQty,0) as Quantity,
            b.Barcode, 
            dd.Article,
            dd.NumberDoc,
            dd.DateDoc,
            dd.LocationGuid
    from Scaner_1cDocData dd 
    left join Scaner_Goods sg on sg.GoodArticle = dd.Article and sg.TaskId = t.Id
    outer apply(select b.BarCode from Scaner_Goods (nolock) b where b.Id = sg.BoxId)b
    where dd.PlanNum = '{_query.PlanNum}'
    group by sg.TaskId,sg.Barcode,sg.CountQty,b.Barcode,dd.Article,dd.NumberDoc,dd.DateDoc,dd.LocationGuid
) g
where t.Id = @TaskId
and NOT EXISTS(select Id from Logs where TaskId = t.Id and ProcessTypeId = @ProcessTypeId)
group by    t.PlanNum,
            t.Id,
            t.CreateDateTime,
            t.EndDateTime,
            g.LocationGuid,
            g.NumberDoc,
            g.Article,    
            g.DateDoc,  
            g.BarCode,
            g.Quantity,
            g.GoodBarcode",
            new { _query.TaskId, _query.PlanNum, _query.ProcessTypeId });

            return entity.ToList();
        }

        public List<Scaner_1cDocData> DocDataByPlanNum(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Scaner_1cDocData>($@"
select  dd.PlanNum,
        dd.NumberDoc,
        dd.DateDoc,
        dd.LocationGuid,
        dd.Article,
        dd.Quantity,
        dd.Barcode
from Scaner_1cDocData dd 
where dd.PlanNum = '{_query.PlanNum}'");

            return entity.ToList();
        }

        public void SetDataTo1c(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId, Response, Request) VALUES (@TaskId,@ProcessTypeId,@Message,@Request)",
            new { _query.TaskId, _query.ProcessTypeId, _query.Message, _query.Request });
        }
    }
}
