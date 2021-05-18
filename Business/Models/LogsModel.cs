using Business.Models.Base;
using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class LogsModel : Model
    {
        public int TaskId { get; set; }
        public int ProcessTypeId { get; set; }       
        public string Response { get; set; }
        public string Request { get; set; }
        public DateTime Created { get; set; }
        public DateTime Ended { get; set; }

        public string PlanNum { get; set; }
        public string ProcessName { get; set; }

        public override ModelValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}
