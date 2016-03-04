using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WXLWeb.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string ContentTxt { get; set; }
        public int Type1 { get; set; }
        public string Type2 { get; set; }
        public string CreateTime { get; set; }
        public DateTime AlterTime { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Tag { get; set; }
        public int LookNum { get; set; }
        public string Abstract { get; set; }
        public string ArticleId2 { get; set; }
    }
}