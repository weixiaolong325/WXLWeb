using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXLWeb.Models;
namespace WXLWeb.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //1.查看sessiion是否已经有用户登录
            HttpCookie cookie = Request.Cookies.Get("UserId");
            if (Session["UserId"] != null)
            {
                return RedirectToAction("Index", "WebManagement");
            }
            //2.查看cookie是否保存有用户登录信息
            else if (cookie != null)
            {
                Session["UserId"] = cookie.Value;
                return RedirectToAction("Index", "WebManagement");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(Users user)
        {
            if (ModelState.IsValid)
            {
                //1.判断用户名密码是否正确
                string sql = "select COUNT(1) from WXL_Users where UserId=@UserId COLLATE Chinese_PRC_CS_AI_WS and Pwd=@Pwd";
                SqlParameter[] param ={new SqlParameter("@UserId",SqlDbType.VarChar,20){Value=user.UserId},
                                         new SqlParameter("@Pwd",SqlDbType.VarChar,64){Value=user.Pwd}};
                if (Convert.ToInt32(SQLHelper.ExecuteScalar(sql, CommandType.Text, param)) <= 0)
                {
                    ModelState.AddModelError("error_login", "用户名或密码错误");
                    return View();
                }
                //2.用户名密码正确则保持用户登录状态
                Session["UserId"] = user.UserId;
                //3.如果用户选择一周免登录,则把用户保存在cookie中
                if (Request.Form["remberpwd"] != null)
                {
                    HttpCookie cookie = new HttpCookie("UserId");
                    cookie.Expires = DateTime.Now.AddDays(7);
                    cookie.Value = user.UserId;
                    Response.Cookies.Add(cookie);
                }
                return RedirectToAction("Index", "WebManagement");
            }
            else return View();
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            Session.Remove("UserId");
            HttpCookie cookie = Request.Cookies.Get("UserId");
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            return RedirectToAction("Login", "User");
        }

    }
}
