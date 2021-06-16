using ScanerApi.Business.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryModels.Logs
{
    public class LogsListQueryModel : ListQueryModel
    {
        public int TaskId { get; set; }
    }
}
