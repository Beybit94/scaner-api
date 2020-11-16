using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Queries.Good
{
    public class GoodQuery:Query
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int? BoxId { get; set; }
        public string BarCode { get; set; }
        public string PlanNum { get; set; }
        public int GoodId { get; set; }
        public string GoodName { get; set; }
        public string GoodArticle { get; set; }
        public int CountQty { get; set; }
    }
}
