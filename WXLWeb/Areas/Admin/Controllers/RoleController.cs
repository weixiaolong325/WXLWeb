using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXL.IBLL;
using WXL.Model;
using WXL.Model.Enum;
using WXL.ViewModel;

namespace WXLWeb.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        private IWXL_RoleBll _roleBll;
        public RoleController(IWXL_RoleBll roleBll)
        {
            _roleBll = roleBll;
        }
        public ActionResult Index()
        {
            return View();
        }
        //获取角色列表
        public ActionResult GetList(PageVModel page, WXL_Role Role)
        {
            page.PageIndex = CommonHelper.GetPageIndex(page.PageIndex, page.PageSize);
            int totalCount;
            WhereHelper<WXL_Role> lambdaWhere = new WhereHelper<WXL_Role>();
            //条件查找
            if(Role!=null)
            {
                if(!string.IsNullOrEmpty(Role.RoleName))
                {
                    lambdaWhere.Contains("RoleName", Role.RoleName);
                }
            }
            //分页获取数据
            var roles = _roleBll.LoadPageEntities<int>(page.PageIndex, page.PageSize, out totalCount, lambdaWhere.GetExpression(), u => u.Id, false);
            return Json(new { total = totalCount, rows = roles.ToList() }, JsonRequestBehavior.AllowGet);
        }
        //增加角色
        [HttpPost]
        public ActionResult AddRole(WXL_Role Role)
        {
            if(Role==null)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "请输入角色信息"));
            }
            if(string.IsNullOrEmpty(Role.RoleName)||Role.RoleName.Length>10)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "名称不存在或超出长度"));
            }
            //查询角色是否已存在
            var role = _roleBll.LoadEntities(r => r.RoleName.Equals(Role.RoleName)).FirstOrDefault();
            if(role!=null)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "角色已存在"));
            }
            //插入数据库
            var result = _roleBll.AddEntity(Role);
            return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "增加成功"));
        }
        //修改角色
        [HttpPost]
        public ActionResult EditRole(WXL_Role Role)
        {
            if(Role==null)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "请输入角色信息"));
            }
            if (string.IsNullOrEmpty(Role.RoleName) || Role.RoleName.Length > 10)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "名称不存在或超出长度"));
            }
            //查询角色
            var role = _roleBll.LoadEntities(r => r.Id == Role.Id).SingleOrDefault();
            //修改
            role.RoleName = Role.RoleName;
            //提交修改
            var result = _roleBll.EditEntity(role);
            if(result)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "修改成功"));
            }
            else
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "修改失败"));
            }

        }
        //删除角色
        public ActionResult DeleteRole(List<int> Ids)
        {           
            var result = _roleBll.DeleteEntities(Ids);
             if(result)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "删除成功"));
            }
             else
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "删除失败"));
            }
        }
    }
}