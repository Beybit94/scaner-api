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
        public int InventoryType { get; set; }
        public int DivisionId { get; set; }
        public int UserId { get; set; }
        public int ControlId { get; set; }
        public string TaskId { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public bool IsEmpty { get; set; }
        public bool HandEnter { get; set; }
        public int TaskType { get; set; }
    }
}
