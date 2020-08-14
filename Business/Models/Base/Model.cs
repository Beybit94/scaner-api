using Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Base
{
    /// <summary>
    /// Базовый класс модели
    /// </summary>
    public abstract class Model : IModel
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Строковое представление идентификатора
        /// </summary>
        public string StrID
        {
            get { return ID.ToString(); }
            set { ID = string.IsNullOrEmpty(value) ? 0 : Convert.ToInt64(value); }
        }

        /// <summary>
        /// Валидация модели
        /// </summary>
        /// <returns>Результаты валидации</returns>
        public abstract ModelValidationResult Validate();
    }
}
