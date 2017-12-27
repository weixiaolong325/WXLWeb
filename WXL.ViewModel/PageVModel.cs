using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXL.ViewModel
{
  public class PageVModel
    {
        /// <summary>
        /// 当前页数
        /// </summary>
        public  int PageIndex { get; set; }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public  int PageSize { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>       
        public int pageCount { get; set; }
      public  PageVModel()
        {
            //设置默认值
            PageIndex = 1;
            PageSize = 5;
        }
    }
}
