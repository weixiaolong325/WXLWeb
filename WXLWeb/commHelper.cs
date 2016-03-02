using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageNum">当前第几页</param>
        /// <param name="pageNumSum">总页数</param>
        /// <param name="url">显示的url</param>
        /// <param name="p">分页参数名</param>
        /// <param name="linCount">显示多少页</param>
        /// <returns></returns>
        public static string page(int pageNum,int pageNumSum,string url,string p,int linCount)
        {
            if (pageNum <1) pageNum = 1;
            if (pageNum > pageNumSum) pageNum = pageNumSum;
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<div class='page'>第<span style='color:#00ff21'>{0}</span>/{1} 页",pageNum,pageNumSum));
            sb.Append(string.Format(" <a href='{0}?{1}={2}'>上一页</a>",url,p,pageNum-1<1?1:pageNum-1));
            //设开始页为1
            int beginPage = 1;
            //中间页为 linCount / 2
            int midPage = linCount / 2;
            if (pageNum > midPage)
            {
                beginPage = pageNum - midPage;
            }
            for (int i = 0; i < linCount; i++)
            {
                //当前页不加链接
                if (pageNum == beginPage + i)
                {
                    sb.Append(string.Format("<span class='sp_blank'></span>{0}<span class='sp_blank'></span>", beginPage + i));
                }
                else
                {
                    sb.Append(string.Format("<a href='{0}?{1}={2}'>{2}</a>",url,p,beginPage+i));
                }
                //如果后面的页数大于总页数，退出循环
                if (beginPage + i >= pageNumSum)
                {
                    break;
                }

            }

            sb.Append(string.Format(" <a href='{0}?{1}={2}'>下一页</a>", url, p, pageNum + 1 >=pageNumSum ? pageNumSum : pageNum+1));
            return sb.ToString();
        }
    }
}