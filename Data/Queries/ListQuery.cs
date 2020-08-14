using System;
using System.Collections.Generic;
using System.Text;

namespace ScanerApi.Data.Queries
{
    /// <summary>
    /// Класс для построения запрос к базе по критерям для списка
    /// </summary>
    public class ListQuery
    {
        /// <summary>
        /// Кол-во пропускаемых строк
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Кол-во строк
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Наименование поля сортировки
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Направление сортировки
        /// </summary>
        public SortDirection Direction { get; set; } = SortDirection.Asc;
    }
}
