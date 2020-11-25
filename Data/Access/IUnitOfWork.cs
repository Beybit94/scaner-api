using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Access
{
    /// <summary>
    /// Единица работы
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Сессия подключения
        /// </summary>
        IDbConnection Session { get; }

        /// <summary>
        /// Текущая транзакция
        /// </summary>
        IDbTransaction Transaction { get; }

        /// <summary>
        /// Старт транзакции
        /// </summary>
        /// <param name="isolationLevel">Уровень изоляции транзакции</param>
        /// <returns>Транзакция</returns>
        IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable);
    }
}
