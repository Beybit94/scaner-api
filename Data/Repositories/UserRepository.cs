using Dapper;
using Data.Model;
using Data.Queries.Users;
using Data.Repositories.Base;
using Data.Access;
using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserRepository : Repository<Users>
    {
        public UserRepository(IWebProjectUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override Users Find(Query query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var _query = query as UsersQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            var entity = UnitOfWork.Session.QueryFirst<Users>($@"
SELECT u.UserId, 
       u.UserFirstName, 
       u.UserSecondName, 
       u.UserDivisionId, 
       CAST(u.UserGuid AS NVARCHAR(50)) as UserGuid, 
       u.UserMiddleName 
FROM Users u WHERE u.UserName = @Login and u.UserPassword = @Password", new { _query.Login, _query.Password });
            
             return entity;
        }

        public int? GetGoodId(string barcode)
        {
            return UnitOfWork.Session.QueryFirstOrDefault<int?>(@"
SELECT G.GOODID
FROM GOODS G 
JOIN GOODSBARCODES GB (NOLOCK) ON GB.GOODID = G.GOODID
JOIN  BARCODES BC (NOLOCK) ON BC.BARCODEID = GB.BARCODEID		
WHERE  BC.BARCODE = @GoodBarCode");
        }

        public int GetPlanNum(string PlanNum)
        {
            return UnitOfWork.Session.QueryFirstOrDefault<int>($@"
SELECT count(pm.PlanNum)
FROM ROT1c1 pm (nolock)
join Inventory_Taskss it (nolock) on it.ROT = pm._Number
WHERE pm.[PlanNum] = @PlanNum ", new { PlanNum });
        }
    }
}
