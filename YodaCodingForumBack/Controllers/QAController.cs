using YodaCodingForumBack.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static YodaCodingForumBack.Controllers.ArticleController;

namespace YodaCodingForumBack.Controllers
{
    public class QAController : Controller
    {
        private readonly ArticleDBContext _context;
        public QAController(ArticleDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var qaQuery = from q in _context.Articles
                          where q.ArticleType == "Q"
                          orderby q.ArticleStatus ascending,q.CreateDate descending
                          select q;

            var qaVM = new SearchClass
            {
                articleList = qaQuery.ToList()
            };
            return View(qaVM);
        }

        public IActionResult QADetails()
        {
            var articleid = HttpContext.Request.Query["q"].ToString();

            var articleQuery = from a in _context.Articles
                               join ua in _context.UserInfos on a.CreateUser equals ua.UserAccount
                               where a.ArticleId == articleid
                               where a.ArticleType == "Q"
                               select new articleDetail
                               {
                                   //ArticleDetail
                                   ArticleId = articleid,
                                   ArticleTitle = a.ArticleTitle,
                                   ArticleContent = a.ArticleContent,
                                   ArticleViews = a.ArticleViews,
                                   ArticleLike = a.ArticleLike,
                                   ArticleStatus = a.ArticleStatus,
                                   Nickname = ua.UserNickname,
                                   ArticleUserID = ua.UserId,
                                   ArticleVersion = a.Version,
                                   CreateUser = a.CreateUser,
                                   CreateDate = a.CreateDate,
                                   LastupdateUser = a.LastupdateUser,
                                   LastupdateDate = a.LastupdateDate,
                                   ArticleImage = (from i in _context.UserImages
                                                   where i.UserId == ua.UserId
                                                   select i.UserId).Count() == 1 ?
                                                  (from i in _context.UserImages
                                                   where i.UserId == ua.UserId
                                                   select i.Uimage).First() :
                                                  (from i in _context.UserImages
                                                   where i.ImageId == "Defult" + ua.UserSax
                                                   select i.Uimage).First(),
                                   //ArticleTag
                                   ArticleTagList = (from t in _context.Tags
                                                     join tAnda in _context.ArticleAndTags on t.TagId equals tAnda.TagId
                                                     where tAnda.ArticleId == a.ArticleId
                                                     orderby t.CreateDate descending
                                                     select t).ToList(),
                                   //ArticleResponseCount
                                   ArticleResponseCount = (from r in _context.Responses
                                                           where r.ParentId == a.ArticleId
                                                           select r).Count(),
                                   //ArticleResponse
                                   ArticleResponseList = (from r in _context.Responses
                                                          where r.ParentId == a.ArticleId
                                                          orderby r.CreateDate
                                                          join u in _context.UserInfos on r.CreateUser equals u.UserAccount
                                                          select new Response_class
                                                          {
                                                              ResponseUserID = u.UserId,
                                                              ResponseAccount = r.CreateUser,
                                                              ResponseContent = r.ResponseContent,
                                                              ResponseID = r.ResponseId,
                                                              ResponseNickname = u.UserNickname,
                                                              ResponseParentID = r.ParentId,
                                                              ResponseStatus = r.ResponseStatus,
                                                              CreateDate = r.CreateDate,
                                                              ResponseParentClass = r.ParentClass,
                                                              ResponseImage = (from i in _context.UserImages
                                                                               where i.UserId == u.UserId
                                                                               select i.UserId).Count() == 1 ?
                                                                              (from i in _context.UserImages
                                                                               where i.UserId == u.UserId
                                                                               select i.Uimage).First() :
                                                                               (from i in _context.UserImages
                                                                                where i.ImageId == "Defult" + u.UserSax
                                                                                select i.Uimage).First(),
                                                          }).ToList(),
                                   ArticleCommentCount = (from c in _context.Comments
                                                          where c.ArticleId == a.ArticleId
                                                          select c).Count(),

                                   //ArticleComment
                                   ArticleCommentList = (from c in _context.Comments
                                                         where c.ArticleId == a.ArticleId
                                                         join u in _context.UserInfos on c.CreateUser equals u.UserAccount
                                                         orderby c.CreateDate
                                                         orderby c.CommentAnswer descending
                                                         select new ArticleComment
                                                         {
                                                             CommentUserID = u.UserId,
                                                             CommentAccount = c.CreateUser,
                                                             CreateDate = c.CreateDate,
                                                             CommentId = c.CommentId,
                                                             CommentVersion = c.Version,
                                                             LastUpdateDate = c.LastupdateDate,
                                                             CommentContent = c.CommentContent,
                                                             CommentStatus = c.CommentStatus,
                                                             CommentLike = c.CommentLike,
                                                             CommentNickname = u.UserNickname,
                                                             CommentAnswer = c.CommentAnswer,
                                                             CommentImage = (from i in _context.UserImages
                                                                             where i.UserId == u.UserId
                                                                             select i.UserId).Count() == 1 ?
                                                                             (from i in _context.UserImages
                                                                              where i.UserId == u.UserId
                                                                              select i.Uimage).First() :
                                                                              (from i in _context.UserImages
                                                                               where i.ImageId == "Defult" + u.UserSax
                                                                               select i.Uimage).First(),
                                                             //ArticleCommentResponse
                                                             CommentResponseList = (from r in _context.Responses
                                                                                    where r.ParentId == c.CommentId
                                                                                    orderby r.CreateDate
                                                                                    join u in _context.UserInfos on r.CreateUser equals u.UserAccount
                                                                                    select new Response_class
                                                                                    {
                                                                                        ResponseUserID = u.UserId,
                                                                                        ResponseAccount = r.CreateUser,
                                                                                        ResponseContent = r.ResponseContent,
                                                                                        ResponseID = r.ResponseId,
                                                                                        ResponseNickname = u.UserNickname,
                                                                                        ResponseParentID = r.ParentId,
                                                                                        ResponseStatus = r.ResponseStatus,
                                                                                        CreateDate = r.CreateDate,
                                                                                        ResponseParentClass = r.ParentClass,
                                                                                        ResponseImage = (from i in _context.UserImages
                                                                                                         where i.UserId == u.UserId
                                                                                                         select i.UserId).Count() == 1 ?
                                                                                                        (from i in _context.UserImages
                                                                                                         where i.UserId == u.UserId
                                                                                                         select i.Uimage).First() :
                                                                                                         (from i in _context.UserImages
                                                                                                          where i.ImageId == "Defult" + u.UserSax
                                                                                                          select i.Uimage).First()
                                                                                    }).ToList()
                                                         }).ToList()
                               };

            var articleDetail = new SearchClass
            {
                articleDetail = articleQuery.ToList().First()
            };
            return View(articleDetail);
        }

        public IActionResult Search(string searchString)
        {

            var qaQuery = from q in _context.Articles
                          where q.ArticleType == "Q"
                          orderby q.CreateDate descending
                          select q;

            if (!String.IsNullOrEmpty(searchString))
            {
                qaQuery = from q in _context.Articles
                          where q.ArticleType == "Q"
                          where q.ArticleTitle.Contains(searchString)
                          orderby q.CreateDate descending
                          select q;
            }

            var qaVM = new SearchClass
            {
                articleList = qaQuery.ToList()
            };

            return View(qaVM);
        }
    }
}
