using Business.Models.Base;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class TasksModel : Model
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

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<TasksModel>().Validate(this);
            return result;
        }
    }
}
