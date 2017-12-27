using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WXL.IDAL;

namespace WXL.DALFactory
{
    //工厂类，用于创建DbSession
   public class SessionFactory
    {
        public static IDbSession CreateDbSession()
        {
            //保证线程内唯一
          IDbSession DbSession = (IDAL.IDbSession)CallContext.GetData("dbSession");
            if (DbSession == null)
            {
                DbSession = new DALFactory.DbSession();
                CallContext.SetData("dbSession", DbSession);
            }
            return DbSession;
        }
    }
}
