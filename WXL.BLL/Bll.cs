 
using WXL.Model;
using WXL.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.BLL
{
   
	
	public partial  class WXL_ArticleBll :BaseBll<WXL_Article>,IWXL_ArticleBll
    {
      //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_ArticleDal;
        }
    }
	
	public partial  class WXL_MENUBll :BaseBll<WXL_MENU>,IWXL_MENUBll
    {
      //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_MENUDal;
        }
    }
	
	public partial  class WXL_RoleBll :BaseBll<WXL_Role>,IWXL_RoleBll
    {     
        //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_RoleDal;
        }
    }
	
	public partial  class WXL_RoleMenu_MidBll :BaseBll<WXL_RoleMenu_Mid>,IWXL_RoleMenu_MidBll
    {
      //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_RoleMenu_MidDal;
        }
    }
	
	public partial  class WXL_TagBll :BaseBll<WXL_Tag>,IWXL_TagBll
    {
      //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_TagDal;
        }
    }
	
	public partial  class WXL_UserMenu_MidBll :BaseBll<WXL_UserMenu_Mid>,IWXL_UserMenu_MidBll
    {
      //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_UserMenu_MidDal;
        }
    }
	
	public partial  class WXL_UserRole_MidBll :BaseBll<WXL_UserRole_Mid>,IWXL_UserRole_MidBll
    {
      //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_UserRole_MidDal;
        }
    }
	
	public partial  class WXL_UsersBll :BaseBll<WXL_Users>,IWXL_UsersBll
    {
      //实现抽象类
        public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDBSession.WXL_UsersDal;
        }
    }
	
}