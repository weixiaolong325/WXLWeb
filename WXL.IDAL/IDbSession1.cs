 

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.IDAL
{
	public partial interface IDbSession
    {

	
		IWXL_ArticleDal WXL_ArticleDal{get;set;}
	
		IWXL_MENUDal WXL_MENUDal{get;set;}
	
		IWXL_RoleDal WXL_RoleDal{get;set;}
	
		IWXL_RoleMenu_MidDal WXL_RoleMenu_MidDal{get;set;}
	
		IWXL_TagDal WXL_TagDal{get;set;}
	
		IWXL_UserMenu_MidDal WXL_UserMenu_MidDal{get;set;}
	
		IWXL_UserRole_MidDal WXL_UserRole_MidDal{get;set;}
	
		IWXL_UsersDal WXL_UsersDal{get;set;}
	}	
}