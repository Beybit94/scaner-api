using Dapper;
using Data.Access;
using Data.Model;
using Data.Queries.Logs;
using Data.Repositories.Base;
using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class LogRepository : Repository<Logs>
    {
        public LogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override List<Logs> List(ListQuery listQuery)
        {
            if (listQuery == null) throw new ArgumentNullException(nameof(listQuery));

            var _query = listQuery as LogsListQuery;
            if (_query == null) throw new InvalidCastException(nameof(_query));

            return UnitOfWork.Session.Query<Logs, Task, Logs>(@"
select hp.Name as ProcessName, 
       l.Response as Response,
       l.Request as Request,
       l.Created as Created,
       t.*
from Logs l
join hProcessType hp on hp.Id = l.ProcessTypeId
left join Tasks t on t.Id = l.TaskId
where l.TaskId = @TaskId
order by l.Created", (l, t) =>
            {
                l.Task = t;
                return l;
            }, new { _query.TaskId }).ToList();
        }
    }
}
