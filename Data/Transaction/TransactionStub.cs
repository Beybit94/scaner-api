using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ScanerApi.Data.Transaction
{
    /// <summary>
    /// Виртуальная транзакция
    /// </summary>
    public class TransactionStub : IDbTransaction
    {
        public TransactionStub(IDbConnection connection, IsolationLevel isolationLevel)
        {
            Connection = connection;
            IsolationLevel = isolationLevel;
        }

        public void Dispose()
        {
            Connection = null;
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public IDbConnection Connection { get; private set; }
        public IsolationLevel IsolationLevel { get; private set; }
    }
}
