using YodaCodingForumBack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YodaCodingForumBack.Controllers
{
    public class UserInfoController : Controller
    {
        private readonly ArticleDBContext _context;
        public UserInfoController(ArticleDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userQuery = (from u in _context.UserInfos
                             join p in _context.UserPoints on u.UserId equals p.UserId
                             orderby u.UserStatus descending, u.CreateDate descending
                             select new UserInfoList
                             {
                                 uid = u.UserId,
                                 account = u.UserAccount,
                                 name = u.UserName,
                                 status = u.UserStatus,
                                 sex = u.UserSax,
                                 email = u.UserEmail,
                                 point = p.PointAmount,
                                 creatdate = u.CreateDate
                             }).ToList();


            var userVM = new SearchClass
            {
                userList = listcheckdata(userQuery)
            };
            return View(userVM);
        }

        public IActionResult Search(string searchString)
        {
            var userQuery = from u in _context.UserInfos
                            join p in _context.UserPoints on u.UserId equals p.UserId
                            orderby u.UserStatus descending, u.CreateDate descending
                            select new UserInfoList
                            {
                                uid = u.UserId,
                                account = u.UserAccount,
                                name = u.UserName,
                                status = u.UserStatus,
                                sex = u.UserSax,
                                email = u.UserEmail,
                                point = p.PointAmount,
                                creatdate = u.CreateDate
                            };

            if (!String.IsNullOrEmpty(searchString))
            {
                userQuery = userQuery.Where(t => t.account.Contains(searchString.Replace(" ","")));
            }

            var userVM = new SearchClass
            {
                userList = listcheckdata(userQuery.ToList())
            };
            return View(userVM);
        }

        public IActionResult UserList(string uid)
        {

            var uinfoQuery = (from u in _context.UserInfos
                              join p in _context.UserPoints on u.UserId equals p.UserId
                              where u.UserId == uid
                              select new UserInfoClass
                              {
                                  uid = u.UserId,
                                  account = u.UserAccount,
                                  name = u.UserName,
                                  nickname = u.UserNickname,
                                  sax = u.UserSax,
                                  birthday = u.UserBirthday.ToString(),
                                  address = u.UserAddress,
                                  email = u.UserEmail,
                                  phone = u.UserPhone,
                                  experience = u.UserExperience,
                                  profession = u.UserProfession,
                                  status = u.UserStatus,
                                  point = p.PointAmount,
                                  remark = u.Remark,
                                  create_date = u.CreateDate,
                                  create_user = u.CreateUser,
                                  lastupdate_date = u.LastupdateDate,
                                  lastupdate_user = u.LastupdateUser,
                                  version = u.Version
                              }).ToList();

            var user = new SearchClass
            {
                userInfo = checkdata(uinfoQuery)
            };
            return View(user);
        }

        [HttpPost]
        public ActionResult UserList(string uid, string status, string remark)
        {
            var userStatusUpdate = _context.UserInfos.Find(uid);
            userStatusUpdate.UserStatus = status;
            userStatusUpdate.Remark = remark;
            userStatusUpdate.LastupdateDate = DateTime.Now;
            userStatusUpdate.LastupdateUser = "SYS";
            userStatusUpdate.Version++;
            _context.SaveChanges();

            return Redirect($"/UserInfo/UserList?uid={uid}");

        }

        //傳入會員List資料
        public List<UserInfoList> listcheckdata(List<UserInfoList> userList)
        {
            //顯示
            foreach (var u in userList)
            {
                u.status = u.status == "N" ? "未認證" : (u.status == "T" ? "正常" : "關閉");
                u.sex = u.sex == "N" ? "不提供" : (u.sex == "M" ? "男" : "女");
            }
            return userList;
        }

        //傳入會員頁面的資料
        public List<UserInfoClass> checkdata(List<UserInfoClass> userinfo)
        {
            foreach (var u in userinfo)
            {
                //使用者圖片
                var imageQuery = from chk in _context.UserImages
                                 where chk.UserId == u.uid
                                 select chk;

                //如使用者沒有設定圖片
                if (imageQuery.Count() == 0)
                {
                    //則依照性別顯示系統設定圖片
                    var sysimageQuery = from img in _context.UserImages
                                        where img.UserId == "SYS"
                                        where img.ImageId == "Defult" + u.sax
                                        select img;

                    foreach (var img in sysimageQuery)
                    {
                        u.uimage = img.Uimage;
                    }
                }
                //顯示使用者設定圖片
                else
                {
                    foreach (var img in imageQuery)
                    {
                        u.uimage = img.Uimage;
                    }
                }

                //顯示用
                u.nickname = u.nickname == null ? "未提供" : u.nickname;
                u.birthday = u.birthday == null ? "未提供" : u.birthday;
                u.address = u.address == null ? "未提供" : u.address;
                u.phone = u.phone == null ? "未提供" : u.phone;
                u.experience = u.experience == null ? "未提供" : u.experience;
                u.profession = u.profession == null ? "未提供" : u.profession;
                u.sax = u.sax == "N" ? "不提供" : (u.sax == "M" ? "男" : "女");


            }
            return userinfo;
        }
    }
}
