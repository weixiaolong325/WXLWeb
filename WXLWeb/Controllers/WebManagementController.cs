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

        //增加文章
        public ActionResult AddArticle()
        {
            //1.查看sessiion是否已经有用户登录
            HttpCookie cookie = Request.Cookies.Get("UserId");
            Article article = new Article();
            if (Session["UserId"]!= null)
            {
                article.UserId = Session["UserId"].ToString();
            }
                //2.查看cookie是否保存有用户登录信息
            else if (cookie != null)
            {
                Session["UserId"] = cookie.Value;
                article.UserId = Session["UserId"].ToString();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
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
            article.UserId = article.UserId = Session["UserId"].ToString();
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
            string sql = "insert into WXL_Article(Title,ContentTxt,Type1,Type2,UserId,UserName,Tag,Abstract) values(@Title,@ContentTxt,@Type1,@Type2,@UserId,@UserName,@Tag,@Abstract)";
            SqlParameter[] param ={new SqlParameter("@Title",SqlDbType.NVarChar,30){Value=article.Title},
                                     new SqlParameter("@ContentTxt",SqlDbType.Text){Value=article.ContentTxt},
                                     new SqlParameter("@Type1",SqlDbType.Int){Value=article.Type1},
                                     new SqlParameter("@Type2",SqlDbType.NVarChar,20){Value=article.Type2},
                                     new SqlParameter("@UserId",SqlDbType.VarChar,20){Value=article.UserId},
                                     new SqlParameter("@UserName",SqlDbType.NVarChar,10){Value=article.UserName},
                                     new SqlParameter("@Tag",SqlDbType.NVarChar,50){Value=article.Tag},
                                  new SqlParameter("@Abstract",SqlDbType.NVarChar,220){Value=article.Abstract}};
            if (SQLHelper.ExecuteNonQuery(sql, CommandType.Text, param) > 0)
                return Content("添加成功!");
            else
            {
                return View();
            }
        }

    }
}
