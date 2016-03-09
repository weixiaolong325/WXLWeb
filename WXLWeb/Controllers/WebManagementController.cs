using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXLWeb.Models;
using System.Data;
using System.Data.SqlClient;
using Common;
using WXLWeb.ViewModels;

namespace WXLWeb.Controllers
{
    public class WebManagementController : Controller
    {
        //
        // GET: /WebManagement/
        /// <summary>
        /// 后台管理主界面
        /// </summary>
        /// <returns></returns>
        
        [Authentication]
        public ActionResult Index()
        {
            return View();
        }
        //增加文章
           [Authentication]
        public ActionResult AddArticle()
        {
            Article article = new Article();
            article.UserId = Session["UserId"].ToString();
           //3.查询用户名

            string sql = "select Name from  WXL_Users where UserId=@UserId";
            SqlParameter[] param = { new SqlParameter("@UserId", SqlDbType.VarChar, 20) { Value = article.UserId } };
            article.UserName = SQLHelper.ExecuteScalar(sql, CommandType.Text, param);
            return View(article);
        }
        //增加文章
        [Authentication]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddArticle(Article article,FormCollection fc)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }
           article.UserId = Session["UserId"].ToString();
            //根据用户id获得用户名
            string name = commHelper.getUserName(article.UserId);
            if ( name!= null)
            {
                article.UserName=name;
            }
            else 
            article.UserName = "";
            article.ContentTxt = fc["content"];
            //文章摘要
            article.Abstract = Commonfun.ReplaceHtmlTag(article.ContentTxt, 150);
            //用时间戳生成文章ID2
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            article.ArticleId2=Convert.ToInt64(ts.TotalMilliseconds).ToString() + new Random().Next(100, 1000);
           
            string sql = "insert into WXL_Article(Title,ContentTxt,Type1,Type2,UserId,UserName,ArticleId2,Abstract) values(@Title,@ContentTxt,@Type1,@Type2,@UserId,@UserName,@ArticleId2,@Abstract)";
            SqlParameter[] param ={new SqlParameter("@Title",SqlDbType.NVarChar,30){Value=article.Title},
                                     new SqlParameter("@ContentTxt",SqlDbType.Text){Value=article.ContentTxt},
                                     new SqlParameter("@Type1",SqlDbType.Int){Value=article.Type1},
                                     new SqlParameter("@Type2",SqlDbType.NVarChar,20){Value=article.Type2},
                                     new SqlParameter("@UserId",SqlDbType.VarChar,20){Value=article.UserId},
                                     new SqlParameter("@UserName",SqlDbType.NVarChar,10){Value=article.UserName},
                                     new SqlParameter("@ArticleId2",SqlDbType.VarChar,20){Value=article.ArticleId2},
                                  new SqlParameter("@Abstract",SqlDbType.NVarChar,220){Value=article.Abstract} };
            //插入文章
            if (SQLHelper.ExecuteNonQuery(sql, CommandType.Text, param) > 0)
            {
                //文章标签
                if (article.Tag != null)
                {
                    List<string> str = article.Tag.Split(",，".ToCharArray()).ToList<string>();
                    foreach (var tag in str)
                    {
                        string sqlAddTag = "insert into WXL_Tag(ArticleId2,TagName,ArticleType1) values(@ArticleId2,@TagName,@ArticleType1)";
                        SqlParameter[] paramTag ={new SqlParameter("@ArticleId2",article.ArticleId2),
                                                new SqlParameter("@TagName",tag),
                                                new SqlParameter("@ArticleType1",article.Type1)};
                        //插入标签
                        SQLHelper.ExecuteNonQuery(sqlAddTag, CommandType.Text, paramTag);
                    }
                }
                return Content("添加成功!");
            }
            else
            {
                return View();
            }
        }

        //文章列表
        [Authentication]
        public ActionResult ArticleList(string page)
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
            return View(articleView(p, "/WebManagement/ArticleList"));
        }

        /// <summary>
        /// 文章列表，返回一个文章集合
        /// </summary>
        /// <param name="type1">文章类别</param>
        /// <param name="pageNum">第几页</param>
        /// <returns></returns>
        [Authentication]
        private ArticleView articleView(int pageNum, string url)
        {
            ArticleView articleList = new ArticleView();
            List<Article> articles = new List<Article>();

            int linNum = 15;//一页显示多少条记录
            string sql = string.Format("select COUNT(1) from WXL_Article where Isdel=0");
            //总条数
            int NumSum = Convert.ToInt32(SQLHelper.ExecuteScalar(sql, CommandType.Text));
            //总页数
            int pageNumSum = NumSum % linNum > 0 ? NumSum / linNum + 1 : NumSum / linNum;
            if (pageNum > pageNumSum) pageNum = pageNumSum;
            articleList.pageNumSum = pageNumSum;
            //当前页数
            articleList.pageNum = pageNum;
            SqlParameter[] param ={new SqlParameter("@LineNum",linNum),
                                     new SqlParameter("@pageNum",pageNum)
                                  };
            ViewBag.page = new MvcHtmlString(commHelper.page(pageNum, pageNumSum, url, "page", 5));
            //文章列表
            using (SqlDataReader sdr = SQLHelper.ExecuteReader("Long_ArticleToPageAllType", CommandType.StoredProcedure, param))
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
                        article.LookNum = Convert.ToInt32(sdr["LookNum"]);
                        article.Abstract = sdr["Abstract"].ToString();

                        articles.Add(article);
                    }
                    articleList.Articles = articles;
                }
            }
            return articleList;
        }

        //修改文章
        //[Authentication]
        public ActionResult AlterArticle(string id)
        {
            AlterArticleView alterArticleView = new AlterArticleView();
            Article article = new Article();
            if (id == null)
            {
                return Redirect("/ReWrite/Error.html");
            }
            string sql = "select ArticleId,Title,ContentTxt,Type1,Type2,Abstract,b.TagId,b.TagName from WXL_Article  a left join WXL_Tag b on a.ArticleId2=b.ArticleId2 and a.Isdel=0 and b.IsDel=0 where ArticleId=@ArticleId ";
            SqlParameter []param={new SqlParameter("@ArticleId",id)};
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(sql, CommandType.Text, param))
            {
                if (!sdr.HasRows)
                {
                    return Redirect("/ReWrite/Error.html");
                }
                else
                {
                    bool fistline = true;
                    while(sdr.Read())
                    {
   
                        //读取第一行
                        if (fistline == true)
                        {
                            fistline = false;
                            article.ArticleId = Convert.ToInt32(sdr["ArticleId"]);
                            article.Title = sdr["Title"].ToString();
                           ViewBag.content= new  MvcHtmlString(sdr["ContentTxt"].ToString());
                            article.Type1 = Convert.ToInt32(sdr["Type1"]);
                            article.Type2 = sdr["Type2"].ToString();
                            //标签
                            if (sdr["TagName"] != null)
                            {
                                alterArticleView.tags= sdr["TagName"].ToString();
                            }
                        }
                        else 
                        {
                            //标签
                            if (sdr["TagName"] != null)
                            {
                                alterArticleView.tags+= ("," + sdr["TagName"].ToString());
                            }
                        }
                    }
                    alterArticleView.article = article;

                }
            }

            return View(alterArticleView);
        }

        [HttpPost]
        public ActionResult AlterArticle(FormCollection fc)
        {
            return Content("暂没有修改功能");
        }
    }
}
