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
and (BoxId = 0 or BoxId is null)
order by Created desc", new { @TaskId = _query.TaskId });
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
and BoxId = @BoxId
order by Created desc", new { @TaskId = _query.TaskId, @BoxId = _query.BoxId });
            return entity.ToList();
        }

        public List<Goods> GetGoodsByFilter(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods>(@"
SELECT G.GOODID AS GOODID, G.GOODARTICLE, G.GOODNAME
FROM (
    SELECT  G.GOODID, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 	
    WHERE G.GOODARTICLE LIKE @GoodArticle
    UNION
    SELECT G.GOODID, G.GOODARTICLE,G.GOODNAME
    FROM GOODS G 	
    WHERE G.GoodName LIKE @GoodArticle) G
GROUP BY G.GOODID,G.GOODARTICLE, G.GOODNAME", new { @GoodArticle = "%" + _query.GoodArticle + "%" });
            return entity.ToList();
        }

        public List<Goods> ExistGood(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods>($@"
IF TRIM(@GoodArticle) = ''
BEGIN
    INSERT INTO Scanner_Log (Method,Params) VALUES ('ExistGood', 'Barcode:{_query.BarCode}')

    SELECT  G.GOODID, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 
    JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.GOODID
    JOIN  BARCODES BC (NOLOCK) ON BC.BARCODEID = GB.BARCODEID		
    WHERE BC.BARCODE = @BarCode
    UNION
    SELECT  0 as GOODID, '' as GOODARTICLE, 'Короб '+ COR._FLD44670 as GOODNAME
    FROM [KAZ-1CBASE5].[ARENAS].[DBO].[_DOCUMENT44667] COR
    WHERE COR._FLD44670 = @BarCode
END
ELSE
BEGIN
    INSERT INTO Scanner_Log (Method,Params) VALUES ('ExistGood', 'GoodArticle:{_query.GoodArticle}')

    SELECT  G.GOODID, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 
    WHERE G.GoodArticle= @GoodArticle
END", new { _query.BarCode, _query.GoodArticle });
            return entity.ToList();
        }

        public void SaveGood(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute($@"
INSERT  INTO Scanner_Log (Method,Params) 
        VALUES ('SaveGood', 'TaskId:{_query.TaskId},GoodArticle:{_query.GoodArticle},BarCode:{_query.BarCode},BoxId:{_query.BoxId}')

IF TRIM(@GoodArticle) = ''
BEGIN
    MERGE Scaner_Goods AS Target
    USING ( SELECT  @TaskId as TaskId, 
                    @PlanNum as PlanNum,
                    @BarCode as BarCode,
                    ISNULL(@BoxId,0) as BoxId,
                    @CountQty as CountQty, 
                    @GoodName as GoodName
            FROM [KAZ-1CBASE5].[ARENAS].[DBO].[_DOCUMENT44667] COR
            WHERE COR._FLD44670 = @BarCode) AS Source
    ON (Target.WmsTaskId = Source.TaskId 
        AND Target.BarCode = Source.BarCode)
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (WMSTASKID, PlanNum, BoxId, GoodName, CountQty, BarCode)
        VALUES (Source.TaskId, Source.PlanNum, Source.BoxId, Source.GoodName, Source.CountQty, Source.BarCode);
END
ELSE
BEGIN
    MERGE Scaner_Goods AS Target
    USING ( SELECT  @TaskId as TaskId, 
                    @PlanNum as PlanNum,
                    @BarCode as BarCode,
                    ISNULL(@BoxId,0) as BoxId,
                    @CountQty as CountQty,
                    G.GOODID, 
                    G.GOODNAME,
                    G.GOODARTICLE
            FROM GOODS G 
            WHERE G.GoodArticle= @GoodArticle) AS Source
    ON (Target.WmsTaskId = Source.TaskId 
        AND Target.GoodArticle = Source.GoodArticle 
        AND ISNULL(Target.BoxId,0) = Source.BoxId)
    WHEN MATCHED THEN
         UPDATE SET Target.CountQty = (Target.CountQty+1), Created = GETDATE()
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (WMSTASKID, PlanNum, BoxId, GoodId, GoodArticle, GoodName, CountQty, BarCode)
        VALUES (Source.TaskId, Source.PlanNum, Source.BoxId, Source.GoodId, Source.GoodArticle, Source.GoodName, Source.CountQty, Source.BarCode);
END",
            new
            {
                _query.TaskId,
                _query.PlanNum,
                _query.GoodId,
                _query.GoodName,
                _query.GoodArticle,
                _query.CountQty,
                _query.BarCode,
                _query.BoxId,
            });
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
delete from Scaner_Goods where BoxId = @Id
delete from Scaner_Goods where Id = @Id", new { _query.Id });
        }
    }
}
