using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXL.IBLL;
using WXL.Model;
using WXL.ViewModel;
using WXL.ViewModel.Admin;

namespace WXLWeb.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private IWXL_MENUBll _MenuBll;
        private IWXL_RoleBll _RoleBll;
        public HomeController(IWXL_MENUBll MenuBll, IWXL_RoleBll RoleBll)
        {
            _MenuBll = MenuBll;
            _RoleBll = RoleBll;
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            //获取全部菜单
            IQueryable<WXL_MENU> menus = _MenuBll.LoadEntities(m => true);
            //把菜单model转为viewModel
            var v_menus = from m in menus
                          select new MenuVModel()
                          {
                              Id = m.Id,
                              Pid = m.Pid,
                              Ico = m.Ico,
                              MenuName = m.MenuName,
                              Url = m.Url,
                              Sort = m.Sort
                          };
            //生成层级菜单
            var resulMenus = CommonHelper.getMenuVModel(v_menus.ToList(), 0);
           
            //查询角色信息
            List<WXL_Role> roles = _RoleBll.GetRoleByUserId(loginUser.Id).ToList();
            string roleNames = "";
            if(roles!=null)
            {
                foreach(var roleName in roles )
                {
                    roleNames += roleName.RoleName + ",";
                }
               roleNames=roleNames.Substring(0, roleNames.Length - 1);
            }
            //用户名
            ViewBag.userName = loginUser.NickName;
            //角色
            ViewBag.Role = roleNames;

            return View(resulMenus);
        }
        public ActionResult Default() //默认页
        {
            return View();
        }
        public ActionResult Colors()
        {
            return View();
        }
    }
}