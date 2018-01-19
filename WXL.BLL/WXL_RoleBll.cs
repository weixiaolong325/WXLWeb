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
        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> Ids)
        {

            //查出要删除的数据
            var Roles = this.CurrentDBSession.WXL_RoleDal.LoadEntities(r => Ids.Contains(r.Id));
            //循环删除
            foreach(var role in Roles)
            {
                this.CurrentDBSession.WXL_RoleDal.DeleteEntity(role);
            }
            //提交删除
            return this.CurrentDBSession.SaveChanges();
        }
    }
}
