using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace ScanerApi.Data.Extensions
{
    public static class TableExtensions
    {
        public static string TableName(this Type type)
        {
            object[] customAttributes = type.GetCustomAttributes(typeof(TableAttribute), true);
            return customAttributes.Length == 0 ? null : ((TableAttribute)customAttributes[0]).Name;
        }

        public static bool NotMapped(this PropertyInfo propertyInfo)
        {
            object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(NotMappedAttribute), true);
            return customAttributes.Length > 0;
        }
    }
}
