using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXLWeb.Models;
using System.Data;
using System.Data.SqlClient;
using WXLWeb.ViewModels;
using Common;

namespace WXLWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        //留言板
        public ActionResult MessageBoard()
        {
            return View();
        }
        //关于我
        public ActionResult AboutMe()
        {
            return View();
        }
        //技术分享
        public ActionResult Skill(string page)
        {
            //默认第一页
            int p = 1;
            if (page != null)
            {
                int isint;
                if (!int.TryParse(page, out isint))
                {
                    return Content("查看内容不存在");
                }
                p = Convert.ToInt32(page) < 1 ? 1 : Convert.ToInt32(page);
            }
            return View(articleView(1,p,"/Home/Skill"));
        }
        //生活
        public ActionResult Life(string page)
        {
            //默认第一页
            int p = 1;
            if (page != null)
            {
                int isint;
                if (!int.TryParse(page, out isint))
                {
                    return Content("查看内容不存在");
                }
                p = Convert.ToInt32(page) < 1 ? 1 : Convert.ToInt32(page);
            }
            return View(articleView(2,1,"/Home/Life"));
        }
        //学习
        public ActionResult Learn(string page)
        {
            //默认第一页
            int p = 1;
            if (page != null)
            {
                int isint;
                if (!int.TryParse(page, out isint))
                {
                    return Content("查看内容不存在");
                }
                p = Convert.ToInt32(page) < 1 ? 1 : Convert.ToInt32(page);
            }
            return View(articleView(3,1,"/Home/Learn"));
        }
        /// <summary>
        /// 文章列表，返回一个文章集合
        /// </summary>
        /// <param name="type1">文章类别</param>
        /// <param name="pageNum">第几页</param>
        /// <returns></returns>
        private ArticleView articleView(int type1,int pageNum,string url)
        {
            ArticleView articleList = new ArticleView();
            List<Article> articles = new List<Article>();

            int linNum = 5;//一页显示多少条记录
            int Type1 =type1 ;//类别为
            string sql = string.Format("select COUNT(1) from WXL_Article where Type1={0} and Isdel=0", type1);
            //总条数
            int NumSum = Convert.ToInt32(SQLHelper.ExecuteScalar(sql, CommandType.Text));
            //总页数
            int pageNumSum = NumSum % linNum > 0 ? NumSum / linNum + 1 : NumSum / linNum;
            if (pageNum > pageNumSum) pageNum = pageNumSum;
            articleList.pageNumSum = pageNumSum;
            //当前页数
            articleList.pageNum = pageNum;
            SqlParameter[] param ={new SqlParameter("@LineNum",linNum),
                                     new SqlParameter("@pageNum",pageNum),
                                  new SqlParameter("@Type1",Type1)};
            ViewBag.page = new MvcHtmlString(commHelper.page(pageNum, pageNumSum,url, "page", 5));
            //文章列表
            using (SqlDataReader sdr = SQLHelper.ExecuteReader("Long_ArticleToPage", CommandType.StoredProcedure, param))
            {
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Article article = new Article();
                        article.ArticleId = Convert.ToInt32(sdr["ArticleId"]);
                        article.Title = sdr["Title"].ToString();
                        article.Type1 = Convert.ToInt32(sdr["Type1"]);
                        article.Type2 = sdr["Type2"].ToString();
                        article.CreateTime = Convert.ToDateTime(sdr["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        article.UserId = sdr["UserId"].ToString();
                        article.UserName = sdr["UserName"].ToString();
                        article.Tag = sdr["Tag"].ToString();
                        article.LookNum = Convert.ToInt32(sdr["LookNum"]);
                        article.Abstract = sdr["Abstract"].ToString();

                        articles.Add(article);
                    }
                    articleList.Articles = articles;
                }
            }
            return articleList;
        }
    }
}
