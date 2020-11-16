﻿using Dapper;
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

        public void SaveDataFrom1c(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as Data1cQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute(@"
insert into Scaner_Goods ([PlanNum], [DateDoc], [GUID_PlanWMSNumber],[NumberDoc],[TypeDoc],[Article],[Barcode],[Quantity],[UserId])
values ( isnull(@PlanNum,0),
         isnull(@DateDoc, '08.08.2020 0:00:00'),
         isnull(@Planguid,'0'),
         isnull(@NumberDoc,0),
         isnull(@TypeDoc,0),
         isnull(@Article,0),
         isnull(@Barcode,0),
         isnull(@Quantity,0),
         isnull(@userid,0))",
         new
         {
             @PlanNum = _query.PlanNum,
             @userid = _query.UserId,
             @DateDoc = _query?.DateDoc,
             @Planguid = _query.Planguid,
             @NumberDoc = _query.NumberDoc,
             @TypeDoc = _query.TypeDoc,
             @Article = _query.Article,
             @Barcode = _query.Barcode,
             @Quantity = _query.Quantity
         });
        }
    }
}
