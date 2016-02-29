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
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddArticle(Article article)
        {
            article.UserId = "weixiaolong";
            article.UserName = "韦小龙";
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
