using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WXLWeb.Models;

namespace WXLWeb.ViewModels
{
    public class ArticleRead
    {
        public Article article { get; set; }
        public List<Tag> tags { get; set; }
    }
}