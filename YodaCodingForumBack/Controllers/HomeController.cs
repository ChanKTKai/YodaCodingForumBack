using YodaCodingForumBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Controllers
{
    public class HomeController : Controller
    {

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly ArticleDBContext _context;
        public HomeController(ArticleDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            DateTime today = DateTime.Today;
            DateTime befor7day = today.AddDays(-7);

            //今日發問
            var todayQ = (from q in _context.Articles
                          where q.ArticleType == "Q"
                          where q.CreateDate.Date == today
                          select q).Count();

            //今日發文
            var todayA = (from a in _context.Articles
                          where a.ArticleType == "A"
                          where a.CreateDate.Date == today
                          select a).Count();

            //本周會員(目前是全部會員)
            var todayM = (from m in _context.UserInfos
                          select m).Count();

            //最新的前10篇文章List
            var articleQuery = (from a in _context.Articles
                                where a.ArticleType == "A"
                                where a.ArticleStatus != "D"
                                orderby a.CreateDate descending
                                select a).Take(10).ToList();
            //使用率前6名的Tag
            var TagQuery = (from a in _context.Articles
                            join atag in _context.ArticleAndTags on a.ArticleId equals atag.ArticleId
                            where a.ArticleStatus != "D"
                            select new tagUseCount
                            {
                                tagID = atag.TagId,
                                tagName = (from t in _context.Tags
                                           where t.TagId == atag.TagId
                                           select t.TagName).First(),
                                UseCount = (from a in _context.Articles
                                            join atag2 in _context.ArticleAndTags on a.ArticleId equals atag2.ArticleId
                                            where a.ArticleStatus != "D"
                                            where atag2.TagId == atag.TagId
                                            select atag2.TagId).Count()
                            }).Distinct().OrderByDescending(t => t.UseCount).Take(6).ToList();

            //問題總數
            var qCount = (from q in _context.Articles
                                 where q.ArticleType == "Q"
                                 where q.ArticleStatus != "D"
                                 select q).Count();

            //已解決問題
            var finichQCount = (from q in _context.Articles
                                 where q.ArticleType == "Q"
                                 where q.ArticleStatus == "F"
                                 select q).Count();

            //文章總數
            var aCount = (from a in _context.Articles
                          where a.ArticleType == "A"
                          where a.ArticleStatus != "D"
                          select a).Count();

            //有效文章和問題總數
            var aqCount = (from aq in _context.Articles
                           where aq.ArticleStatus != "D"
                           select aq).Count();

            //累計瀏覽數(文章&問答)
            var viewCount = (from a in _context.Articles
                             select a.ArticleViews).Sum();

            //留言總數
            var cCount = (from c in _context.Comments
                          where c.CommentStatus == "N"
                          select c).Count();

            //長條圖使用率顯示
            //1.會員/目標100
            double mp = Convert.ToDouble(todayM) / Convert.ToDouble(100);
            string Mperc = string.Format("{0:0.00%}", mp);
            //2.解決/總問題
            double pp = Convert.ToDouble(finichQCount) / Convert.ToDouble(qCount);
            string Pperc = string.Format("{0:0.00%}", pp);
            //3.累計瀏覽/目標2000
            double vp = Convert.ToDouble(viewCount) / Convert.ToDouble(2000);
            string VPerc = string.Format("{0:0.00%}", vp);
            //4.文章數/目標50
            double ap = Convert.ToDouble(aCount) / Convert.ToDouble(50);
            string Aperc = string.Format("{0:0.00%}", ap);
            List<string> Bar1 = new List<string>() { Mperc,Pperc,VPerc,Aperc};


            //取得Tag總使用數
            List<string> tagPerc = new List<string>();
            int tagUseSum = (from ts in _context.ArticleAndTags
                             select ts).Count();
                             
            //Tag使用率
            for (var i = 0; i< TagQuery.Count; i++)
            {
                int uc = TagQuery[i].UseCount;
                double result = Convert.ToDouble(uc) / Convert.ToDouble(tagUseSum);
                string Tperc = string.Format("{0:0.0%}", result);
                tagPerc.Add(Tperc);
            }
            
            var homeVm = new HomeVM
            {
                todayQ = todayQ,
                todayA = todayA,
                todayM = todayM,
                articles = articleQuery,
                taguse = TagQuery,
                articleViewCount = viewCount,
                questionCount = qCount,
                articleCount = aCount,
                aAndQCount = aqCount,
                finichQuestionCount = finichQCount,
                commendCount = cCount,
                bar1 = Bar1,
                peiPrec = tagPerc
            };

            var indexVM = new SearchClass
            {
                HomeVMs = homeVm
            };


            return View(indexVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
