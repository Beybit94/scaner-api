using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Queries.Task
{
    public class TaskQuery: Query
    {
        public string PlanNum { get; set; }
        public int UserId { get; set; }
        public int DivisionId { get; set; }
        public int TaskId { get; set; }
        public int BoxId { get; set; }
        public string Path { get; set; }
    }
}
