using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.Model;

namespace WXL.IBLL
{
  public partial interface IWXL_RoleBll : IBaseBll<WXL_Role>
    {
        /// <summary>
        /// 根据用户id查角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IQueryable<WXL_Role> GetRoleByUserId(int userId);
        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        bool DeleteEntities(List<int> Ids);
    }
}
