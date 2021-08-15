using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static YodaCodingForumBack.Controllers.ArticleController;

namespace YodaCodingForumBack.Models
{
    public class SearchClass
    {
        public List<UserInfoList> userList { get; set; }
        public List<UserInfoClass> userInfo { get; set; }
        public List<TagList> tagList { get; set; }
        public List<Article> articleList { get; set; }
        public articleDetail articleDetail { get; set; }
        public List<questionList> questLists { get; set; }
        public List<ProgramLang> langList{ get; set; }
        public List<QuestionType> questionTypeList { get; set; }
        public List<TagAndTagAlso> tagDetailList { get; set; }
        public List<ReportList> reportList { get; set; }
        public HomeVM HomeVMs { get; set; }
        public string SearchString { get; set; }

    }
}
