using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WXL.Model;

namespace WXL.DAL
{

    public class DbContextFactory
    {
        /// <summary>
        /// 负责创建EF数据操作上下文实例,必须保证线程内唯一
        /// </summary>
        public static DbContext CreateContext()
        {
            DbContext dbcontext = (DbContext)CallContext.GetData("dbContext");
            if (dbcontext == null)
            {
                dbcontext = new qds178346566_dbEntities();
                CallContext.SetData("dbContext", dbcontext);
            }
            return dbcontext;
        }
    }
}
