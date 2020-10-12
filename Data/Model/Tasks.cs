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
        public string Id { get; set; }
        public int DivisionId { get; set; }
        public int UserId { get; set; }
        public int TaskTypeId { get; set; }
        public string PlanNum { get; set; }
        public string BoxNum { get; set; }
    }
}
