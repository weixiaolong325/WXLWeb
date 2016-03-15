using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WXLWeb.Models
{
    public class Tag
    {
        public  int TagId{get;set;}
        public string ArticleId2{get;set;}
        public string TagName { get; set; }
        public DateTime CreateTime { get; set; }
        public string ArticleType2 { get; set; }
    }
}