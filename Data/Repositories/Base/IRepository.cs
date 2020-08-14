using Data.Model.Base;
using ScanerApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Base
{
    /// <summary>
    /// Базовый интерфейс репозитория
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Старт транзакции
        /// </summary>
        /// <returns>Транзакция</returns>
        IDbTransaction BeginTransaction();
    }

    /// <summary>
    /// Базовый интерфейс репозитория
    /// </summary>
    public interface IRepository<T> : IRepository where T : IEntity
    {
        /// <summary>
        /// Вставка
        /// </summary>
        /// <param name="entity">Сущность</param>
        void Insert(T entity);
        /// <summary>
        /// Обновление
        /// </summary>
        /// <param name="entity">Сущность</param>
        void Update(T entity);
        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="id">Идентификатор</param>
        void Delete(long id);
        /// <summary>
        /// Получение сущности
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Сущность</returns>
        T Get(long id);
        /// <summary>
        /// Поиск сущности
        /// </summary>
        /// <param name="query">Критерии запроса</param>
        /// <returns>Сущность</returns>
        T Find(Query query);
        /// <summary>
        /// Получение списка записей
        /// </summary>
        /// <param name="listQuery">Критерии запроса</param>
        /// <returns>Список записей</returns>
        List<T> List(ListQuery listQuery);
        /// <summary>
        /// Получение количества записей
        /// </summary>
        /// <param name="listQuery">Критерии запроса</param>
        /// <returns>Количество записей</returns>
        int Count(ListQuery listQuery);
        /// <summary>
        /// Получение списка записей заданной модели
        /// </summary>
        /// <typeparam name="TM">Тип модели</typeparam>        
        /// <param name="listQuery">Критерии</param>
        /// <returns></returns>
        List<TM> List<TM>(ListQuery listQuery);
    }
}
