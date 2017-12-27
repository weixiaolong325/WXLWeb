using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.Model.ThirdModel
{
    //返回消息
   public class ReturnMsg
    {
       /// <summary>
       /// 返回代码
       /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 返回消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 反回数据
        /// </summary>
        public object Data { get; set; }
    }
}
