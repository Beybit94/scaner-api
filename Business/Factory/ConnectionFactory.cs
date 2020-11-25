using Common.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Factory
{
    public static class ConnectionFactory
    {
        public static IDbConnection Create()
        {
            var sqlConn = new SqlConnection(ConfigurtionOptions.MainConnectionString);
            sqlConn.Open();
            return sqlConn;
        }

        public static IDbConnection Create(string connectionString)
        {
            var sqlConn = new SqlConnection(connectionString);
            sqlConn.Open();
            return sqlConn;
        }
    }
}
