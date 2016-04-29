using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXLWeb.Models;
using System.Data;
using System.Data.SqlClient;
using WXLWeb.ViewModels;
using Common;
using System.Threading.Tasks;

namespace WXLWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            IndexView indexView = new IndexView();
            DAL.IndexDAL indexDal=new DAL.IndexDAL();
            //最新文章
            string sql_newArticle = "select top 10 ArticleId,Title from WXL_Article with (nolock) where Isdel=0 order by CreateTime desc";
            indexView.newArticles = indexDal.GetArticleNews(sql_newArticle);
            return View(indexView);
        }

        //留言板
        public ActionResult MessageBoard()
        {
            return View();
        }
        //关于我
        public ActionResult AboutMe()
        {
            return View();
        }
        //技术分享
        public ActionResult Skill(string page)
        {
            //默认第一页
            int p = 1;
            if (page != null)
            {
                int isint;
                if (!int.TryParse(page, out isint))
                {
                    return Content("查看内容不存在");
                }
                p = Convert.ToInt32(page) < 1 ? 1 : Convert.ToInt32(page);
            }
            return View(articleView(1,p,"/Home/Skill"));
        }
        //生活
        public ActionResult Life(string page)
        {
            //默认第一页
            int p = 1;
            if (page != null)
            {
                int isint;
                if (!int.TryParse(page, out isint))
                {
                    return Content("查看内容不存在");
                }
                p = Convert.ToInt32(page) < 1 ? 1 : Convert.ToInt32(page);
            }
            int articleType = (int)(ArticleTypeEnum.Life);
            return View(articleView(articleType, p, "/Home/Life"));
        }
        //学习
        public ActionResult Learn(string page)
        {
            //默认第一页
            int p = 1;
            if (page != null)
            {
                int isint;
                if (!int.TryParse(page, out isint))
                {
                    return Content("查看内容不存在");
                }
                p = Convert.ToInt32(page) <=1 ? 1 : Convert.ToInt32(page);
            }
            int articleType = Convert.ToInt32(ArticleTypeEnum.learn);
            return View(articleView(articleType, p, "/Home/Learn"));
        }
        //查看文章
        public ActionResult Article(string id)
        {
            ArticleRead articleRead = new ArticleRead();
            Article article = new Article();
            List<Tag> tags = new List<Tag>();
            if (string.IsNullOrEmpty(id))
            {
                return Content("出错啦!这里没有内容哦~");
            }
            string strId = id.ToLower().Replace(".html", "");
            int isint = 0;
            if (!int.TryParse(strId, out isint))
            {
                return Content("出错啦!这里没有内容哦~");
            }
            int articleId = Convert.ToInt32(strId);

            //1.把文章内容查出来
            string sql = "select a.ArticleId,a.Title,a.ContentTxt,a.Type1,a.Type2,a.CreateTime,a.UserId,a.UserName,a.LookNum, a.ArticleId2,b.TagId,b.TagName from WXL_Article a left join WXL_Tag b on a.ArticleId2=b.ArticleId2 and b.IsDel=0 where ArticleId=@ArticleId and a.Isdel=0 ";
            SqlParameter[] param = { new SqlParameter("@ArticleId", articleId) };
            DataTable dt = SQLHelper.Dt(sql, CommandType.Text, param);
            //判断文章是否存在
            if (dt.Rows.Count <= 0)
            {
                return Content("出错啦!这里没有内容哦~");
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //1.如果是第一行就读取文章内容和标签
                if (i == 0)
                {
                    Tag tag = new Tag();
                    article.ArticleId = Convert.ToInt32(dt.Rows[i]["ArticleId"]);
                    article.Title = dt.Rows[i]["Title"].ToString();
                    article.CreateTime = Convert.ToDateTime(dt.Rows[i]["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    ViewBag.ContentTxt =new MvcHtmlString(dt.Rows[i]["ContentTxt"].ToString());
                    article.UserId = dt.Rows[i]["UserId"].ToString();
                    article.UserName = dt.Rows[i]["UserName"].ToString();
                    article.LookNum = Convert.ToInt64(dt.Rows[i]["LookNum"]);
                    article.Type2 = dt.Rows[i]["Type2"].ToString();
                    if (dt.Rows[i]["TagId"] != DBNull.Value)
                        tag.TagId = Convert.ToInt32(dt.Rows[i]["TagId"]);
                    if (dt.Rows[i]["TagName"] != DBNull.Value)
                        tag.TagName = dt.Rows[i]["TagName"].ToString();
                    if(tag.TagName!=null)
                    tags.Add(tag);
                    //开一个线程执行文章阅读数+1
                    var task = Task.Run(() => { ArticleRaddNumAdd(article.ArticleId); });
                }
                //第二行起只读取标签
                else
                {
                    Tag tag = new Tag();
                    if (dt.Rows[i]["TagId"] != DBNull.Value)
                        tag.TagId = Convert.ToInt32(dt.Rows[i]["TagId"]);
                    if (dt.Rows[i]["TagName"] != DBNull.Value)
                        tag.TagName = dt.Rows[i]["TagName"].ToString();
                    if (tag.TagName != null)
                    tags.Add(tag);
                }
            }
            articleRead.article = article;
            articleRead.tags = tags;
            return View(articleRead);
        }
        /// <summary>
        /// 文章列表，返回一个文章集合
        /// </summary>
        /// <param name="type1">文章类别</param>
        /// <param name="pageNum">第几页</param>
        /// <returns></returns>
        private ArticleView articleView(int type1,int pageNum,string url)
        {
            ArticleView articleList = new ArticleView();
            List<Article> articles = new List<Article>();
            List<TagCount> tagCountList = new List<TagCount>();

            int linNum = 5;//一页显示多少条记录
            int delStaus=Convert.ToInt32(IsDelEnum.normal);
            string sql = string.Format("select COUNT(1) from WXL_Article where Type1={0} and Isdel={1}", type1,delStaus);
            //总条数
            int NumSum = Convert.ToInt32(SQLHelper.ExecuteScalar(sql, CommandType.Text));
            //总页数
            int pageNumSum = NumSum % linNum > 0 ? NumSum / linNum + 1 : NumSum / linNum;
            if (pageNum > pageNumSum) pageNum = pageNumSum;
            articleList.pageNumSum = pageNumSum;
            //当前页数
            articleList.pageNum = pageNum;
            SqlParameter[] param ={new SqlParameter("@LineNum",linNum),
                                     new SqlParameter("@pageNum",pageNum),
                                  new SqlParameter("@Type1",type1)};
            ViewBag.page = new MvcHtmlString(commHelper.page(pageNum, pageNumSum,url, "page", 5));
            //文章列表
            using (SqlDataReader sdr = SQLHelper.ExecuteReader("Long_ArticleToPage", CommandType.StoredProcedure, param))
            {
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        Article article = new Article();
                        article.ArticleId = Convert.ToInt32(sdr["ArticleId"]);
                        article.Title = sdr["Title"].ToString();
                        article.Type1 = Convert.ToInt32(sdr["Type1"]);
                        article.Type2 = sdr["Type2"].ToString();
                        article.CreateTime = Convert.ToDateTime(sdr["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        article.UserId = sdr["UserId"].ToString();
                        article.UserName = sdr["UserName"].ToString();
                        article.LookNum = Convert.ToInt32(sdr["LookNum"]);
                        article.Abstract = sdr["Abstract"].ToString();

                        articles.Add(article);
                    }
                }
            }
           
            //我的标签列表
            string sql_tagCount =string.Format("select top 10 a.TagName,COUNT(1) countnum from WXL_Tag a inner join WXL_Article b on a.ArticleId2=b.ArticleId2 and b.Type1={0} and a.IsDel=0 group by a.TagName order by countnum desc",type1);
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(sql_tagCount, CommandType.Text))
            {
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        TagCount tagCount = new TagCount();
                        tagCount.TagName = sdr["TagName"].ToString();
                        tagCount.CountNum = sdr["CountNum"].ToString();
                        tagCountList.Add(tagCount);
                    }
                }
            }
            articleList.Articles = articles;
            articleList.tagCounts = tagCountList;
            return articleList;
        }

        //文章阅读数+1
        private static void ArticleRaddNumAdd(int id)
        {
            string sql = "update WXL_Article set LookNum=LookNum+1 where ArticleId=@ArticleId";
            SqlParameter[] param = { new SqlParameter("@ArticleId", id) };
            SQLHelper.ExecuteNonQuery(sql, CommandType.Text, param);
        }
    }
}
