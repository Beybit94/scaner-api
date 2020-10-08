using Dapper;
using Data.Model.Base;
using Data.Access;
using Data.Extensions;
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
    /// Базовый класс репозитори
    /// </summary>
    /// <typeparam name="T">Сущность БД IEntity</typeparam>
    public abstract class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected IUnitOfWork UnitOfWork { get; }

        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IDbTransaction BeginTransaction()
        {
            return UnitOfWork.BeginTransaction();
        }

        public virtual void Insert(T entity)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                var tableName = typeof(T).TableName();
                var props = entity.GetType().GetProperties();
                var columns = props.Where(p => !p.NotMapped()).Select(p => p.Name).Where(s => s != "ID").ToArray();

                if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

                entity.ID = UnitOfWork.Session.QuerySingleOrDefault<long>($@"
INSERT INTO {tableName} ({string.Join(",", columns)})
VALUES (@{string.Join(",@", columns)})
SELECT SCOPE_IDENTITY()", entity, UnitOfWork.Transaction);

                transaction.Commit();
            }
        }

        public virtual void Update(T entity)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                var tableName = typeof(T).TableName();
                var props = entity.GetType().GetProperties();
                var columns = props.Where(p => !p.NotMapped()).Select(p => p.Name).Where(s => s != "ID").ToArray();

                if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

                UnitOfWork.Session.Execute($@"
UPDATE {tableName} WITH(ROWLOCK)
SET {string.Join(",", columns.Select(name => name + "=@" + name))}
WHERE ID = @ID", entity, UnitOfWork.Transaction);

                transaction.Commit();
            }
        }

        public virtual void Delete(long id)
        {
            using (var transaction = UnitOfWork.BeginTransaction())
            {
                var tableName = typeof(T).TableName();

                if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

                UnitOfWork.Session.Execute($"DELETE FROM {tableName} WHERE ID = @id", new { id }, UnitOfWork.Transaction);

                transaction.Commit();
            }
        }

        public virtual T Get(long id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var tableName = typeof(T).TableName();

            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

            using (var transaction = UnitOfWork.BeginTransaction())
            {
                return UnitOfWork.Session.QuerySingleOrDefault<T>($"SELECT * FROM {tableName} WHERE ID = @id", new { id }, UnitOfWork.Transaction);
            }
        }

        public virtual T Find(Query query)
        {
            throw new NotImplementedException();
        }

        public virtual List<T> List(ListQuery listQuery)
        {
            if (listQuery == null) throw new ArgumentNullException(nameof(listQuery));

            var tableName = typeof(T).TableName();

            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

            var order = "ORDER BY ID DESC";

            var page = listQuery.Page();

            return UnitOfWork.Session.Query<T>($@"SELECT * FROM {tableName} {order} {page}", listQuery).ToList();
        }

        public virtual int Count(ListQuery listQuery)
        {
            if (listQuery == null) throw new ArgumentNullException(nameof(listQuery));

            var tableName = typeof(T).TableName();

            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

            return UnitOfWork.Session.ExecuteScalar<int>($@"SELECT COUNT(*) FROM {tableName}");
        }

        public virtual List<TM> List<TM>(ListQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
