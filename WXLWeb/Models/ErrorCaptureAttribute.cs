using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WXLWeb.Models
{
    public class ErrorCaptureAttribute : HandleErrorAttribute
    {
        //定义一个队列接收异常
        public static Queue<Exception> ErrorQueue = new Queue<Exception>();
        //程序出错就会执行此方法
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            ErrorQueue.Enqueue(filterContext.Exception);//异常入队
            //跳转到错误页面
            //filterContext.HttpContext.Response.Redirect("/ReWrite/Error.html");
        }
    }
}