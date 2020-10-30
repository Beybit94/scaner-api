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

        public Goods GetGoodById(Query query)
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
WHERE Id = @Id", new { _query.Id });
            return entity;
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
and (BoxId = 0 or BoxId is null)
order by Id desc", new { @TaskId = _query.TaskId });
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
order by Id desc", new { @TaskId = _query.TaskId, @BoxId = _query.BoxId });
            return entity.ToList();
        }

        public List<Goods> GetGoodsByFilter(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods>(@"
SELECT  G.GOODID AS GOODID, 
        G.GOODARTICLE,
        G.GOODNAME,
        BC.BARCODE
FROM GOODS G 
JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.GOODID
JOIN  BARCODES BC (NOLOCK) ON BC.BARCODEID = GB.BARCODEID	
WHERE G.GOODARTICLE LIKE @GoodArticle
GROUP BY G.GOODID AS GOODID,G.GOODARTICLE, G.GOODNAME", new { @GoodArticle = "%"+_query.GoodArticle });
    return entity.ToList();
        }

        public int Save(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var Id = UnitOfWork.Session.QueryFirstOrDefault<int?>(@"
IF (ISNULL(@BoxId,0)=0)
BEGIN
    SELECT TOP 1 ID FROM SCANER_GOODS WHERE WMSTASKID= @WmsTaskId AND BARCODE = @GoodBarCode AND BOXID IS NULL
END
ELSE
BEGIN
    SELECT TOP 1 ID FROM SCANER_GOODS WHERE WMSTASKID= @WmsTaskId AND BARCODE = @GoodBarCode AND BOXID = @BoxId
END", new { @WmsTaskId = _query.TaskId, @BoxId = _query.BoxId, @GoodBarCode = _query.BarCode });

            return UnitOfWork.Session.QueryFirstOrDefault<int>(@"
IF NOT EXISTS (select cor._Fld44670 from [KAZ-1CBASE5].[ARENAS].[dbo].[_Document44667] cor where cor._Fld44670 = @GoodBarCode)
BEGIN
    IF (ISNULL(@Id,0) = 0)
    BEGIN
        INSERT INTO SCANER_GOODS (
            [WMSTASKID], 
            [BOXID], 
            [CONUMBER], 
            [GOODID], 
            [GOODARTICLE], 
            [ORDERQTY], 
            [COUNTQTY], 
            [EXCESSQTY], 
            [ORDERGUID], 
            [SAVED], 
            [SEND1C], 
            [IMG],
            [FAVORITE1], 
            [FAVORITE], 
            [PLANNUM], 
            [GOODNAME], 
            [BARCODE], 
            BOXBAR)
        SELECT  @WmsTaskId,
                @BoxId,
                '1' AS CONUMBER, 
                G.GOODID AS GOODID, 
                G.GOODARTICLE,
                1 AS ORDERQTY,
                1 AS COUNTQTY ,
                1 AS EXCESSQTY ,
                '1' AS ORDERGUID ,
                1 AS SAVED ,
                1 AS SEND1C  ,
                '1' AS IMG ,
                '1' AS FAVORITE1 ,
                1 AS FAVORITE ,
                @PlanNum, 
                G.GOODNAME, 
                @GoodBarCode, 
                '0'
	    FROM GOODS G 
	    JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.GOODID
	    JOIN  BARCODES BC (NOLOCK) ON BC.BARCODEID = GB.BARCODEID		
	    WHERE  BC.BARCODE = @GoodBarCode
        
        SELECT SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        DECLARE @Countup INT = (SELECT ISNULL(COUNTQTY,0) FROM SCANER_GOODS WHERE ID = @Id) + 1
        UPDATE SCANER_GOODS SET COUNTQTY = @Countup, BOXID = @BoxId WHERE ID = @Id

        SELECT @Id;
    END
END
ELSE
BEGIN
    INSERT INTO SCANER_GOODS (     
        [WMSTASKID],
        [CONUMBER],
        [GOODID],
        [GOODARTICLE],
        [ORDERQTY],
        [COUNTQTY],
        [EXCESSQTY],
        [ORDERGUID],
        [SAVED],
        [SEND1C],
        [IMG],
        [FAVORITE1],
        [FAVORITE],
        [PLANNUM],
        [GOODNAME],
        [BARCODE],
        BOXBAR)
    SELECT  @WmsTaskId,
            '1' AS CONUMBER,
            0 AS GOODID,
            0 AS GOODARTICLE,
            1 AS ORDERQTY,
            1 AS COUNTQTY,
            1 AS EXCESSQTY,
            '1' AS ORDERGUID,
            1 AS SAVED,
            1 AS SEND1C,
            '1' AS IMG,
            '1' AS FAVORITE1,
            1 AS FAVORITE,
            @PlanNum,
            ('КОРОБ' +' ' +  @GoodBarCode) AS GOODNAME,
            @GoodBarCode,
            COR._FLD44670
    FROM [KAZ-1CBASE5].[ARENAS].[DBO].[_DOCUMENT44667] COR
    WHERE COR._FLD44670 = @GoodBarCode
    
    SELECT SCOPE_IDENTITY();
END",
new
{
    @Id = Id,
    @WmsTaskId = _query.TaskId,
    @BoxId = _query.BoxId,
    @PlanNum = _query.PlanNum,
    @GoodBarCode = _query.BarCode,
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
delete from Scaner_Goods where Id = @Id", new { Id = _query.Id });
        }
    }
}
