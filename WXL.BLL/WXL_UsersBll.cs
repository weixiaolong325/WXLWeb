using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.IBLL;
using WXL.Model;

namespace WXL.BLL
{
    public partial class WXL_UsersBll : BaseBll<WXL_Users>, IWXL_UsersBll
    {
        //实现抽象类
        //public override void SetCurrentDal()
        //{
        //    CurrentDal = this.CurrentDBSession.UsersDal;
        //}
        /// <summary>
        ///更改用户状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool UpdateUserState(List<int> ids)
        {
            //查出查修改的用户
            var userList = CurrentDBSession.WXL_UsersDal.LoadEntities(u => ids.Contains(u.Id));
            foreach(var user in userList)
            {
                if(user.State=="1")
                {
                    user.State = "0";
                }
                else
                {
                    user.State = "1";
                }
                //更改用户状态
                CurrentDBSession.WXL_UsersDal.EditEntity(user);
            }
            return CurrentDBSession.SaveChanges();
        }
    }
}
