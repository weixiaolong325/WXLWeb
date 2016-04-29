using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WXLWeb.ViewModels;
using System.Data;
using System.Data.SqlClient;

namespace WXLWeb.DAL
{
    public class IndexDAL
    {
        public List<ArticleNew> GetArticleNews(string sql)
        {
            List<ArticleNew> articleNews = new List<ArticleNew>();
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(sql,CommandType.Text))
            {
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        ArticleNew articleNew = new ArticleNew();
                        articleNew.Id = sdr["ArticleId"].ToString();
                        articleNew.Title = sdr["Title"].ToString();
                        articleNews.Add(articleNew);
                    }
                }
                return articleNews;
            }
        }
    }
}