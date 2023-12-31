﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using System.Text.Json;


namespace prjCoreWebWantWant.Controllers
{
    public class MemberApiController : Controller
    {
        private readonly NewIspanProjectContext _db;
        public MemberApiController(NewIspanProjectContext dbContext)
        {
            _db = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("{id}/photo")]
        public IActionResult GetMemberPhoto(int id)
        {
            var member = _db.MemberAccounts.Find(id);

            if (member == null)
            {
                return NotFound();
            }

            if (member.MemberPhoto == null || member.MemberPhoto.Length == 0)
            {
                return NotFound("No photo available for this member.");
            }

            return File(member.MemberPhoto, "image/jpeg"); // 假設資料庫中的圖片格式為jpeg
        }
        public IActionResult UserList()//列出聊天對象清單
        {
            //if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER)) //判斷是否有登入
            //{
            //    string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            //    CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);

            //    BinaryData userPhoto = 

            //    var orderMessages = _db.ChatMessages
            //                    .OrderByDescending(m => m.Created) // 按照時間戳排序，最新的在前面
            //                    .ToList();

            //    var users = orderMessages
            //              .Where(m => m.SenderId == loggedInUser.AccountId || m.ReceiverId == loggedInUser.AccountId)
            //              .OrderByDescending(m => m.Created)
            //              .Select(m => m.SenderId == loggedInUser.AccountId ? m.ReceiverId : m.SenderId)
            //              .Distinct().ToList();

            //    //JOIN參考資料 https://ithelp.ithome.com.tw/articles/10196394?sc=iThelpR
            //    //https://www.tutorialsteacher.com/linq/linq-joining-operator-join
            //    //A.Join(B,a=>a,b=>b,(a,b)=>new{...}) a=>a  b => b 這邊是兩個表相同的欄位全選(這邊是id)
            //    //(a, b)=>new { ...}  這邊是選擇兩邊的交集，再從中挑出我想要的資訊來
            //    var usersInfo = users
            //                    .Join(_db.MemberAccounts, a => a, member => member.AccountId, (a, member) => new
            //                    {
            //                        member.AccountId,
            //                        member.Name,
            //                        member.UserName,
            //                        member.MemberPhoto,
            //                        LatestMessage = _db.ChatMessages
            //                                        .Where(chat =>
            //                                               (chat.SenderId == loggedInUser.AccountId && chat.ReceiverId == member.AccountId) ||
            //                                               (chat.ReceiverId == loggedInUser.AccountId && chat.SenderId == member.AccountId))
            //                                        .OrderByDescending(chat => chat.Created)
            //                                        .Select(chat => new
            //                                        {
            //                                            chat.Message,
            //                                            chat.Created
            //                                        })
            //                                        .FirstOrDefault()
            //                    }).ToList();
            //    return Json(usersInfo);
            //}
            //else
                return RedirectToAction("Login", "Member");
        }
    }
}
