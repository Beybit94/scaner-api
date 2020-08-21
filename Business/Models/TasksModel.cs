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
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public int UserId { get; set; }
        public int TaskTypeId { get; set; }
        public string PlanNum { get; set; }
        public string BoxNum { get; set; }
        public string PlanNumName
        {
            get
            {
                return string.IsNullOrEmpty(BoxNum) ? PlanNum : string.Concat(PlanNum, "-К№", BoxNum);
            }
        }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<TasksModel>().Validate(this);
            return result;
        }
    }
}
