using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;
using WXL.ViewModel;

namespace Common
{
    public class CommonHelper
    {
   
        #region 去除html返回内容
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
        #endregion
        #region Md5加密
        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>返回加密后的md5字符串</returns>
        public static string MD5Encry(string str)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider  
            MD5 md5 = MD5.Create();
            byte[] bs = Encoding.UTF8.GetBytes(str);
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化  
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion
        #region Md5加盐加密
        /// <summary>
        /// md5加盐加密
        /// </summary>
        /// <param name="str">要加密的string</param>
        /// <returns></returns>
        public static string MD5AppendStr(string str)
        {
            return MD5Encry(str + "wxl.123");
        }
        #endregion
        #region 生成随机码       
        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="length">随机码长度</param>
        /// <returns></returns>
        public static string GetRandCode(int length)
        {
            //生成随机数
            string authTxt = "";
            StringBuilder stringbd = new StringBuilder();
            //添加数字1-9
            for (int i = 1; i <= 9; i++)
            {
                stringbd.Append(i.ToString());
            }
            //添加大写字母A-Z,不包括O
            char temp = ' ';
            for (int i = 0; i < 26; i++)
            {
                temp = Convert.ToChar(i + 65);
                if (!temp.Equals('O'))
                {
                    stringbd.Append(temp);
                }
            }
            //添加小写字母a-z,不包括o
            for (int i = 0; i < 26; i++)
            {
                temp = Convert.ToChar(i + 97);
                if (!temp.Equals('o'))
                {
                    stringbd.Append(temp);
                }
            }
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                authTxt += stringbd[rd.Next(0, stringbd.Length)];
            }
            return authTxt;
        }
        #endregion
        #region 根据字符串生成验证码图象
        /// <summary>
        /// 根据字符串生成验证码图象
        /// </summary>
        /// <param name="authTxt"></param>
        /// <returns></returns>
        public static Bitmap GetValidateCode(string authTxt)
        {
            Random rd = new Random();
            //创建绘图,宽度根据验证码长度改变，高度为24
            Bitmap bitmap = new Bitmap(authTxt.Length * 18, 24);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;//图象质量
                //清除整个绘图面并以指定背景色填充
                g.Clear(Color.AliceBlue);
                //创建画笔
                using (SolidBrush brush = new SolidBrush(Color.Blue))
                {
                    //添加前景噪点
                    for (int i = 0; i < bitmap.Width * 2; i++) //多少个噪点
                    {
                        bitmap.SetPixel(rd.Next(bitmap.Width), rd.Next(bitmap.Height), Color.Blue);
                    }
                    //添加背景噪点
                    using (Pen pen = new Pen(Color.Azure, 0))
                    {
                        for (int i = 0; i < bitmap.Width * 2; i++)
                        {
                            g.DrawRectangle(pen, rd.Next(bitmap.Width), rd.Next(bitmap.Height), 1, 1);
                        }
                    }
                    //文字居中
                    StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    //字体样式
                    Font font = new Font("Times New Roman", rd.Next(15, 18), FontStyle.Regular);
                    //验证码旋转，防止机器识别
                    char[] chars = authTxt.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        //转动度数
                        float angle = rd.Next(-40, 40);
                        g.TranslateTransform(12, 12);
                        g.RotateTransform(angle);
                        g.DrawString(chars[i].ToString(), font, brush, -2, 2, format);
                        g.RotateTransform(-angle);
                        g.TranslateTransform(2, -12);
                    }
                }
            }
            return bitmap;
        }
        #endregion
        # region 递归生成菜单
        /// <summary>
        /// 递归生成菜单
        /// </summary>
        /// <param name="menus">菜单集合</param>
        /// <param name="pid">父id</param>
        /// <returns></returns>
        public static List<MenuVModel> getMenuVModel(List<MenuVModel> menus, int pid)
        {
            List<MenuVModel> result = new List<MenuVModel>();
            List<MenuVModel> childs = new List<MenuVModel>();
            foreach (var item in menus)
            {
                if (item.Pid == pid)
                {
                    result.Add(item);
                    childs = getMenuVModel(menus, item.Id);
                    if (childs.Count > 0)
                    {
                        item.Childs = childs;
                    }
                }
            }
            return result;
        }
        #endregion
        #region 计算总页数
        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <param name="PageSize">每页显示条数</param>
        /// <param name="TotalCount">总条数</param>
        /// <returns></returns>
        public static int GetPageCount(int PageSize,int TotalCount)
        {
            return TotalCount % PageSize > 0 ? TotalCount / PageSize + 1 : TotalCount / PageSize;
        }
        #endregion
        /// <summary>
        /// string 转int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StrToInt(string str)
        {
            return int.Parse(str);
        }
        /// <summary>
        /// 根据跳过的页数计算当前页
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetPageIndex(int offset,int pageSize)
        {
            return offset / pageSize + 1;
        }
    }
}
