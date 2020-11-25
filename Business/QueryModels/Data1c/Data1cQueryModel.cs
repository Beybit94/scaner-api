using Business.Models;
using Business.Validation;
using ScanerApi.Business.QueryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.QueryModels.Data1c
{
    public class Data1cQueryModel : QueryModel
    {
        public int UserId { get; set; }
        public string PlanNum { get; set; }
        public int TaskId { get; set; }
        public int StatusId { get; set; }
        public int ProcessTypeId { get; set; }
        public string NumberDoc { get; set; }
        public string Message { get; set; }

        public List<Scaner_1cDocDataModel> docDatas = new List<Scaner_1cDocDataModel>();
        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<Data1cQueryModel>().Validate(this);
            return result;
        }
    }
}
