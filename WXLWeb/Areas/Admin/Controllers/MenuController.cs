using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXL.IBLL;
using WXL.ViewModel;

namespace WXLWeb.Areas.Admin.Controllers
{
    public class MenuController : Controller
    {
        private IWXL_MENUBll _MenuBll;
        public MenuController(IWXL_MENUBll MenuBll)
        {
            _MenuBll = MenuBll;
        }
        // GET: Admin/Menu
        public ActionResult Index()
        {
            return View();
        }
        //菜单
        public ActionResult GetMenuList()
        {
            //获取全部菜单
           var menus =_MenuBll.LoadEntities(m => true);
            //把菜单model转为viewModel
            var v_menus = from m in menus
                          select new MenuVModel()
                          {
                              Id = m.Id,
                              Pid=m.Pid,
                              Ico=m.Ico,
                              MenuName=m.MenuName,
                              Url=m.Url,
                              Sort=m.Sort
                          };
            //生成层级菜单
            var resulMenus = CommonHelper.getMenuVModel(v_menus.ToList(), 0);
            return Json(resulMenus,JsonRequestBehavior.AllowGet);
        }
      
    }
}