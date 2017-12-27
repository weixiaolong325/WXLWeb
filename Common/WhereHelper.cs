using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 把实体变成labda表达式
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public class WhereHelper<T> where T:class
    {
        private ParameterExpression param;
        private BinaryExpression filter;

        public WhereHelper()
        {
            param = Expression.Parameter(typeof(T), "c");

            //1==1
            Expression left = Expression.Constant(1);
            filter = Expression.Equal(left, left);
        }

        public Expression<Func<T, bool>> GetExpression()
        {
            return Expression.Lambda<Func<T, bool>>(filter, param);
        }

        /// <summary>
        /// 等于
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        public void Equal(string propertyName, object value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Equal(left, right);
            filter = Expression.And(filter, result);
        }

        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        public void Contains(string propertyName, string value)
        {
            Expression left = Expression.Property(param, typeof(T).GetProperty(propertyName));
            Expression right = Expression.Constant(value, value.GetType());
            Expression result = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
            filter = Expression.And(filter, result);
        }
    }
}
