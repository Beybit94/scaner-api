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
    }
}
