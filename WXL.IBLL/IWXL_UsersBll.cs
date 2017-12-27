using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.Model;

namespace WXL.IBLL
{
   public partial interface IWXL_UsersBll:IBaseBll<WXL_Users>
    {
        /// <summary>
        /// 更改用户状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        bool UpdateUserState(List<int> ids);
    }
}
