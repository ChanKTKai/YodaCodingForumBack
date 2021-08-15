using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Models
{
    public class UserViewModecs
    {
        public List<UserInfoList> userList { set; get; }

        public string SearchString { set; get; }
    }
}
