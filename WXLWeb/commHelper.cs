using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace WXLWeb
{
    public class commHelper
    {
        public static string getUserName(string UserId)
        {
            string name = string.Empty;
            string sql = "select Name from WXL_Users where UserId=@UserId";
            SqlParameter[] param = { new SqlParameter("@UserId",UserId) };
            return SQLHelper.ExecuteScalar(sql, CommandType.Text, param);
           
        }
    }
}