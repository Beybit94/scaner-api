using Data.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    [Table("Scaner_File")]
    public class ScanerFile:Entity
    {
        public int TaskId { get; set; }
        public int? GoodId { get; set; }
        public string Path { get; set; }
        public int TypeId { get; set; }
    }
}
