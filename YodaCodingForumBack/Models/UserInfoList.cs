using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Models
{
    public class UserInfoList
    {
        public string uid { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string sex { get; set; }
        public string email { get; set; }
        public int point { get; set; }
        public DateTime creatdate { get; set; }
    }
}
