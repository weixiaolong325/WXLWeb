 
using WXL.DAL;
using WXL.IDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.DALFactory
{
	public partial class DbSession : IDbSession
    {
	
		private IWXL_ArticleDal _WXL_ArticleDal;
        public IWXL_ArticleDal WXL_ArticleDal
        {
            get
            {
                if(_WXL_ArticleDal == null)
                {
                    _WXL_ArticleDal = new WXL_ArticleDal();
                }
                return _WXL_ArticleDal;
            }
            set { _WXL_ArticleDal = value; }
        }
	
		private IWXL_MENUDal _WXL_MENUDal;
        public IWXL_MENUDal WXL_MENUDal
        {
            get
            {
                if(_WXL_MENUDal == null)
                {
                    _WXL_MENUDal = new WXL_MENUDal();
                }
                return _WXL_MENUDal;
            }
            set { _WXL_MENUDal = value; }
        }
	
		private IWXL_RoleDal _WXL_RoleDal;
        public IWXL_RoleDal WXL_RoleDal
        {
            get
            {
                if(_WXL_RoleDal == null)
                {
                    _WXL_RoleDal = new WXL_RoleDal();
                }
                return _WXL_RoleDal;
            }
            set { _WXL_RoleDal = value; }
        }
	
		private IWXL_RoleMenu_MidDal _WXL_RoleMenu_MidDal;
        public IWXL_RoleMenu_MidDal WXL_RoleMenu_MidDal
        {
            get
            {
                if(_WXL_RoleMenu_MidDal == null)
                {
                    _WXL_RoleMenu_MidDal = new WXL_RoleMenu_MidDal();
                }
                return _WXL_RoleMenu_MidDal;
            }
            set { _WXL_RoleMenu_MidDal = value; }
        }
	
		private IWXL_TagDal _WXL_TagDal;
        public IWXL_TagDal WXL_TagDal
        {
            get
            {
                if(_WXL_TagDal == null)
                {
                    _WXL_TagDal = new WXL_TagDal();
                }
                return _WXL_TagDal;
            }
            set { _WXL_TagDal = value; }
        }
	
		private IWXL_UserMenu_MidDal _WXL_UserMenu_MidDal;
        public IWXL_UserMenu_MidDal WXL_UserMenu_MidDal
        {
            get
            {
                if(_WXL_UserMenu_MidDal == null)
                {
                    _WXL_UserMenu_MidDal = new WXL_UserMenu_MidDal();
                }
                return _WXL_UserMenu_MidDal;
            }
            set { _WXL_UserMenu_MidDal = value; }
        }
	
		private IWXL_UserRole_MidDal _WXL_UserRole_MidDal;
        public IWXL_UserRole_MidDal WXL_UserRole_MidDal
        {
            get
            {
                if(_WXL_UserRole_MidDal == null)
                {
                    _WXL_UserRole_MidDal = new WXL_UserRole_MidDal();
                }
                return _WXL_UserRole_MidDal;
            }
            set { _WXL_UserRole_MidDal = value; }
        }
	
		private IWXL_UsersDal _WXL_UsersDal;
        public IWXL_UsersDal WXL_UsersDal
        {
            get
            {
                if(_WXL_UsersDal == null)
                {
                    _WXL_UsersDal = new WXL_UsersDal();
                }
                return _WXL_UsersDal;
            }
            set { _WXL_UsersDal = value; }
        }
	}	
}