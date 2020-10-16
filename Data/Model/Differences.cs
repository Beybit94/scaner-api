using Data.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Scaner_1cDocData")]
    public class Differences : Entity
    {
        public string NumberDoc { get; set; }
        public int GoodId { get; set; }
        public string GoodArticle { get; set; }
        public string GoodName { get; set; }
        public int Quantity { get; set; }
        public int CountQty { get; set; }
        public int ExcessQty { get; set; }
    }
}
