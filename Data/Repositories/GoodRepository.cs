using Dapper;
using Data.Model;
using Data.Queries.Good;
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
       CountQty,
       GoodName,
       GoodArticle,
       BarCode,
       DamagePercentId 
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
       CountQty,
       GoodName,
       GoodArticle,
       BarCode,
       DamagePercentId 
FROM Scaner_Goods
WHERE TaskId = @TaskId 
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
       CountQty,
       GoodName,
       GoodArticle,
       BarCode,
       DamagePercentId 
FROM Scaner_Goods
WHERE TaskId = @TaskId 
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
       CountQty,
       GoodName,
       GoodArticle,
       BarCode,
       DamagePercentId 
FROM Scaner_Goods
WHERE TaskId = @TaskId 
and BoxId = @BoxId
order by Id desc", new { @TaskId = _query.TaskId, @BoxId = _query.BoxId });
            return entity.ToList();
        }

        public int Save(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var Id = UnitOfWork.Session.QueryFirstOrDefault<int?>(@"
DECLARE @ID INT = (SELECT G.GOODID
                    FROM GOODS G 
                    JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.GOODID
                    JOIN  BARCODES BC (NOLOCK) ON BC.BARCODEID = GB.BARCODEID		
                    WHERE  BC.BARCODE = @BarCode)

IF (ISNULL(@BoxId,0)=0)
BEGIN
    SELECT TOP 1 ID FROM SCANER_GOODS WHERE TaskId= @TaskId AND GoodId = @ID AND BOXID IS NULL
END
ELSE
BEGIN
    SELECT TOP 1 ID FROM SCANER_GOODS WHERE TaskId= @TaskId AND GoodId = @ID AND BOXID = @BoxId
END", new { @TaskId = _query.TaskId, @BoxId = _query.BoxId, @BarCode = _query.BarCode });

            return UnitOfWork.Session.QueryFirstOrDefault<int>(@"
IF NOT EXISTS (select Id from Boxes where BarCode = @BarCode)
BEGIN
    IF (ISNULL(@Id,0) = 0)
    BEGIN
        INSERT INTO SCANER_GOODS (
            TaskId, 
            BoxId,
            GoodId, 
            GoodArticleL, 
            GoodName, 
            CountQty, 
            BarCode )
        SELECT  @TaskId,
                @BoxId,
                G.GOODID, 
                G.GOODARTICLE,
                G.GOODNAME, 
                1 AS COUNTQTY ,
                1 AS EXCESSQTY ,
                @BarCode
        FROM GOODS G 
	    JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.GOODID
	    JOIN  BARCODES BC (NOLOCK) ON BC.BARCODEID = GB.BARCODEID		
	    WHERE  BC.BARCODE = @BarCode
        
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
            TaskId, 
            GoodId, 
            GoodArticleL, 
            GoodName, 
            CountQty, 
            BarCode )
    SELECT  @TaskId,
            0 AS GOODID,
            '' AS GOODARTICLE,         
            ('КОРОБ' +' ' +  @BarCode) AS GOODNAME,
            1 AS COUNTQTY,
            @BarCode
    FROM Boxes b
    WHERE b.BarCode = @BarCode
    
    SELECT SCOPE_IDENTITY();
END",
new
{
    @Id = Id,
    @TaskId = _query.TaskId,
    @BoxId = _query.BoxId,
    @PlanNum = _query.PlanNum,
    @BarCode = _query.BarCode,
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
