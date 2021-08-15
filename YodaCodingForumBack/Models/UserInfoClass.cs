using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Models
{
    public class UserInfoClass
    {
        public byte[] uimage { get; set; }
        public string uid { get; set; }
        public string account { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string sax { get; set; }
        public string birthday { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string experience { get; set; }
        public string profession { get; set; }
        public string status { get; set; }
        public int point { get; set; }
        public string remark { get; set; }
        public DateTime create_date { get; set; }
        public string create_user { get; set; }
        public string lastupdate_user { get; set; }
        public DateTime lastupdate_date { get; set; }
        public int version { get; set; }


    }
}
