using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXLWeb.Models;
using System.Data;
using System.Data.SqlClient;
using Common;

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
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddArticle(Article article)
        {
           article.UserId = Session["UserId"].ToString();
            //根据用户id获得用户名
            string name = commHelper.getUserName(article.UserId);
            if ( name!= null)
            {
                article.UserName=name;
            }
            else 
            article.UserName = "";
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

    }
}
