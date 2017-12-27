 
using WXL.Model;
using WXL.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.DAL
{
   
	
	public partial  class WXL_ArticleDal :BaseDal<WXL_Article>,IWXL_ArticleDal
    {
      
    }
	
	public partial  class WXL_MENUDal :BaseDal<WXL_MENU>,IWXL_MENUDal
    {
      
    }
	
	public partial  class WXL_RoleDal :BaseDal<WXL_Role>,IWXL_RoleDal
    {
      
    }
	
	public partial  class WXL_RoleMenu_MidDal :BaseDal<WXL_RoleMenu_Mid>,IWXL_RoleMenu_MidDal
    {
      
    }
	
	public partial  class WXL_TagDal :BaseDal<WXL_Tag>,IWXL_TagDal
    {
      
    }
	
	public partial  class WXL_UserMenu_MidDal :BaseDal<WXL_UserMenu_Mid>,IWXL_UserMenu_MidDal
    {
      
    }
	
	public partial  class WXL_UserRole_MidDal :BaseDal<WXL_UserRole_Mid>,IWXL_UserRole_MidDal
    {
      
    }
	
	public partial  class WXL_UsersDal :BaseDal<WXL_Users>,IWXL_UsersDal
    {
      
    }
	
}