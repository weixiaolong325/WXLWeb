using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WXLWeb.Models;
using System.Data;
using System.Data.SqlClient;
using Common;
using WXLWeb.ViewModels;

namespace WXLWeb.Controllers
{
            [Authentication]
    public class WebManagementController : Controller
    {
        //
        // GET: /WebManagement/
        /// <summary>
        /// 后台管理主界面
        /// </summary>
        /// <returns></returns>
        

        public ActionResult Index()
        {
            return View();
        }
        //增加文章
        public ActionResult AddArticle()
        {
            Article article = new Article();
            article.UserId = Session["UserId"].ToString();
           //3.查询用户名

            string sql = "select Name from  WXL_Users where UserId=@UserId";
            SqlParameter[] param = { new SqlParameter("@UserId", SqlDbType.VarChar, 20) { Value = article.UserId } };
            article.UserName = SQLHelper.ExecuteScalar(sql, CommandType.Text, param);
            return View(article);
        }
        //增加文章
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddArticle(Article article,FormCollection fc)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }
           article.UserId = Session["UserId"].ToString();
            //根据用户id获得用户名
            string name = commHelper.getUserName(article.UserId);
            if ( name!= null)
            {
                article.UserName=name;
            }
            else 
            article.UserName = "";
            article.ContentTxt = fc["content"];
            //文章摘要
            article.Abstract = Commonfun.ReplaceHtmlTag(article.ContentTxt, 150);
            //用时间戳生成文章ID2
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            article.ArticleId2=Convert.ToInt64(ts.TotalMilliseconds).ToString() + new Random().Next(100, 1000);
           
            string sql = "insert into WXL_Article(Title,ContentTxt,Type1,Type2,UserId,UserName,ArticleId2,Abstract) values(@Title,@ContentTxt,@Type1,@Type2,@UserId,@UserName,@ArticleId2,@Abstract)";
            SqlParameter[] param ={new SqlParameter("@Title",SqlDbType.NVarChar,30){Value=article.Title},
                                     new SqlParameter("@ContentTxt",SqlDbType.Text){Value=article.ContentTxt},
                                     new SqlParameter("@Type1",SqlDbType.Int){Value=article.Type1},
                                     new SqlParameter("@Type2",SqlDbType.NVarChar,20){Value=article.Type2},
                                     new SqlParameter("@UserId",SqlDbType.VarChar,20){Value=article.UserId},
                                     new SqlParameter("@UserName",SqlDbType.NVarChar,10){Value=article.UserName},
                                     new SqlParameter("@ArticleId2",SqlDbType.VarChar,20){Value=article.ArticleId2},
                                  new SqlParameter("@Abstract",SqlDbType.NVarChar,220){Value=article.Abstract} };
           
            
            //插入文章
            using (SqlTransaction tran = SQLHelper.BeginTransaction())
            {
                try {
                    SQLHelper.ExecuteNonQuery(sql, CommandType.Text, param,tran);
                     if (article.Tag != null)
                     {
                         List<string> str = article.Tag.Split(",，".ToCharArray()).ToList<string>();
                         foreach (var tag in str)
                         {
                             string sqlAddTag = "insert into WXL_Tag(ArticleId2,TagName) values(@ArticleId2,@TagName)";
                             SqlParameter[] paramTag ={new SqlParameter("@ArticleId2",article.ArticleId2),
                                                new SqlParameter("@TagName",tag)};
                             //插入标签
                             SQLHelper.ExecuteNonQuery(sqlAddTag, CommandType.Text, paramTag,tran);
                         }
                     }
                     tran.Commit();//提交事务
                     return Content("添加 成功");

                }
                catch {
                    tran.Rollback();//回滚
                    return View();
                }
            }
            //if (SQLHelper.ExecuteNonQuery(sql, CommandType.Text, param) > 0)
            //{
            //    //文章标签
            //    if (article.Tag != null)
            //    {
            //        List<string> str = article.Tag.Split(",，".ToCharArray()).ToList<string>();
            //        foreach (var tag in str)
            //        {
            //            string sqlAddTag = "insert into WXL_Tag(ArticleId2,TagName) values(@ArticleId2,@TagName)";
            //            SqlParameter[] paramTag ={new SqlParameter("@ArticleId2",article.ArticleId2),
            //                                    new SqlParameter("@TagName",tag)};
            //            //插入标签
            //            SQLHelper.ExecuteNonQuery(sqlAddTag, CommandType.Text, paramTag);
            //        }
            //    }
            //    return Content("添加成功!");
            //}
            //else
            //{
            //    return View();
            //}
        }

        //文章列表
        public ActionResult ArticleList(string page)
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
            return View(articleView(p, "/WebManagement/ArticleList"));
        }

        /// <summary>
        /// 文章列表，返回一个文章集合
        /// </summary>
        /// <param name="type1">文章类别</param>
        /// <param name="pageNum">第几页</param>
        /// <returns></returns>
        private ArticleView articleView(int pageNum, string url)
        {
            ArticleView articleList = new ArticleView();
            List<Article> articles = new List<Article>();

            int linNum = 15;//一页显示多少条记录
            string sql = string.Format("select COUNT(1) from WXL_Article where Isdel=0");
            //总条数
            int NumSum = Convert.ToInt32(SQLHelper.ExecuteScalar(sql, CommandType.Text));
            //总页数
            int pageNumSum = NumSum % linNum > 0 ? NumSum / linNum + 1 : NumSum / linNum;
            if (pageNum > pageNumSum) pageNum = pageNumSum;
            articleList.pageNumSum = pageNumSum;
            //当前页数
            articleList.pageNum = pageNum;
            SqlParameter[] param ={new SqlParameter("@LineNum",linNum),
                                     new SqlParameter("@pageNum",pageNum)
                                  };
            ViewBag.page = new MvcHtmlString(commHelper.page(pageNum, pageNumSum, url, "page", 5));
            //文章列表
            using (SqlDataReader sdr = SQLHelper.ExecuteReader("Long_ArticleToPageAllType", CommandType.StoredProcedure, param))
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
                        article.ArticleId2 = sdr["ArticleId2"].ToString();
                        article.UserId = sdr["UserId"].ToString();
                        article.UserName = sdr["UserName"].ToString();
                        article.LookNum = Convert.ToInt32(sdr["LookNum"]);
                        article.Abstract = sdr["Abstract"].ToString();

                        articles.Add(article);
                    }
                    articleList.Articles = articles;
                }
            }
            return articleList;
        }

        //修改文章
        public ActionResult AlterArticle(string id)
        {
            AlterArticleView alterArticleView = new AlterArticleView();
            Article article = new Article();
            if (id == null)
            {
                return Redirect("/ReWrite/Error.html");
            }
            string sql = "select ArticleId,Title,ContentTxt,Type1,Type2,Abstract,b.TagId,b.TagName,a.ArticleId2 from WXL_Article  a left join WXL_Tag b on a.ArticleId2=b.ArticleId2 and a.Isdel=0 and b.IsDel=0 where ArticleId=@ArticleId ";
            SqlParameter []param={new SqlParameter("@ArticleId",id)};
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(sql, CommandType.Text, param))
            {
                if (!sdr.HasRows)
                {
                    return Redirect("/ReWrite/Error.html");
                }
                else
                {
                    bool fistline = true;
                    while(sdr.Read())
                    {
   
                        //读取第一行
                        if (fistline == true)
                        {
                            fistline = false;
                            article.ArticleId = Convert.ToInt32(sdr["ArticleId"]);
                            article.Title = sdr["Title"].ToString();
                           ViewBag.content= new  MvcHtmlString(sdr["ContentTxt"].ToString());
                            article.Type1 = Convert.ToInt32(sdr["Type1"]);
                            article.Type2 = sdr["Type2"].ToString();
                            article.ArticleId2 = sdr["ArticleId2"].ToString();
                            //标签
                            if (sdr["TagName"] != null)
                            {
                                alterArticleView.tags= sdr["TagName"].ToString();
                            }
                        }
                        else 
                        {
                            //标签
                            if (sdr["TagName"] != null)
                            {
                                alterArticleView.tags+= ("," + sdr["TagName"].ToString());
                            }
                        }
                    }
                    alterArticleView.article = article;

                }
            }

            return View(alterArticleView);
        }
         [ValidateInput(false)]
        [HttpPost]
        public ActionResult AlterArticle(FormCollection fc,AlterArticleView av )
        {
            try
            {
                //1.获取文章内容
                string content = fc["content"];
                //文章摘要
                string Abstract = Commonfun.ReplaceHtmlTag(content, 150);
                //修改时间
                DateTime alterTime = DateTime.Now;
                //2.更新文章
                string sql = "update WXL_Article set Title=@Title,ContentTxt=@ContentTxt,Type1=@Type1,Type2=@Type2,AlterTime=@AlterTime,Abstract=@Abstract where ArticleId=@ArticleId";
                SqlParameter[] param ={new SqlParameter("@Title",av.article.Title),
                                       new SqlParameter("@ContentTxt",content),
                                       new SqlParameter ("@Type1",av.article.Type1),
                                       new SqlParameter("@Type2",av.article.Type2),
                                       new SqlParameter("@AlterTime",alterTime),
                                       new SqlParameter("@Abstract",Abstract),
                                       new SqlParameter("@ArticleId",av.article.ArticleId)};
                SQLHelper.ExecuteNonQuery(sql, CommandType.Text, param);
                //获取标签Tag
                List<string> strTag = av.tags.Split(",，".ToCharArray(),StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                if (strTag.Count > 0)
                {
                    string sqlTag = "select TagId,ArticleId2,TagName from WXL_Tag where ArticleId2=@ArticleId2";
                    SqlParameter[] paramTag = { new SqlParameter("@ArticleId2", av.article.ArticleId2) };
                    using (SqlDataReader sdr = SQLHelper.ExecuteReader(sqlTag, CommandType.Text, paramTag))
                    {
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                string TagNow = string.Empty;
                                bool isHave = false;
                                string tagName = sdr["TagName"].ToString();
                                //循环看是否能匹配到,匹配不到则删除
                                foreach (var str in strTag)
                                {
                                    //能匹配到
                                    if (tagName == str)
                                    {
                                        TagNow = str;
                                        isHave = true;
                                        break;
                                    }
                                }
                                //匹配到,则前端传进来的标签移除一个
                                if (isHave == true)
                                {
                                    strTag.Remove(TagNow);
                                }
                                else
                                {
                                    //匹配不到，则数据库里删除这个标签
                                    string sql_TagDel = "delete WXL_Tag where ArticleId2=@ArticleId2 and TagName=@TagName";
                                    SqlParameter[] param_TagDel ={
                                                   new SqlParameter("@ArticleId2",av.article.ArticleId2),
                                                   new SqlParameter("@TagName",TagNow)};
                                    SQLHelper.ExecuteNonQuery(sql_TagDel,CommandType.Text,param_TagDel);
                                }
                            }
                            //剩下的标签都是新增加的
                            if (strTag.Count > 0)
                            {
                                foreach (var str in strTag)
                                {
                                    string sql_tagAdd = "insert into WXL_Tag(ArticleId2,TagName) values(@ArticleId2,@TagName)";
                                    SqlParameter[] param_tagAdd ={
                                          new SqlParameter("@ArticleId2",av.article.ArticleId2),
                                          new SqlParameter("@TagName",str)};
                                    SQLHelper.ExecuteNonQuery(sql_tagAdd, CommandType.Text, param);
                                }
                            }
                        }
                    }
                }
                else
                {
                    string sqlTag = "delete WXL_Tag where ArticleId2=@ArticleId2";
                    SqlParameter[] paramTag = { new SqlParameter("@ArticleId2", av.article.ArticleId2) };
                    SQLHelper.ExecuteNonQuery(sqlTag,CommandType.Text,paramTag);
                }
                return Content("修改成功");
            }
            catch 
            {
                return Redirect("/ReWrite/Error.html");
            }
        }

                /// <summary>
                /// 删除文章
                /// </summary>
                /// <param name="id"></param>
                /// <returns></returns>
         public ActionResult DeleteArticle(string id)
         {
             string id2 = id;
             SqlParameter[] param={new SqlParameter("@ArticleId2",id2)};
             try {
                 string result=SQLHelper.ExecuteScalar("DeleteAritcle", CommandType.StoredProcedure, param);
                 if (result=="0")
                 {
                     return Content("删除失败");
                 }
                 return RedirectToAction("ArticleList");
             }
             catch 
             {
                 return Content("删除失败");
             }
            
         }
    }
}
