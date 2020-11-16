using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.Base
{
    /// <summary>
    /// Базовый класс сущности базы данных
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
    }
}
