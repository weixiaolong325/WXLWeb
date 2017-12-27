using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.Model;

namespace WXL.IDAL
{
    public partial interface IWXL_RoleDal : IBaseDal<WXL_Role>
    {
        /// <summary>
        /// 根据用户id查找对应角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IQueryable<WXL_Role> GetRoleByUserId(int userId);
    }
}
