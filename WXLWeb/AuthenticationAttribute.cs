using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WXLWeb
{
    public class AuthenticationAttribute:ActionFilterAttribute
    {
         
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //查看session是否为空
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                //跳转到登录页
                filterContext.HttpContext.Response.Redirect("/User/Login");
            }
            else
            {
               //不做任何事件
            }
           
        }
    }
}