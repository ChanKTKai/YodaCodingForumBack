using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaCodingForumBack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YodaCodingForumBack.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ArticleDBContext _context;
        public QuestionController(ArticleDBContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            //語言清單
            var langquery = from p in _context.ProgramLangs
                            select p;
            //測驗清單
            var questquery = from q in _context.Questions
                             orderby q.CreateDate descending
                             select new questionList
                             {
                                 qid = q.QuestionId,
                                 name = q.QuestionName,
                                 type = q.QuestionType,
                                 createdate = q.CreateDate,
                                 createuser = q.CreateUser,
                                 option = (from o in _context.QuestionOptions
                                           where o.QuestionId == q.QuestionId
                                           select new QuestionOption
                                           {
                                               QoptionId = o.QoptionId,
                                               QuestionId = o.QuestionId,
                                               QoptionName = o.QoptionName,
                                               IscorrectAnswer = o.IscorrectAnswer,
                                               CreateUser = o.CreateUser,
                                               CreateDate = o.CreateDate,
                                               Version = o.Version
                                           }).ToList(),
                                 lang = (from l in _context.ProgramLangs
                                         where l.PlId == q.PlId
                                         select l).ToList(),
                                 lastupdate_date = q.LastupdateDate,
                                 lastupdate_user = q.LastupdateUser,
                                 version = q.Version
                             };

            //目前沒有問題類別Table,於這邊建立後傳給前端
            List<QuestionType> qt = new List<QuestionType>();
            qt.Add(new QuestionType() { code = "A", name = "單選題" });
            qt.Add(new QuestionType() { code = "M", name = "多選題" });
            qt.Add(new QuestionType() { code = "T", name = "判斷題" });

            var questVm = new SearchClass
            {
                langList = langquery.ToList(),
                questLists = questquery.ToList(),
                questionTypeList = qt
            };

            return View(questVm);
        }

        public IActionResult Search(string searchString)
        {

            var langquery = from p in _context.ProgramLangs
                            select p;

            var questquery = from q in _context.Questions
                             orderby q.CreateDate descending
                             select new questionList
                             {
                                 qid = q.QuestionId,
                                 name = q.QuestionName,
                                 type = q.QuestionType,
                                 createdate = q.CreateDate,
                                 createuser = q.CreateUser,
                                 option = (from o in _context.QuestionOptions
                                           where o.QuestionId == q.QuestionId
                                           select new QuestionOption
                                           {
                                               QoptionId = o.QoptionId,
                                               QuestionId = o.QuestionId,
                                               QoptionName = o.QoptionName,
                                               IscorrectAnswer = o.IscorrectAnswer,
                                               CreateUser = o.CreateUser,
                                               CreateDate = o.CreateDate,
                                               Version = o.Version
                                           }).ToList(),
                                 lang = (from l in _context.ProgramLangs
                                         where l.PlId == q.PlId
                                         select l).ToList(),
                                 lastupdate_date = q.LastupdateDate,
                                 lastupdate_user = q.LastupdateUser,
                                 version = q.Version
                             };

            List<QuestionType> qt = new List<QuestionType>();
            qt.Add(new QuestionType() { code = "A", name = "單選題" });
            qt.Add(new QuestionType() { code = "M", name = "多選題" });
            qt.Add(new QuestionType() { code = "T", name = "判斷題" });

            if (!String.IsNullOrEmpty(searchString))
            {
                questquery = questquery.Where(q => q.name.Contains(searchString));
            }

            var questVm = new SearchClass
            {
                langList = langquery.ToList(),
                questLists = questquery.ToList(),
                questionTypeList = qt
            };

            return View(questVm);
        }

        [HttpPost]
        //新增
        public ActionResult IndexAdd(string qname, string langid, string qtype,
                                     string op1isanswer, string op1name,
                                     string op2isanswer, string op2name,
                                     string op3isanswer, string op3name,
                                     string op4isanswer, string op4name)
        {
            Question newQuestion = new Question
            {
                QuestionName = qname,
                PlId= langid,
                QuestionType = qtype,
                CreateUser = "SYS",
                CreateDate = DateTime.Now,
                LastupdateUser = "SYS",
                LastupdateDate = DateTime.Now
            };
            _context.Questions.Add(newQuestion);
            _context.SaveChanges();

            //查ID
            var newQuestionId = (from q in _context.Questions
                                 where q.QuestionName == newQuestion.QuestionName
                                 where q.CreateDate == newQuestion.CreateDate
                                 where q.Version == 1
                                 select q.QuestionId).ToList();

            //op1
            QuestionOption newOption1 = new QuestionOption
            {
                QuestionId = newQuestionId[0],
                QoptionName = op1name,
                IscorrectAnswer = op1isanswer is null ? "F" : "T",
                CreateUser = "SYS",
                CreateDate = DateTime.Now,
                LastupdateUser = "SYS",
                LastupdateDate = DateTime.Now
            };

            _context.QuestionOptions.Add(newOption1);

            //op2
            QuestionOption newOption2 = new QuestionOption
            {
                QuestionId = newQuestionId[0],
                QoptionName = op2name,
                IscorrectAnswer = op2isanswer is null ? "F" : "T",
                CreateUser = "SYS",
                CreateDate = DateTime.Now,
                LastupdateUser = "SYS",
                LastupdateDate = DateTime.Now
            };

            _context.QuestionOptions.Add(newOption2);

            //op3
            QuestionOption newOption3 = new QuestionOption
            {
                QuestionId = newQuestionId[0],
                QoptionName = op3name,
                IscorrectAnswer = op3isanswer is null ? "F" : "T",
                CreateUser = "SYS",
                CreateDate = DateTime.Now,
                LastupdateUser = "SYS",
                LastupdateDate = DateTime.Now
            };

            _context.QuestionOptions.Add(newOption3);

            //op4
            QuestionOption newOption4 = new QuestionOption
            {
                QuestionId = newQuestionId[0],
                QoptionName = op4name,
                IscorrectAnswer = op4isanswer is null ? "F" : "T",
                CreateUser = "SYS",
                CreateDate = DateTime.Now,
                LastupdateUser = "SYS",
                LastupdateDate = DateTime.Now
            };

            _context.QuestionOptions.Add(newOption4);
            _context.SaveChanges();

            return Redirect($"/Question/Index");
        }

        [HttpPost]
        //更新
        public ActionResult Index(string qid, string qname, string langid, string qtype,
                                  string op1id, string op1isanswer, string op1name,
                                  string op2id, string op2isanswer, string op2name,
                                  string op3id, string op3isanswer, string op3name,
                                  string op4id, string op4isanswer, string op4name)
        {

            //Question
            var questionUpdate = _context.Questions.Find(qid);
            questionUpdate.QuestionName = qname;
            questionUpdate.PlId = langid;
            questionUpdate.QuestionType = qtype;
            questionUpdate.LastupdateUser = "SYS";
            questionUpdate.LastupdateDate = DateTime.Now;
            questionUpdate.Version++;

            //Option
            var option1Update = _context.QuestionOptions.Find(op1id);
            option1Update.QoptionName = op1name;
            option1Update.IscorrectAnswer = op1isanswer is null ? "F" : "T";
            option1Update.LastupdateDate = DateTime.Now;
            option1Update.LastupdateUser = "SYS";
            option1Update.Version++;

            var option2Update = _context.QuestionOptions.Find(op2id);
            option2Update.QoptionName = op2name;
            option2Update.IscorrectAnswer = op2isanswer is null ? "F" : "T";
            option2Update.LastupdateDate = DateTime.Now;
            option2Update.LastupdateUser = "SYS";
            option2Update.Version++;

            var option3Update = _context.QuestionOptions.Find(op3id);
            option3Update.QoptionName = op3name;
            option3Update.IscorrectAnswer = op3isanswer is null ? "F" : "T";
            option3Update.LastupdateDate = DateTime.Now;
            option3Update.LastupdateUser = "SYS";
            option3Update.Version++;

            var option4Update = _context.QuestionOptions.Find(op4id);
            option4Update.QoptionName = op4name;
            option4Update.IscorrectAnswer = op4isanswer is null ? "F" : "T";
            option4Update.LastupdateDate = DateTime.Now;
            option4Update.LastupdateUser = "SYS";
            option4Update.Version++;

            _context.SaveChanges();

            return Redirect($"/Question/Index");
        }

        //刪除
        public ActionResult Delete(string id)
        {
            //找出這問題的所有選項,並刪除
            var options = (from o in _context.QuestionOptions
                          where o.QuestionId == id
                          select o.QoptionId).ToList();

            foreach (string opid in options)
            {
                var deleteOP = _context.QuestionOptions.Find(opid);
                _context.Remove(deleteOP);
            }

            //刪除問題
            var deleteQ = _context.Questions.Find(id);
            _context.Remove(deleteQ);
            _context.SaveChanges();

            return Redirect($"/Question/Index");
        }

    }
}
