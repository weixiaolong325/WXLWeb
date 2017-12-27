using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.IDAL
{
  public interface IBaseDal<T> where T:class,new()
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
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
         IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         bool DeleteEntity(T entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         bool EditEntity(T entity);
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T AddEntity(T entity);
    }
}
