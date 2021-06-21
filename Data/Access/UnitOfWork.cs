using Data.Transaction;
using System.Data;
using System.Data.SqlClient;

namespace Data.Access
{
    public class UnitOfWork : IUnitOfWork
    {
        private TransactionWrapper _transaction;
        private string _connectString;
        public IDbConnection Session { get; private set; }
        public IDbTransaction Transaction => _transaction?.InternalTransaction;

        public UnitOfWork(string connectionString)
        {
            Session = new SqlConnection(connectionString);
            _connectString = connectionString;
        }

        public void Init()
        {
            Session.Open();
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable)
        {
            if (_transaction == null)
            {
                _transaction = new TransactionWrapper(Session.BeginTransaction(isolationLevel));
                _transaction.Disposing += OnTransactionDisposing;

                return _transaction;
            }

            return new TransactionStub(Session, isolationLevel);
        }

        private void OnTransactionDisposing(TransactionWrapper sender)
        {
            sender.Disposing -= OnTransactionDisposing;
            sender = null;

            _transaction = null;
        }

        public void Dispose()
        {
            Session?.Close();
            Session?.Dispose();
            Session = null;
        }

        public IDbConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectString);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            return connection;
        }
    }
}
