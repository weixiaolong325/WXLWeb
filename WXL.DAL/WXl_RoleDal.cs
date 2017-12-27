using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.IDAL;
using WXL.Model;

namespace WXL.DAL
{
    public partial class WXL_RoleDal : BaseDal<WXL_Role>, IWXL_RoleDal
    {
        /// <summary>
        /// 根据用户id获取角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>       
        public IQueryable<WXL_Role> GetRoleByUserId(int userId)
        {
            var query = from r in Db.Set<WXL_Role>()
                        join rm in Db.Set<WXL_UserRole_Mid>()
                        on r.Id equals rm.RoleId
                        where rm.UserId == userId                     
                        select r;
            return query;
        }
    }
}
