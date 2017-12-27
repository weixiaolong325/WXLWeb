using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public class Setting
    {
        /// <summary>
        /// 用户登录的Key
        /// </summary>
        public static string sessionId { get { return "sessionId"; } }  
        /// <summary>
        /// 登录验证码的Key
        /// </summary>
        public static string ValidateCodeLogin { get { return "ValidateCodeLogin"; } }
    }
}
