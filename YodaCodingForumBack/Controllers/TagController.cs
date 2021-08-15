using YodaCodingForumBack.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace YodaCodingForumBack.Controllers
{
    public class TagController : Controller
    {
        private readonly ArticleDBContext _context;
        public TagController(ArticleDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tagQuery = from tag in _context.Tags
                           orderby tag.CreateDate descending
                           select new TagList
                           {
                               tid = tag.TagId,
                               name = tag.TagName,
                               createdate = tag.CreateDate,
                               createuser = tag.CreateUser,
                               followcount= (from f in _context.UserFollowTags
                                             where f.TagId == tag.TagId
                                             select f).Count(),
                               usecount = (from ta in _context.ArticleAndTags
                                           where ta.TagId == tag.TagId
                                           select ta).Count(),
                               alsocount = (from tt in _context.Tagalsos
                                            where tt.TagId == tag.TagId
                                            select tt).Count()
                           };

            var tagVM = new SearchClass
            {
                tagList = tagQuery.ToList()
            };
            return View(tagVM);
        }

        public IActionResult Search(string searchString)
        {
            var tagQuery = from tag in _context.Tags
                           orderby tag.CreateDate descending
                           select new TagList
                           {
                               tid = tag.TagId,
                               name = tag.TagName,
                               createdate = tag.CreateDate,
                               createuser = tag.CreateUser,
                               followcount = (from f in _context.UserFollowTags
                                              where f.TagId == tag.TagId
                                              select f).Count(),
                               usecount = (from ta in _context.ArticleAndTags
                                           where ta.TagId == tag.TagId
                                           select ta).Count(),
                               alsocount = (from tt in _context.Tagalsos
                                            where tt.TagId == tag.TagId
                                            select tt).Count()
                           };

            if (!String.IsNullOrEmpty(searchString))
            {
                tagQuery = tagQuery.Where(t => t.name.Contains(searchString.Replace(" ", "")));
            }

            var tagVM = new SearchClass
            {
                tagList = tagQuery.ToList()
            };

            return View(tagVM);
        }

        public IActionResult Tagalso()
        {
            var tagId = HttpContext.Request.Query["tag"].ToString();
            if (String.IsNullOrEmpty(tagId))
            {

                return View();

            }
            else
            {
                var tag = from t in _context.Tags
                          where t.TagId == tagId
                          select new TagAndTagAlso
                          {
                              TagID = tagId,
                              TagName = t.TagName,
                              CreateUser = t.CreateUser,
                              CreateDate = t.CreateDate,
                              LastupdateUser = t.LastupdateUser,
                              LastupdateDate = t.LastupdateDate,
                              Version = t.Version,
                              TagAlsoList = (from tA in _context.Tagalsos
                                             where tA.TagId == t.TagId
                                             select tA).ToList(),
                          };


                var tagList = new SearchClass
                {
                    tagDetailList = tag.ToList()
                };
                return View(tagList);
            }
        }
        //更新舊TAG的TagAlso
        [HttpPost]

        public ActionResult oldTagAddEditTagAlso(string oldTagAddEditTagAlso, string oldTagID, string oldTagAlsoID)
        {
            //原本TagAlso名稱
            var orginalTagAlsoNameList = (from ta in _context.Tagalsos
                                          where ta.TagalsoId == oldTagAlsoID
                                          select ta.TagalsoName).ToList();
            var orginalTagAlsoName = orginalTagAlsoNameList[0];


            if (oldTagAddEditTagAlso == oldTagID)
            {
                return Redirect($"/tag/tagalso?tag={oldTagID}");
            }
            else
            {
                var TagAlso = _context.Tagalsos.Find(oldTagAlsoID);
                TagAlso.TagalsoName = oldTagAddEditTagAlso;
                TagAlso.Version++;
                TagAlso.LastupdateUser = "SYS";
                TagAlso.LastupdateDate = DateTime.Now;
                _context.SaveChanges();
                return Redirect($"/tag/tagalso?tag={oldTagID}");
            }
        }

        //新增舊TAG的TagAlso
        [HttpPost]
        public ActionResult oldTagAddNewTagAlso(string oldTagAddNewTagAlso, string TagID)
        {


            var tagAlsoNameIsRepeated = (from ta in _context.Tagalsos
                                         where ta.TagId == TagID
                                         where ta.TagalsoName == oldTagAddNewTagAlso
                                         select ta.TagalsoName.ToList()).Count();
            //判斷是否重複
            if (tagAlsoNameIsRepeated > 0)
            {
                return Redirect($"/tag/tagalso?tag={TagID}");

            }

            else
            {

                //新增tagAlso
                Tagalso newTagAlso = new Tagalso
                {
                    TagId = TagID,
                    TagalsoName = oldTagAddNewTagAlso,
                    CreateDate = DateTime.Now,
                    CreateUser = "SYS",
                    LastupdateDate = DateTime.Now,
                    LastupdateUser = "SYS"
                };
                _context.Tagalsos.Add(newTagAlso);
                _context.SaveChanges();

                return Redirect($"/tag/tagalso?tag={TagID}");
            }

        }
        //刪除TagAlso
        public IActionResult tagAlsoDelete()
        {
            //欲刪除TagAlsoID
            var tagAlsoID = HttpContext.Request.Query["taid"].ToString();

            //欲刪除TagAlso之主TagId
            var tagIDList = from t in _context.Tagalsos
                            where t.TagalsoId == tagAlsoID
                            select t.TagId;
            var tagID = tagIDList.ToList()[0];

            //刪除TagAlso
            var DeleteTagAlso = _context.Tagalsos.Find(tagAlsoID);
            _context.Tagalsos.Remove(DeleteTagAlso);
            _context.SaveChanges();

            return Redirect($"/tag/tagalso?tag={tagID}");
        }
        //刪除Tag
        public IActionResult tagDelete()
        {
            //欲刪除TagAlsoID
            var tagID = HttpContext.Request.Query["tag"].ToString();



            //刪除Tag
            var DeleteTag = _context.Tags.Find(tagID);
            _context.Tags.Remove(DeleteTag);

            //刪除Tag之TagAlso
            var deleteTagAlsoList = (from ta in _context.Tagalsos
                                     where ta.TagId == tagID
                                     select ta.TagalsoId).ToList();
            foreach (var deleteTagAlso in deleteTagAlsoList)
            {
                var TagAlso = _context.Tagalsos.Find(deleteTagAlso);
                _context.Tagalsos.Remove(TagAlso);
            }
            _context.SaveChanges();

            return Redirect($"/tag/index");
        }

        //更新TagName
        public IActionResult editTagName(string newTagName, string TagID)
        {
            //原本的tagName
            var originalTagNameList = (from t in _context.Tags
                                       where t.TagId == TagID
                                       select t.TagName).ToList();
            var originalTagName = originalTagNameList[0];

            if (originalTagName == newTagName)
            {
                return Redirect($"/tag/tagalso?tag={TagID}");
            }
            else
            {
                var Tag = _context.Tags.Find(TagID);
                Tag.TagName = newTagName;
                Tag.LastupdateUser = "SYS";
                Tag.LastupdateDate = DateTime.Now;
                Tag.Version++;
                _context.SaveChanges();
                return Redirect($"/tag/tagalso?tag={TagID}");
            }
        }

        public ActionResult addNewTag(string TagName)
        {
            //檢查是否重覆

            var tagRepeatedCount = (from t in _context.Tags
                                    where t.TagName == TagName
                                    select t).ToList().Count();
            if (tagRepeatedCount > 0)
            {
                return Content("Repeated");
            }
            else if (String.IsNullOrEmpty(TagName))
            {
                return Content("tagEmpty");
            }
            else
            {
                Tag newTag = new Tag()
                {
                    TagName = TagName,
                    CreateUser = "SYS",
                    CreateDate = DateTime.Now,
                    LastupdateUser = "SYS",
                    LastupdateDate = DateTime.Now,
                };
                _context.Tags.Add(newTag);
                _context.SaveChanges();

                var newTagIDList = (from t in _context.Tags
                                    where t.TagName == TagName
                                    select t.TagId).ToList();
                var newTagID = newTagIDList[0];

                return Content(newTagID);
            }

        }


    }
}
