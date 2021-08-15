using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaCodingForumBack.Models;

namespace YodaCodingForumBack.Controllers
{
    public class ReportController : Controller
    {
        private readonly ArticleDBContext _context;
        public ReportController(ArticleDBContext context)
        {
            _context = context;
        }

        // 未審核列表
        public IActionResult Index()
        {
            var ReportQuery = from Report in _context.Reports
                              orderby Report.CreateDate descending
                              select new ReportList
                              {
                                  ReportTargetId = Report.ReportTargetId,
                                  ReportId = Report.ReportId,
                                  UserAccount = (from u in _context.UserInfos
                                                 where u.UserId == Report.UserId
                                                 select u.UserAccount).First(),
                                  UserName = (from u in _context.UserInfos
                                              where u.UserId == Report.UserId
                                              select u.UserName).First(),
                                  ReasonCode = (from r in _context.ReportReasons
                                                where r.ReasonCode == Report.ReasonCode
                                                select r.ReasonName).First(),
                                  ReportTargetType = Report.ReportTargetType,
                                  ReportRemarks = Report.ReportRemarks,
                                  ReportContents = Report.ReportContents,
                                  ReportStatus = Report.ReportStatus,
                                  ReportVerifyPerson = Report.ReportVerifyPerson,
                                  CreateUser = Report.CreateUser,
                                  CreateDate = Report.CreateDate,
                                  LastupdateUser = Report.LastupdateUser,
                                  LastupdateDate = Report.LastupdateDate,
                                  Version = Report.Version

                              };


            var reportVM = new SearchClass
            {
                reportList = ReportQuery.ToList()
            };
            return View(reportVM);
        }
        // 細節
        public IActionResult ReportDetails()
        {
            var ReportID = HttpContext.Request.Query["rid"].ToString();

            var ReportQuery = from Report in _context.Reports
                              orderby Report.CreateDate descending
                              where Report.ReportId == ReportID
                              select new ReportList
                              {
                                  ReportId = Report.ReportId,
                                  UserAccount = (from u in _context.UserInfos
                                                 where u.UserId == Report.UserId
                                                 select u.UserAccount).First(),
                                  UserName = (from u in _context.UserInfos
                                              where u.UserId == Report.UserId
                                              select u.UserName).First(),
                                  ReasonCode = (from r in _context.ReportReasons
                                                where r.ReasonCode == Report.ReasonCode
                                                select r.ReasonName).First(),               
                                  ReportTargetType = Report.ReportTargetType,
                                  ReportTargetId = Report.ReportTargetId,
                                  ReportRemarks = Report.ReportRemarks,
                                  ReportContents = Report.ReportContents,
                                  ReportStatus = Report.ReportStatus,
                                  ReportVerifyPerson = Report.ReportVerifyPerson,
                                  CreateUser = Report.CreateUser,
                                  CreateDate = Report.CreateDate,
                                  LastupdateDate = Report.LastupdateDate,
                                  LastupdateUser = Report.LastupdateUser,
                                  Version = Report.Version
                              };

            var reportVM = new SearchClass
            {
                reportList = ReportQuery.ToList()
            };

            //取得檢舉ID 和 類別
            string TargetId = reportVM.reportList[0].ReportTargetId;
            string Targettype = reportVM.reportList[0].ReportTargetType;

            var TargetarticleID = "";
            var ReportedID = "";
            
            //檢舉類別:留言
            if (Targettype == "C")
            {
                TargetarticleID = (from C in _context.Comments
                                   where C.CommentId == TargetId
                                   join A in _context.Articles on C.ArticleId equals A.ArticleId
                                   select A.ArticleId).First();

                //被檢舉人
                ReportedID = (from c in _context.Comments
                              where c.CommentId == TargetId
                              select c.CreateUser).First();


                Targettype = (from c in _context.Articles
                              where c.ArticleId == TargetarticleID
                              select c.ArticleType).First();


            }
            //檢舉類別:回覆
            else if (Targettype == "R")
            {
                //找回覆的父階
                var TargetresponsesID = (from C in _context.Responses
                                         where C.ResponseId == TargetId
                                         select C.ParentId).First();

                //被檢舉人
                ReportedID = (from r in _context.Responses
                              where r.ResponseId == TargetId
                              select r.CreateUser).First();

                var a = (from ar in _context.Articles
                         where ar.ArticleId == TargetresponsesID
                         select ar.ArticleId).Count();

                var c = (from cer in _context.Comments
                         where cer.CommentId == TargetresponsesID
                         select cer.CommentId).Count();

                //父階為文章
                if ( a == 1 )
                {
                    Targettype = (from arid in _context.Articles
                                  where arid.ArticleId == TargetresponsesID
                                  select arid.ArticleType).First();
                    TargetarticleID = TargetresponsesID;
                }

                //父階為留言
                if ( c == 1 )
                {
                    Targettype = (from R in _context.Responses
                                  where R.ResponseId == TargetresponsesID
                                  join type in _context.Comments on R.ParentId equals type.CommentId
                                  join aid in _context.Articles on type.ArticleId equals aid.ArticleId
                                  select aid.ArticleType).First();

                    TargetarticleID = (from R in _context.Responses
                                       where R.ResponseId == TargetresponsesID
                                       join type in _context.Comments on R.ParentId equals type.CommentId
                                       join aid in _context.Articles on type.ArticleId equals aid.ArticleId
                                       select aid.ArticleId).First();
                }
            }
            //父階是文章
            else
            {
                //被檢舉人
                ReportedID = (from a in _context.Articles
                              where a.ArticleId == TargetId
                              select a.CreateUser).First();

                TargetarticleID = TargetId;

            }

            ViewBag.Targettype = Targettype;
            ViewBag.TargetarticleID = TargetarticleID;
            ViewBag.ReportedID = ReportedID;
            return View(reportVM);
        }

        //改變檢舉狀態
        [HttpPost]
        public ActionResult Review(string ReportId, string inlineRadioOptions, string rttype, string rttid)
        {
            //string text = ReportId + ","+ inlineRadioOptions;
            var rdchange = _context.Reports.Find(ReportId);
            rdchange.ReportStatus = inlineRadioOptions;
            rdchange.LastupdateUser = "SYS";
            rdchange.LastupdateDate = DateTime.Now;
            rdchange.Version++;

            if (inlineRadioOptions == "Y")
            {
                if (rttype == "A" || rttype == "Q")
                {
                    var change = _context.Articles.Find(rttid);
                    change.ArticleStatus = "D";
                    change.LastupdateUser = "SYS";
                    change.LastupdateDate = DateTime.Now;
                    change.Version++;
                }
                else if (rttype == "C")
                {
                    var change = _context.Comments.Find(rttid);
                    change.CommentStatus = "D";
                    change.LastupdateUser = "SYS";
                    change.LastupdateDate = DateTime.Now;
                    change.Version++;
                }
                else
                {
                    var change = _context.Responses.Find(rttid);
                    change.ResponseStatus = "D";
                    change.LastupdateUser = "SYS";
                    change.LastupdateDate = DateTime.Now;
                    change.Version++;
                }
            }

            _context.SaveChanges();
            return Redirect($"/Report/ReportDetails?rid={ReportId}");
        }
    }
}
