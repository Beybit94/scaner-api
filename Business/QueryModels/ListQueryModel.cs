using Business.Validation;
using System.ComponentModel.DataAnnotations;

namespace ScanerApi.Business.QueryModels
{
    /// <summary>
    /// Базовый класс для запроса списков
    /// </summary>
    public class ListQueryModel : QueryModel
    {
        public ListQueryModel()
        {

        }

        /// <summary>
        /// Количество пропускаемых записей
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Количество получаемых записей
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Limit { get; set; }

        public override ModelValidationResult Validate()
        {
            return new ModelValidator<ListQueryModel>().Validate(this);
        }
    }

}