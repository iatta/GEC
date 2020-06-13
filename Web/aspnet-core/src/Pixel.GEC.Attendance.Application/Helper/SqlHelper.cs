using FastMember;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Pixel.GEC.Attendance.Helper
{
   public static class SqlHelper
    {
        public static T ConvertToObject<T>(this DbDataReader rd) where T : class, new()
        {

            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                if (!rd.IsDBNull(i))
                {
                    string fieldName = rd.GetName(i);
                    
                    try
                    {
                        if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                        {
                            accessor[t, fieldName] = rd.GetValue(i);
                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                   
                }
            }

            return t;
        }
    }
}
