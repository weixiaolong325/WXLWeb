using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WXLWeb.DAL;

namespace WXLWeb
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_Error()
        {
            //获取最后一次出错的错误对象
            Exception exMsg = Context.Server.GetLastError();
            //根据错误对象生成错误消息
            StringBuilder sbLogText = new StringBuilder();
            //写入时间
            sbLogText.AppendLine(DateTime.Now.ToString("yyyy-MM-dd"));
            //写入具体的错误信息
            sbLogText.AppendLine("错误消息:" + exMsg.Message);
            sbLogText.AppendLine("错误详细信息:" + exMsg.StackTrace);
            if (exMsg != null)
            {
                sbLogText.AppendLine("==========内部异常信息=========:" + exMsg.InnerException);
            }
            sbLogText.AppendLine(Environment.NewLine);//空一行
            sbLogText.AppendLine("----出错客户端IP:" + Context.Request.UserHostAddress + "---------");
            //3.把错误传递给对应的记录日志的方法
            LogHelper.LogEnqueue(sbLogText.ToString());
            Response.Redirect("~/ReWrite/Error.html", true);
        }
    }
   
}