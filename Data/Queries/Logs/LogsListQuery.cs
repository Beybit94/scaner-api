using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Queries.Logs
{
    public class LogsListQuery:ListQuery
    {
        public int TaskId { get; set; }
    }
}
