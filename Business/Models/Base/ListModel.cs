using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models.Base
{
    /// <summary>
    /// Базовый класс возврата постраничных списков
    /// </summary>
    /// <typeparam name="T">Тип возвращаемых данных</typeparam>
    public class ListModel<T>
    {
        /// <summary>
        /// Данные
        /// </summary>
        public List<T> Data { get; set; }

        /// <summary>
        /// Общее количество записей
        /// </summary>
        public int Total { get; set; }
    }
}
