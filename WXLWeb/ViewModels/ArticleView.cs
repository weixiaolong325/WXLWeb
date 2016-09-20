using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WXLWeb.Models;

namespace WXLWeb.ViewModels
{
    public class ArticleView
    {
        public List<Article> Articles { get; set; }
        //第几页
        public int pageNum { get; set; }
        //总页数
        public int pageNumSum { get; set; }
        //我的标签
        public IEnumerable<TagCount> tagCounts { get; set; }

    }
}