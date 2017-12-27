using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.Model.Enum;
using WXL.Model.ThirdModel;

namespace Common
{
   public class MessageHelper
    {
      
        /// <summary>
        /// 创建返回消息
        /// </summary>
        /// <param name="returnMsgCode">返回代码</param>
        /// <param name="Msg">返回消息</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static ReturnMsg CreateReturnMsg(ReturnMsgCode returnMsgCode,String Msg,Object data=null)
        {
            ReturnMsg returnMsg = new ReturnMsg();
            switch(returnMsgCode)
            {
                case ReturnMsgCode.Sucess:returnMsg.Code = "0";//成功 
                    break;
                case ReturnMsgCode.Fail:returnMsg.Code = "1"; //失败
                    break;
            }
            returnMsg.Msg = Msg;
            returnMsg.Data = data;
            return returnMsg;
        }
    }
}
