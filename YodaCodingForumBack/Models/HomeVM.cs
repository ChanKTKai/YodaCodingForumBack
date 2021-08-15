using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace YodaCodingForumBack.Models
{
    public class HomeVM
    {
        public int todayQ { get; set; }
        public int todayA { get; set; }
        public int todayM { get; set; }
        public List<Article> articles { get; set; }
        public List<tagUseCount> taguse { get; set; }
        public int articleViewCount { get; set; }
        public int questionCount { get; set; }
        public int finichQuestionCount { get; set; }
        public int articleCount { get; set; }
        public int aAndQCount { get; set; }
        public int commendCount { get; set; }
        public List<string> bar1 { get; set; }
        public List<string> peiPrec { get; set; }
    }
}
