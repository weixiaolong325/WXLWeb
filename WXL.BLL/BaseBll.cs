using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WXL.DALFactory;
using WXL.IDAL;

namespace WXL.BLL
{
   public abstract class BaseBll<T> where T:class,new()
    {

        //创建会话
        public IDbSession CurrentDBSession
        {
            get { return SessionFactory.CreateDbSession(); }
        }
        //当前dal
        public IDAL.IBaseDal<T> CurrentDal { get; set; }
        //抽象方法
        public abstract void SetCurrentDal();
        public BaseBll()
        {
            SetCurrentDal();//子类一定要实现抽象方法,让CurrentDal赋值具体Dal
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
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
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntities<s>(pageIndex, pageSize, out totalCount, whereLambda, orderbyLambda, isAsc);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
             CurrentDal.DeleteEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
             CurrentDal.AddEntity(entity);
            CurrentDBSession.SaveChanges();
            return entity;
        }
    }
}
