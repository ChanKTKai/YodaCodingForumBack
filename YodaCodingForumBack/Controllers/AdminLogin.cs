using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YodaCodingForumBack.Models;

namespace YodaCodingForumBack.Controllers
{
    public class AdminLogin : Controller
    {
        private readonly ArticleDBContext _context;

        public AdminLogin(ArticleDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult checkAccount(string account, string password)
        {
            var accountList = (from YD in _context.Ydadmins
                               select YD.AdminAccount).ToList();
            var passwordList = (from YD in _context.Ydadmins
                                select YD.AdminPassword).ToList();

            if (accountList.Contains(account) && passwordList.Contains(password))
            {
                return Content("Success");
            }
            else
            {
                return Content("False");
            }
        }
    }
}
