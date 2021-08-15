using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Models
{
    public class questionList
    {
        public string qid { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public DateTime createdate { get; set; }
        public string createuser { get; set; }
        public string lastupdate_user { get; set; }
        public DateTime lastupdate_date { get; set; }
        public List<ProgramLang> lang { get; set; }
        public List<QuestionOption> option { get; set; }
        public int version { get; set; }
    }
}

