using Data.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Tasks")]
    public class Tasks: Entity
    {
        public int UserId { get; set; }
        public int DivisionId { get; set; }
        public int StatusId { get; set; }
        public string PlanNum { get; set; }
        public string BarCode { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
    }
}
