using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Queries.Task
{
    public class TaskQuery : Query
    {
        public string PlanNum { get; set; }
        public int UserId { get; set; }
        public int DivisionId { get; set; }
        public int TaskId { get; set; }
        public int GoodId { get; set; }
        public string Path { get; set; }
        public int StatusId { get; set; }
        public DateTime? EndDateTime { get; set; }
        
        public int Start { get; set; }
        public int End { get; set; }
        public int InProcess { get; set; }
       
        public int Deleted { get; set; }
        public int ProcessTypeId { get; set; }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
