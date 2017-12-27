using System.Web;
using System.Web.Mvc;
using WXLWeb.Models;

namespace WXLWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorCaptureAttribute());
        }
    }
}