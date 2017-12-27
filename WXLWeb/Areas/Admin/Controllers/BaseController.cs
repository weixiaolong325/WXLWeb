using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXL.Model;

namespace WXLWeb.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        public WXL_Users loginUser { get; set; }
        /// <summary>
        /// 进入控制器前先执行这个方法
        /// </summary>
        /// <param name="filterContext"></param>
       protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            bool isSuccess = false;
            if(Request.Cookies[Setting.sessionId] !=null)
            {
                string sessionId = Request.Cookies[Setting.sessionId].Value;
                //根据该值查redis
                WXL_Users obj = CachedHelper.GetCached<WXL_Users>(sessionId);
                if(obj!=null)
                {
                    loginUser = obj;
                    isSuccess = true;
                    //模拟滑动过期时间
                    CachedHelper.SetCached<WXL_Users>(sessionId, loginUser, DateTime.Now.AddMinutes(20));
                    //留一个后门，测试方便。发布的时候一定要删除该代码。
                    if (loginUser.UserName == "admin")
                    {
                        return;
                    }
                    //判断用户是否有权限访问这个地址
                }
            }
            if (!isSuccess)
            {
                filterContext.Result = Redirect("/Admin/User/Login");//注意.
            }
        }
    }
}