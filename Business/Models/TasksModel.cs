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
        public int UserId { get; set; }
        public int DivisionId { get; set; }
        public int StatusId { get; set; }
        public string PlanNum { get; set; }
        public string BarCode { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<TasksModel>().Validate(this);
            return result;
        }
    }

    public enum TaskStatusCode
    {
        Start,
        End,
        Deleted,
    }
}
