using Business.Validation;

namespace ScanerApi.Business.QueryModels
{
    public abstract class QueryModel
    {
        /// <summary>
        /// Валидация модели
        /// </summary>
        /// <returns>Результаты валидации</returns>
        public abstract ModelValidationResult Validate();
    }
}
