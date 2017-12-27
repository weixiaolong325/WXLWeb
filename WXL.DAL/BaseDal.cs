using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.DAL
{
   public class BaseDal<T> where T:class,new()
    {
        //获取数据库上下文
        public DbContext Db = DbContextFactory.CreateContext();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where(whereLambda);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="s">排序类型</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderbyLambda">排序字段</param>
        /// <param name="isAsc">true-升序,false-降序</param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int pageIndex,int pageSize,out int totalCount,System.Linq.Expressions.Expression<Func<T,bool>> whereLambda,System.Linq.Expressions.Expression<Func<T,s>> orderbyLambda,bool isAsc)
        {
            var temp = Db.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if(isAsc)//升序
            {
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else//降序
            {
                temp=temp.OrderByDescending<T,s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditEntity(T entity)
        {
            Db.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            return true;
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
           Db.Set<T>().Add(entity);
            return entity;
        }
    }
}
