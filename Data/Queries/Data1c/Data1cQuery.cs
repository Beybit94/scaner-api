using Data.Model;
using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Queries.Data1c
{
    public class Data1cQuery : Query
    {
        public string PlanNum { get; set; }
        public int TaskId { get; set; }

        public int StatusId { get; set; }
        public int ProcessTypeId { get; set; }
        public string Message { get; set; }
        public string Request { get; set; }

        public List<Scaner_1cDocData> docDatas = new List<Scaner_1cDocData>();
    }
}
