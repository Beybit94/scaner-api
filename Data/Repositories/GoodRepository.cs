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

        public List<Goods> GetGoodWithBarcode(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods>($@"
select g.GoodName, g.GoodArticle, b.BarCode
from Goods g
left join GoodsBarcodes gb on gb.GoodId = g.Id
left join BarCodes b on b.Id = gb.BarcodeId
group by g.GoodName, g.GoodArticle, b.BarCode");
            return entity.ToList();
        }

        public List<Goods> GetGoods(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.Query<Goods, Defects, Goods>($@"
SELECT g.*, d.*
FROM Scaner_Goods g
LEFT JOIN Defects d on d.Id = g.DefectId
WHERE TaskId = @TaskId
ORDER BY g.Created", (g, d) =>
            {
                g.Defect = d;
                return g;
            },
            new { _query.TaskId, _query.PlanNum });
            return entity.ToList();
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
       DefectId 
FROM Scaner_Goods
WHERE TaskId = @TaskId 
order by Created desc", new { _query.TaskId });
            return entity.ToList();
        }

        public List<Goods> GetBoxesByTask(Query query)
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
       DefectId 
FROM Scaner_Goods
WHERE TaskId = @TaskId 
and GoodId = 0
and DefectId is null
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
       DefectId 
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

        public Goods GetGoodsByArticle(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            return UnitOfWork.Session.QueryFirstOrDefault<Goods>(@"
SELECT  G.ID as GoodId, G.GoodArticle, G.GoodName, 0 as CountQty,'' as BarCode
FROM GOODS G 	
WHERE G.GOODARTICLE = @GoodArticle", new { _query.GoodArticle });
        }

        public List<Goods> ExistGood(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var transaction = session.BeginTransaction();
                try
                {
                    session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'SearchGood'),
        'ШК: '+@BarCode+' , Артикуль: '+@GoodArticle)", new { _query.BarCode, _query.GoodArticle, _query.TaskId });

                    var entity = session.Query<Goods>($@"
IF RTRIM(LTRIM(@GoodArticle)) = ''
BEGIN
    SELECT TOP(1) G.Id, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 
    JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.Id
    JOIN  BARCODES BC (NOLOCK) ON BC.Id = GB.BARCODEID		
    WHERE BC.BARCODE = @BarCode
    UNION
    SELECT TOP(1) 0 as Id, '' as GOODARTICLE, 'Короб '+ b.BARCODE as GOODNAME
    FROM Scaner_1cDocData  b
    WHERE b.BARCODE = @BarCode
END
ELSE
BEGIN
    SELECT  G.Id, G.GOODARTICLE, G.GOODNAME
    FROM GOODS G 
    WHERE G.GoodArticle= @GoodArticle
END", new { _query.BarCode, _query.GoodArticle, _query.ProcessType, _query.TaskId }, transaction);

                    if (!entity.Any())
                    {
                        session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'NotFound'),
        'ШК: '+@BarCode+' , Артикуль: '+@GoodArticle)", new { _query.BarCode, _query.GoodArticle, _query.TaskId }, transaction);
                    }

                    transaction.Commit();
                    return entity.ToList();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return new List<Goods>();
                }
            }

        }

        public void Save(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var transaction = session.BeginTransaction();
                try
                {
                    session.Execute($@"
IF RTRIM(LTRIM(@GoodArticle)) = ''
BEGIN
    MERGE Scaner_Goods AS Target
    USING ( SELECT  @TaskId as TaskId, 
                    @BarCode as BarCode,
                    ISNULL(@BoxId,0) as BoxId,
                    @CountQty as CountQty, 
                    0 as GOODID, 
                    '' as GOODARTICLE,
                    @GoodName as GoodName) AS Source
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
              }, transaction);

                    if (_query.GoodId != 0)
                    {
                        session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'UpdateGood'),
        'Артикуль:'+@GoodArticle+', ШК:'+ @BarCode +', Было:'+ (@CountQty-1) + ', Стало:'+ @CountQty)", new { _query.TaskId, _query.GoodArticle, _query.BarCode, _query.CountQty }, transaction);
                    }
                    else
                    {
                        session.Execute(@"
INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'CreateGood'),
        'Артикуль:'+@GoodArticle+', ШК:'+@BarCode);", new { _query.TaskId, _query.GoodArticle, _query.CountQty, _query.BarCode }, transaction);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public void Update(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var transaction = session.BeginTransaction();
                try
                {
                    session.Execute(@"update Scaner_Goods set CountQty = @CountQty where Id = @Id", new { _query.CountQty, _query.Id }, transaction);
                    session.Execute(@"
DECLARE @TaskId int,
@Barcode nvarchar(50), 
@Article nvarchar(150),
@CountQtyOld int ;

SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode, @CountQtyOld = CountQty 
from Scaner_Goods where Id=@Id;

INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'UpdateGood'),
        'Артикуль:'+@Article+', ШК:'+ @BarCode)", new { _query.CountQty, _query.Id }, transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }

        }

        public void Delete(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var transaction = session.BeginTransaction();
                try
                {
                    session.Execute(@"
delete from Scaner_Goods where BoxId = @Id
delete from Scaner_Goods where Id = @Id", new { _query.Id }, transaction);

                    session.Execute(@"
DECLARE @TaskId int,
@Barcode nvarchar(50), 
@Article nvarchar(50);

SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode 
from Scaner_Goods where Id=@Id;

INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'DeleteGood'),
        'Артикуль:'+@Article+', ШК:'+ @BarCode)", new { _query.Id, _query.BarCode }, transaction);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

            }
        }

        public void SaveDefect(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as GoodQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            using (var session = UnitOfWork.GetConnection())
            {
                var transaction = session.BeginTransaction();
                try
                {
                    if (_query.DefectId == 0)
                    {

                        var DefectId = session.Query<int>(@"
INSERT INTO Defects (Damage, SerialNumber, Description)
VALUES (@Damage, @SerialNumber, @Description); 
SELECT SCOPE_IDENTITY()", new { _query.Damage, _query.SerialNumber, _query.Description }, transaction);

                        session.Execute(@"
update Scaner_Goods SET DefectId = @DefectId, Created = getdate() where Id = @Id", new { DefectId, _query.Id }, transaction);
                        session.Execute(@"
DECLARE @TaskId int,
@Barcode nvarchar(50), 
@Article nvarchar(50);

SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode 
from Scaner_Goods where Id=@Id;

INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Defect'),
        'Артикуль:'+@Article+', ШК:'+ @BarCode)", new { _query.Id, _query.BarCode }, transaction);
                    }
                    else
                    {
                        session.Execute(@"
delete from Scaner_Goods where BoxId = @Id
update Scaner_Goods SET DefectId = NULL where Id = @Id
delete from Defects Where Id = @DefectId", new { _query.Id, _query.DefectId }, transaction);
                        session.Execute(@"
DECLARE @TaskId int,
@Barcode nvarchar(50), 
@Article nvarchar(50);

SELECT @TaskId = TaskId, @Article = GoodArticle, @Barcode = BarCode 
from Scaner_Goods where Id=@Id;

INSERT INTO Logs (TaskId, ProcessTypeId, Description) 
VALUES (@TaskId, 
        (SELECT TOP 1 Id FROM hProcessType WHERE Code = 'Undefect'),
        'Артикуль:'+@Article+', ШК:'+ @BarCode)", new { _query.Id, _query.BarCode }, transaction);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
