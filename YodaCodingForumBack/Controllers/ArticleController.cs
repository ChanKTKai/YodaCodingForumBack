using YodaCodingForumBack.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleDBContext _context;
        public ArticleController(ArticleDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var articleQuery = from a in _context.Articles
                               where a.ArticleType == "A"
                               orderby a.ArticleStatus descending,a.CreateDate descending
                               select a;



            var articleVM = new SearchClass
            {
                articleList = articleQuery.ToList()
            };

            return View(articleVM);

        }

        public IActionResult ArticleDetails()
        {
            var articleid = HttpContext.Request.Query["a"].ToString();

            var articleQuery = from a in _context.Articles
                               join ua in _context.UserInfos on a.CreateUser equals ua.UserAccount
                               where a.ArticleId == articleid
                               where a.ArticleType == "A"
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

        [HttpPost]
        public IActionResult changeStatus(string TargetID, string ArticleID, string status, string ArticleType, string TargetType)
        {

            switch (TargetType)
            {
                case "A":
                    var changeTargetA = _context.Articles.Find(TargetID);
                    if (status == "D")
                    {
                        changeTargetA.ArticleStatus = "D";
                    }
                    else if (status == "N")
                    {
                        changeTargetA.ArticleStatus = "N";
                    }

                    else if (status == "F")
                    {
                        changeTargetA.ArticleStatus = "F";
                    }
                    else
                    {
                        changeTargetA.ArticleStatus = "A";
                    }

                    changeTargetA.LastupdateUser = "SYS";
                    changeTargetA.LastupdateDate = DateTime.Now;
                    changeTargetA.Version++;
                    _context.SaveChanges();
                    if (ArticleType == "A")
                    {

                        return Redirect($"/article/articledetails?a={ArticleID}");
                    }
                    else
                    {
                        return Redirect($"/qa/qadetails?q={ArticleID}");
                    }
                case "AR":
                    var changeTargetAR = _context.Responses.Find(TargetID);
                    if (status == "D")
                    {
                        changeTargetAR.ResponseStatus = "D";
                    }
                    else
                    {
                        changeTargetAR.ResponseStatus = "N";
                    }


                    changeTargetAR.LastupdateUser = "SYS";
                    changeTargetAR.LastupdateDate = DateTime.Now;
                    changeTargetAR.Version++;
                    _context.SaveChanges();
                    if (ArticleType == "A")
                    {

                        return Redirect($"/article/articledetails?a={ArticleID}");
                    }
                    else
                    {
                        return Redirect($"/qa/qadetails?q={ArticleID}");
                    }
                case "C":
                    var changeTargetC = _context.Comments.Find(TargetID);
                    if (status == "D")
                    {
                        changeTargetC.CommentStatus = "D";
                    }
                    else
                    {
                        changeTargetC.CommentStatus = "N";
                    }


                    changeTargetC.LastupdateUser = "SYS";
                    changeTargetC.LastupdateDate = DateTime.Now;
                    changeTargetC.Version++;
                    _context.SaveChanges();
                    if (ArticleType == "A")
                    {

                        return Redirect($"/article/articledetails?a={ArticleID}");
                    }
                    else
                    {
                        return Redirect($"/qa/qadetails?q={ArticleID}");
                    }
                case "CR":
                    var changeTargetCR = _context.Responses.Find(TargetID);
                    if (status == "D")
                    {
                        changeTargetCR.ResponseStatus = "D";
                    }
                    else
                    {
                        changeTargetCR.ResponseStatus = "N";
                    }


                    changeTargetCR.LastupdateUser = "SYS";
                    changeTargetCR.LastupdateDate = DateTime.Now;
                    changeTargetCR.Version++;
                    _context.SaveChanges();
                    if (ArticleType == "A")
                    {

                        return Redirect($"/article/articledetails?a={ArticleID}");
                    }
                    else
                    {
                        return Redirect($"/qa/qadetails?q={ArticleID}");
                    }
                default:
                    return Content("False");
            }
        }

        public IActionResult Search(string searchString)
        {


            var articleQuery = from a in _context.Articles
                               where a.ArticleType == "A"
                               orderby a.ArticleStatus descending, a.CreateDate descending
                               select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                articleQuery = from a in _context.Articles
                               where a.ArticleType == "A"
                               where a.ArticleTitle.Contains(searchString)
                               orderby a.ArticleStatus descending, a.CreateDate descending
                               select a;
            }


            var articleVM = new SearchClass
            {
                articleList = articleQuery.ToList()
            };

            return View(articleVM);
        }


        public class ArticleComment
        {
            public string CommentAccount { get; set; }
            public int CommentVersion { get; set; }
            public DateTime LastUpdateDate { get; set; }
            public DateTime CreateDate { get; set; }
            public string CommentId { get; set; }
            public string CommentContent { get; set; }
            public string CommentStatus { get; set; }
            public string CommentNickname { get; set; }
            public int CommentCollections { get; set; }
            public int CommentLike { get; set; }
            public string CommentAnswer { get; set; }
            public string CommentUserID { get; set; }
            public int CommentResponseCount { get; set; }
            public List<Response_class> CommentResponseList { get; set; }
            public byte[] CommentImage { get; set; }

        }
        public class Response_class
        {
            public string ResponseAccount { get; set; }
            public DateTime CreateDate { get; set; }
            public string ResponseID { get; set; }
            public string ResponseContent { get; set; }
            public string ResponseStatus { get; set; }
            public string ResponseNickname { get; set; }
            public string ResponseParentID { get; set; }
            public string ResponseParentClass { get; set; }
            public string ResponseUserID { get; set; }
            public byte[] ResponseImage { get; set; }
        }

        public class articleDetail
        {
            public string ArticleId { get; set; }

            public string ArticleUserID { get; set; }
            public string ArticleTitle { get; set; }
            public string ArticleContent { get; set; }
            public int ArticleViews { get; set; }
            public int ArticleLike { get; set; }
            public List<int> ArticleCollections { get; set; }
            public string ArticleStatus { get; set; }
            public string Nickname { get; set; }
            public int ArticleVersion { get; set; }
            public string CreateUser { get; set; }
            public DateTime CreateDate { get; set; }
            public string LastupdateUser { get; set; }
            public DateTime LastupdateDate { get; set; }
            public byte[] ArticleImage { get; set; }
            public List<Tag> ArticleTagList { get; set; }
            public int ArticleResponseCount { get; set; }
            public int ArticleCommentCount { get; set; }
            public List<Response_class> ArticleResponseList { get; set; }
            public List<ArticleComment> ArticleCommentList { get; set; }

        };

    }
}
