using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WXLWeb.Models;

namespace WXLWeb.ViewModels
{
    public class AlterArticleView
    {
        public Article article { get; set; }
        public string tags { get; set; }
    }
}