using Business.Validation;
using ScanerApi.Business.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryModels.Task
{
    public class TaskQueryModel : QueryModel
    {
        public string PlanNum { get; set; }
        public int UserId { get; set; }
        public int DivisionId { get; set; }
        public int TaskId { get; set; }
        public int BoxId { get; set; }
        public string Path { get; set; }
        public int StatusId { get; set; }
        public int TypeId { get; set; }

        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<TaskQueryModel>().Validate(this);
            return result;
        }
    }
}
