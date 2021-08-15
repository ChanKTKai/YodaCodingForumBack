using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Models
{
    public class TagList
    {
        public string tid { get; set; }
        public string name { get; set; }
        public int usecount { get; set; }
        public int alsocount { get; set; }
        public int followcount { get; set; }
        public string createuser { get; set; }
        public DateTime createdate { get; set; }

    }
}
