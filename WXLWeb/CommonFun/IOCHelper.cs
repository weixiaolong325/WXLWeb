using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WXL.BLL;
using WXL.IBLL;

namespace WXLWeb.CommonFun
{
    public class IOCHelper
    {
        /// <summary>
        /// autofac --Ioc容器初始化
        /// </summary>
        public static void IocInit()
            {
                //autofac --Ioc容器初始化
                ContainerBuilder builder = new ContainerBuilder();
                //注册了当前程序集内所有的Controller类
                builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //注册了当前程序集内所有类
            // builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //     .AsImplementedInterfaces();

             builder.RegisterType<WXL_ArticleBll>().As<IWXL_ArticleBll>();
            builder.RegisterType<WXL_UsersBll>().As<IWXL_UsersBll>();
            builder.RegisterType<WXL_MENUBll>().As<IWXL_MENUBll>();
            builder.RegisterType<WXL_RoleBll>().As<IWXL_RoleBll>();
                var container = builder.Build();
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            }
        /// <summary>
        /// 返回对应的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetByName<T>(string name)
        {
            return AutofacDependencyResolver.Current.RequestLifetimeScope.ResolveNamed<T>(name);
        }
    }


}