using Data.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class Differences : Entity
    {
        public string NumberDoc { get; set; }
        public int GoodId { get; set; }
        public string GoodArticle { get; set; }
        public string GoodName { get; set; }
        public int Quantity { get; set; }
        public int CountQty { get; set; }
        public int ExcessQty { get; set; }
        public int TaskId { get; set; }
        public string GoodGroupName { get; set; }
        public string Favorite { get; set; }
        public string Img { get; set; }
        public string Text1 { get; set; }
        public string UserName { get; set; }
        public string CreationDate { get; set; }
        public int Status { get; set; }
    }
}
