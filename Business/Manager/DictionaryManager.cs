using Business.Factory;
using Business.Models.Dictionary;
using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Manager
{
    public class DictionaryManager
    {
        /// <summary>
        /// Gets the dictionary collection of the give type from DB.
        /// Type of dictionary must be subclass of BaseDictionary!
        /// </summary>
        /// <returns>Array of dictionary of the given type</returns>
        public static T[] GetDictionary<T>() where T : BaseDictionary
        {
            T[] result;
            using (IDbConnection con = ConnectionFactory.Create())
            {
                string tableName = GetTableName(typeof(T)) ?? typeof(T).Name;
                string queryStr = string.Format("select c.* from {0} AS c order by Id", tableName);

                result = con.Query<T>(queryStr, new { EndDate = DateTime.Now }).ToArray();
            }

            return result;
        }

        /// <summary>
        /// Gets the dictionary collection of the give type from DB.
        /// Type of dictionary must be subclass of BaseDictionary!
        /// </summary>
        /// <returns>Array of dictionary of the given type</returns>
        public static T[] GetDictionaryShort<T>() where T : BaseDictionaryShort
        {
            T[] result;
            using (IDbConnection con = ConnectionFactory.Create())
            {
                string tableName = GetTableName(typeof(T)) ?? typeof(T).Name;
                string queryStr = string.Format("select c.* from {0} AS c order by Id", tableName);

                result = con.Query<T>(queryStr, new { EndDate = DateTime.Now }).ToArray();
            }

            return result;
        }

        /// <summary>
        /// Gets the table's name that a class is mapped to (Тhrough the System.ComponentModel.DataAnnotations.Schema.TableAttribute)
        /// </summary>
        /// <param name="type">typeof(BO)</param>
        /// <returns>Mapped table's name</returns>
        private static string GetTableName(Type type)
        {
            object[] temp = type.GetCustomAttributes(typeof(TableAttribute), true);

            return temp.Length == 0 ? null : ((TableAttribute)temp[0]).Name;
        }
    }
}
