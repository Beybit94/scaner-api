using Data.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Scaner_Goods")]
    public class Goods: Entity
    {
        public int GoodId { get; set; }
        public int Count { get; set; }
        public string GoodName { get; set; }
        public string GoodArticle { get; set; }
        public string BarCode { get; set; }
        public int? BoxId { get; set; }
    }
}
