 
using WXL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.IDAL
{
   
	
	public partial interface IWXL_ArticleDal :IBaseDal<WXL_Article>
    {
      
    }
	
	public partial interface IWXL_MENUDal :IBaseDal<WXL_MENU>
    {
      
    }
	
	public partial interface IWXL_RoleDal :IBaseDal<WXL_Role>
    {
      
    }
	
	public partial interface IWXL_RoleMenu_MidDal :IBaseDal<WXL_RoleMenu_Mid>
    {
      
    }
	
	public partial interface IWXL_TagDal :IBaseDal<WXL_Tag>
    {
      
    }
	
	public partial interface IWXL_UserMenu_MidDal :IBaseDal<WXL_UserMenu_Mid>
    {
      
    }
	
	public partial interface IWXL_UserRole_MidDal :IBaseDal<WXL_UserRole_Mid>
    {
      
    }
	
	public partial interface IWXL_UsersDal :IBaseDal<WXL_Users>
    {
      
    }
	
}