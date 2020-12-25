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
order by Created desc", new { _query.TaskId });
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
order by Created desc", new { _query.TaskId, _query.BoxId });
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
    SELECT  G.ID as GOODID, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 	
    WHERE G.GOODARTICLE LIKE @GoodArticle
    UNION
    SELECT G.ID as GOODID, G.GOODARTICLE,G.GOODNAME
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
IF RTRIM(LTRIM(@GoodArticle)) = ''
BEGIN
    SELECT TOP(1) G.Id, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 
    JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.Id
    JOIN  BARCODES BC (NOLOCK) ON BC.Id = GB.BARCODEID		
    WHERE BC.BARCODE = @BarCode
    UNION
    SELECT  0 as Id, '' as GOODARTICLE, 'Короб '+ b.BARCODE as GOODNAME
    FROM BOXES b
    WHERE b.BARCODE = @BarCode
END
ELSE
BEGIN
    SELECT  G.Id, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 
    WHERE G.GoodArticle= @GoodArticle
END", new { _query.BarCode, _query.GoodArticle });
            return entity.ToList();
        }

        public void Save(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            UnitOfWork.Session.Execute($@"
IF RTRIM(LTRIM(@GoodArticle)) = ''
BEGIN
    MERGE Scaner_Goods AS Target
    USING ( SELECT  @TaskId as TaskId, 
                    @BarCode as BarCode,
                    ISNULL(@BoxId,0) as BoxId,
                    @CountQty as CountQty, 
                    0 as GOODID, 
                    '' as GOODARTICLE,
                    @GoodName as GoodName
            FROM BOXES B
            WHERE B.BarCode = @BarCode) AS Source
    ON (Target.TaskId = Source.TaskId 
        AND Target.BarCode = Source.BarCode)
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (TaskId, BoxId, GoodId, GoodArticle, GoodName, CountQty, BarCode)
        VALUES (Source.TaskId, Source.BoxId, Source.GoodId, Source.GoodArticle, Source.GoodName, Source.CountQty, Source.BarCode);
END
ELSE
BEGIN
    MERGE Scaner_Goods AS Target
    USING ( SELECT  @TaskId as TaskId, 
                    @BarCode as BarCode,
                    ISNULL(@BoxId,0) as BoxId,
                    @CountQty as CountQty,
                    G.Id as GOODID, 
                    G.GOODNAME,
                    G.GOODARTICLE
            FROM GOODS G 
            WHERE G.GoodArticle= @GoodArticle) AS Source
    ON (Target.TaskId = Source.TaskId 
        AND Target.GoodArticle = Source.GoodArticle 
        AND ISNULL(Target.BoxId,0) = Source.BoxId)
    WHEN MATCHED THEN
         UPDATE SET Target.CountQty = (Target.CountQty+1), Created = GETDATE()
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (TaskId, BoxId, GoodId, GoodArticle, GoodName, CountQty, BarCode)
        VALUES (Source.TaskId, Source.BoxId, Source.GoodId, Source.GoodArticle, Source.GoodName, Source.CountQty, Source.BarCode);
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
update Scaner_Goods set CountQty = @CountQty where Id = @Id", new { _query.CountQty, _query.Id });
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

        public void SaveDefect(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            if(_query.DamagePercentId == 0)
            {
                UnitOfWork.Session.Execute(@"
delete from Scaner_Goods where BoxId = @Id
update Scaner_Goods SET DamagePercentId = NULL where Id = @Id", new { _query.Id, _query.TaskId });
            }
            else
            {
                UnitOfWork.Session.Execute(@"
update Scaner_Goods SET DamagePercentId = @DamagePercentId where Id = @Id", new { _query.DamagePercentId, _query.Id });
            }
           
        }
    }
}
