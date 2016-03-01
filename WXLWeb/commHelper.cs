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
        /// <summary>
        /// 根据用户Id返回用户名
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static string getUserName(string UserId)
        {
            string name = string.Empty;
            string sql = "select Name from WXL_Users where UserId=@UserId";
            SqlParameter[] param = { new SqlParameter("@UserId",UserId) };
            return SQLHelper.ExecuteScalar(sql, CommandType.Text, param);
           
        }
    }
}