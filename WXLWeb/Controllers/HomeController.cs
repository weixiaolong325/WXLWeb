using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        //文章
        public ActionResult Skill()
        {
            return View();
        }
    }
}
