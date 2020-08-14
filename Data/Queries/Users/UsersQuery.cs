using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Queries.Users
{
    public class UsersQuery:Query
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
