using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Base
{
    public interface IModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Строкое представление идентификатора
        /// </summary>
        string StrID { get; set; }

        /// <summary>
        /// Валидация модели
        /// </summary>
        /// <returns>Результат валидации</returns>
        ModelValidationResult Validate();
    }
}
