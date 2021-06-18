using Data.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Logs")]
    public class Logs: Entity
    {
        public int TaskId { get; set; }
        public int ProcessTypeId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Ended { get; set; }

        public string ProcessName { get; set; }

        public Task Task { get; set; }
    }
}
