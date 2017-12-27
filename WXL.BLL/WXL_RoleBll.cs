using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.IBLL;
using WXL.Model;

namespace WXL.BLL
{
    public partial class WXL_RoleBll : BaseBll<WXL_Role>, IWXL_RoleBll
    {
        //编写自有的方法
        public IQueryable<WXL_Role> GetRoleByUserId(int userId)
        {
           return this.CurrentDBSession.WXL_RoleDal.GetRoleByUserId(userId);
        }
    }
}
