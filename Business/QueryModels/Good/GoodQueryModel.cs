using Business.Validation;
using ScanerApi.Business.QueryModels;

namespace Business.QueryModels.Good
{
    public class GoodQueryModel : QueryModel
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int BoxId { get; set; }
        public string BarCode { get; set; }
        public string PlanNum { get; set; }
        public int GoodId { get; set; }
        public string GoodName { get; set; }
        public string GoodArticle { get; set; }
        public int CountQty { get; set; }
        public int DamagePercentId { get; set; }
        public string Path { get; set; }
        public override ModelValidationResult Validate()
        {
            var result = new ModelValidator<GoodQueryModel>().Validate(this);
            return result;
        }
    }
}
