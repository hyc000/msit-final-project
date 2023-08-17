using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjCoreWebWantWant.Models;
using System;

namespace WantTask.Controllers
{
    public class ChatApiController : Controller
    {

        int loginID = 56;//先綁死ID登入
        NewIspanProjectContext _db = new NewIspanProjectContext();


        public IActionResult UserList()//列出聊天對象清單
        {
            var orderMessages = _db.ChatMessages
                                .OrderByDescending(m => m.Created) // 按照時間戳排序，最新的在前面
                                .ToList();

            var users= orderMessages
                      .Where(m=>m.SenderId== loginID||m.ReceiverId==loginID)
                      .OrderByDescending(m => m.Created)
                      .Select(m=>m.SenderId==loginID?m.ReceiverId:m.SenderId)
                      .Distinct().ToList();

            //JOIN參考資料 https://ithelp.ithome.com.tw/articles/10196394?sc=iThelpR
            //https://www.tutorialsteacher.com/linq/linq-joining-operator-join
            //A.Join(B,a=>a,b=>b,(a,b)=>new{...}) a=>a  b => b 這邊是兩個表相同的欄位全選(這邊是id)
            //(a, b)=>new { ...}  這邊是選擇兩邊的交集，再從中挑出我想要的資訊來
            var usersInfo = users
                            .Join(_db.MemberAccounts, a => a, member => member.AccountId, (a, member) => new
                            {
                                member.AccountId,
                                member.Name,
                                member.UserName,
                                member.MemberPhoto,
                                LatestMessage = _db.ChatMessages
                                                .Where(chat =>
                                                       (chat.SenderId == loginID && chat.ReceiverId == member.AccountId) ||
                                                       (chat.ReceiverId == loginID && chat.SenderId == member.AccountId))
                                                .OrderByDescending(chat => chat.Created)
                                                .Select(chat => new { chat.Message,
                                                    chat.Created })
                                                .FirstOrDefault()
                            }).ToList();
            return Json(usersInfo);
        }

        //列出與聊天對象內容
        public IActionResult ChatDetail(int chatWithId,int page=1)
        {
            double countTotal = _db.ChatMessages.Where(p => p.SenderId == chatWithId || p.ReceiverId == chatWithId).Count();
            int perpage = 15;//每頁筆數
            int totalPage = (int)Math.Floor(countTotal / perpage) + 1;

            var chatInfo = _db.ChatMessages
                         .Where(chat =>
                                    (chat.SenderId == loginID && chat.ReceiverId == chatWithId) ||
                                    (chat.ReceiverId == loginID && chat.SenderId == chatWithId))
                                    .OrderByDescending(chat => chat.Created)
                                    .Skip((page-1)* perpage)
                                    .Take(perpage)
                                    .ToList();
            return Json(chatInfo);
        }
    }
}
