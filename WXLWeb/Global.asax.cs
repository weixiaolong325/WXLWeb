using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WXLWeb.CommonFun;
using WXLWeb.DAL;
using WXLWeb.Models;

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
            IOCHelper.IocInit();//ioc初始化

            log4net.Config.XmlConfigurator.Configure();//读取Log4Net配置信息
            //开启一个线程来不断监听错误信息
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)//用死循环监听,一直执行这个线程,不会停止
                {
                    //异常队列数大于0
                    if (ErrorCaptureAttribute.ErrorQueue.Count > 0)
                    {
                        //移除并接收队列开始处的消息
                        Exception ab = ErrorCaptureAttribute.ErrorQueue.Dequeue();
                         //创建一个logger,参数为logger的key
                        ILog logger = LogManager.GetLogger("WebLogger");
                        //记录日志
                        logger.Error(ab.ToString());
                    }
                    else
                    {
                        Thread.Sleep(4000);//没有扫描到错误信息，休息4秒时间，减轻cpu压力
                    }
                }

            });
        }
           
        
    }
   
}