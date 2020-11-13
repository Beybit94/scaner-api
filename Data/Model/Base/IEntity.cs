using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.Base
{
    /// <summary>
    /// Базовый интерфейс сущности базы данных
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        int Id { get; set; }
    }
}
