using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WXL.DALFactory;
using WXL.IBLL;
using WXL.IDAL;
using WXL.Model;
using WXL.Model.Enum;
using WXL.ViewModel;

namespace WXLWeb.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User


        private IWXL_UsersBll _bll;
        public UserController(IWXL_UsersBll Bll)
        {
            _bll = Bll;
        }
        #region 列表
        public ActionResult Index(PageVModel page,WXL_Users user)
        {
            int totalCount;
            WhereHelper<WXL_Users> lambdaWhere = new WhereHelper<WXL_Users>();
            if(!string.IsNullOrEmpty(user.UserName))
            {
                lambdaWhere.Contains("UserName", user.UserName);
            }
            if(!string.IsNullOrEmpty(user.NickName))
            {
                lambdaWhere.Contains("NickName", user.NickName);
            }
            ViewBag.UserName = user.UserName;
            ViewBag.NickName = user.NickName;

             //分页获取数据
             var users = _bll.LoadPageEntities<DateTime>(page.PageIndex, page.PageSize, out totalCount, lambdaWhere.GetExpression(), u => u.CreateTime, false);
            //获取总页数
            int pageCount = CommonHelper.GetPageCount(page.PageSize, totalCount);
            page.pageCount = pageCount;
            ViewBag.Page = page;

            return View(users.ToList());
        }
        #endregion
        #region 增加用户
        [HttpPost]
        public ActionResult AddUser(WXL_Users User)
        {
            User.UserPwd = "123456";//默认密码123456
            #region 1.校验录入的用户字段规则
            //用户名,2-16位数字，字母，下划线，点
            string userNameRegex = "^[a-zA-Z0-9_.]{2,16}$";           
            //昵称,不超过10位
            string nickNameRegex = "^.{0,10}$";
            if(!Regex.IsMatch(User.UserName, userNameRegex))
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "用户名不合规范"));
            }
            if (!string.IsNullOrEmpty(User.NickName))
            {
                if (!Regex.IsMatch(User.NickName, nickNameRegex))
                {
                    return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "昵称不合规范"));
                }
            }
            #endregion
            #region 2.查询是否存在同户名
            var user = _bll.LoadEntities(u => u.UserName == User.UserName).SingleOrDefault();
            if(user!=null)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "用户已存在"));
            }
            #endregion
            #region 3.保存到数据库
            //用户密码加密
            User.UserPwd = CommonHelper.MD5AppendStr(User.UserPwd);
            User.State = "0";//用户状态默认为启用
            User.SuperAdmin = "0";
            User.CreateTime = DateTime.Now;
            _bll.AddEntity(User);
            return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "增加成功"));
            #endregion

        }
        #endregion
        #region 修改用户
        [HttpPost]
        public ActionResult UpdateUser(WXL_Users User)
        {
            //昵称,不超过10位
            string nickNameRegex = "^.{0,10}$";
            if (!string.IsNullOrEmpty(User.NickName))
            {
                if (!Regex.IsMatch(User.NickName, nickNameRegex))
                {
                    return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "昵称不合规范"));
                }
            }
            //查出用户
            WXL_Users userInfo = _bll.LoadEntities(u=>u.Id==User.Id).SingleOrDefault();
            if(userInfo==null)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "用户不存在"));
            }
            userInfo.NickName = User.NickName;
            //更新用户
            bool result = _bll.EditEntity(userInfo);
            if(result)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "更新成功"));
            }
            else
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "更新失败"));
            }
        }
        #endregion
        #region 启禁用用户
        [HttpPost]
        public ActionResult UpdateUserState(string ids)
        {
            //ids eg "1001,1002,1003"
            if (string.IsNullOrEmpty(ids))
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail , "请选择一个用户"));
            }
            string[] strIds = ids.Split(',');
            List<int> intIds = new List<int>();
            foreach (string id in strIds)
            {
                intIds.Add(Convert.ToInt32(id));
            }
            bool result=_bll.UpdateUserState(intIds);
            if(result)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "更改状态成功"));
            }
            else
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "操作失败"));
            }
        }
        #endregion
        #region 登录页面
        public ActionResult Login()
        {
            return View();
        }
        #endregion
        #region 登录请求
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPwd">密码</param>
        /// <param name="ValidateCode">验证码</param>
        /// <param name="isRemember">记住密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string UserName,string UserPwd,string ValidateCode,bool isRemember=false)
        {
            //1.检验参数格式是否正确
             if(string.IsNullOrEmpty(UserName)||string.IsNullOrEmpty(UserPwd))
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "用户名或密码不能为空!"));
            }
            //2.判断验证码是否正确
            var codeId = CachedHelper.GetCookie(Setting.ValidateCodeLogin);
            string vc = CachedHelper.GetCached(codeId) as string;//获取验证码
            //比较验证码,忽略大小写
            if(!ValidateCode.Equals(vc, StringComparison.OrdinalIgnoreCase))
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "验证码不正确!"));
            }           
            //3.判断用户名密码是否正确
            string pwd = CommonHelper.MD5AppendStr(UserPwd);//密码加密
            var user = _bll.LoadEntities(u => u.UserName == UserName && u.UserPwd == pwd).SingleOrDefault();
            if(user==null)
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "用户名或密码不正确!"));
            }
            //用户状态是否正常
            if(user.State=="1")
            {
                return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Fail, "用户状态异常,请联系管理员!"));
            }
            //清除验证码
            CachedHelper.KeyDelete(codeId);
            CachedHelper.DelCookie(Setting.ValidateCodeLogin);

            //4.生成sessionId存到cookie
            string sessionId = Guid.NewGuid().ToString();
            //存储用户cookie信息
            CachedHelper.SetCookie(Setting.sessionId, sessionId);
            //是否记住密码
            if (isRemember)
            {
                CachedHelper.SetCached<WXL_Users>(sessionId, user);
            }
            else
            {
                CachedHelper.SetCached<WXL_Users>(sessionId, user, DateTime.Now.AddMinutes(20));
            }                    
            //to do
            return Json(MessageHelper.CreateReturnMsg(ReturnMsgCode.Sucess, "登录成功"));

        }
        #endregion
        #region 获取登录验证码
        //获取登录验证码
        public ActionResult ValidateCode()
        {
            //先删掉原来的验证码,以免占内存
            string oldCodeId = CachedHelper.GetCookie(Setting.ValidateCodeLogin);
            CachedHelper.KeyDelete(oldCodeId);

            //生成新验证码
             var ranDCode =CommonHelper.GetRandCode(4);
            //验证码cookieId
            var CodeId = Guid.NewGuid().ToString();
            //验证码保存两分钟
            CachedHelper.SetCookie(Setting.ValidateCodeLogin, CodeId, DateTime.Now.AddMinutes(2));
            //把验证码存到缓存
            CachedHelper.SetCached(CodeId, ranDCode,DateTime.Now.AddMinutes(2));
            //生成验证码图片
            var bmp = CommonHelper.GetValidateCode(ranDCode);
            //用流的方式显示验证码
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bmp.Dispose();
                return File(ms.ToArray(), "image/jpeg");
            }
        }
        #endregion
        #region 退出登录
        public ActionResult LogOut()
        {
            //获取cookie中用户登录时的key
            string sessionId = CachedHelper.GetCookie(Setting.sessionId);
            //从缓存中删除的登录信息
            CachedHelper.KeyDelete(sessionId);
            //跳转到登录页
            return RedirectToAction("Login");
        }
        #endregion
      

    }
}