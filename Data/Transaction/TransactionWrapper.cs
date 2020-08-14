using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ScanerApi.Data.Transaction
{
    public class TransactionWrapper : IDbTransaction
    {
        public TransactionWrapper(IDbTransaction transaction)
        {
            InternalTransaction = transaction;
        }

        internal IDbTransaction InternalTransaction { get; private set; }

        public void Dispose()
        {
            OnDisposing();
            InternalTransaction.Dispose();
            InternalTransaction = null;
        }

        public void Commit()
        {
            InternalTransaction.Commit();
        }

        public void Rollback()
        {
            InternalTransaction.Rollback();
        }

        public IDbConnection Connection => InternalTransaction.Connection;
        public IsolationLevel IsolationLevel => InternalTransaction.IsolationLevel;

        public event Action<TransactionWrapper> Disposing;

        protected virtual void OnDisposing()
        {
            Disposing?.Invoke(this);
        }
    }
}
