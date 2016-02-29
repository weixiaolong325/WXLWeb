using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Common
{
    public class Commonfun
    {
        /// <summary>
        /// 去除html返回内容
        /// </summary>
        /// <param name="html">带html的内容</param>
        /// <param name="length">反回字符串长度</param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html, int length)
        {
            string strText = Regex.Replace(html, "<[^>]+>", "");
            strText = Regex.Replace(strText, "&[^;]+;", "");
            if (length > 0 && strText.Length > length)
            {
                return strText.Substring(0, length);
            }
            return strText;
        }
    }
}
