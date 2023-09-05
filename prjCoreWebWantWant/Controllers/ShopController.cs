using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using prjCoreWebWantWant.Models;
using prjCoreWebWantWant.ViewModels;
using System.Text.Json;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Org.BouncyCastle.Crypto.Engines;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;
using Org.BouncyCastle.Crypto.Macs;

namespace prjShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly NewIspanProjectContext _context;
        private readonly IWebHostEnvironment _host = null;


        public ShopController(NewIspanProjectContext context, IWebHostEnvironment host, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            _host = host;
        }


        //專家商店
        public IActionResult ExpertShop()
        {
            int id = GetAccountID();

            ViewBag.Points = GetAccumulatedPoints(id);

            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "上架" && p.CategoryId == 1);

            return View(q);
        }
        //任務商店
        public IActionResult CaseShop()
        {
            int id = GetAccountID();

            ViewBag.Points = GetAccumulatedPoints(id);

            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "上架" && p.CategoryId == 2);

            return View(q);
        }

        //paypal結帳
        public IActionResult PayPalCheckOut()
        {
            int userId = GetAccountID();
            ViewBag.Points = GetAccumulatedPoints(userId);

            var latestOrder = _context.Orders
       .Include(o => o.OrderDetails)
       .ThenInclude(o => o.Product)
       .Where(o => o.AccountId == userId)
       .OrderByDescending(o => o.CreateTime)
       .FirstOrDefault();

            List<Order> ordersList = new List<Order>();
            if (latestOrder != null)
            {
                ordersList.Add(latestOrder);
            }

            return View(ordersList);
        }

        public IActionResult Index()
        {
            return View();
        }
        //會員訂單列表
        public IActionResult Order(string filter)
        {
            ViewBag.OrderFilter = filter;
            int userId = GetAccountID();
            IQueryable<Order> query = _context.Orders
                .Include(t => t.Category)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Resume)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Case)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.AccountId == userId);

            if (filter == "today")
            {
                query = query.Where(o => (o.CreateTime ?? DateTime.MinValue).Date == DateTime.Today);
            }
            else if (filter == "7days")
            {
                query = query.Where(o => o.CreateTime >= DateTime.Today.AddDays(-7));
            }
            else if (filter == "31days")
            {
                query = query.Where(o => o.CreateTime >= DateTime.Today.AddDays(-31));
            }
            // Add more conditions for other filters

            var q = query.OrderByDescending(o => o.CreateTime).ToList();
            return View(q);
        }
        //曝光中
        public IActionResult onTop() 
        {

           
            int userId = GetAccountID();
            IQueryable<Order> query = _context.Orders
                .Include(t => t.Category)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Resume)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Case)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.AccountId == userId);

          

            var q = query.OrderByDescending(o => o.CreateTime).ToList();
            return PartialView(q);
        }
        //抓會員登入
        public int GetAccountID()
        {
            string userDataJson = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            CLoginUser loggedInUser = JsonSerializer.Deserialize<CLoginUser>(userDataJson);
            int id = loggedInUser.AccountId; //抓登入者的id                                                                             
            return id;
        }
        //載入任務
        public IActionResult loadCase()
        {
            int id = GetAccountID();

            var cases = _context.TaskLists
                .Where(t => t.AccountId == id && t.PublishOrNot == "立刻上架")
                .Select(t => new { t.CaseId, t.TaskTitle })
                .ToList();
            return Json(new { success = true, tasks = cases });
        }
        //載入專家
        public IActionResult loadExpert()
        {
            int id = GetAccountID();
            var resume = _context.Resumes
                 .Include(r => r.ExpertResume)
                .Where(r => r.AccountId == id && r.IsExpertOrNot == true &&r.CaseStatusId==23)
                .Select(r => new { r.ResumeId, r.ResumeTitle})
             .ToList();

            return Json(resume);

        }

        //使用者紅利
        public int GetAccumulatedPoints(int accountId)
        {
            var memberAccount = _context.MemberAccounts
                .FirstOrDefault(p => p.AccountId == accountId);

            if (memberAccount != null)
            {
                return memberAccount.MemberTotalPoint;
            }
            else
            {
                return 0; //默認為0
            }
        }
        // 加入case商品到購物車
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            int userId = GetAccountID(); //使用者ID

            var cart = GetCart(userId); // 用戶ID購物車
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CCartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = (decimal)product.UnitPrice,
                    Quantity = 1,
                    ImageUrl = product.CoverPhoto,
                    GetPoint = product.GetPoint,
                    TopDate = product.TopDate,
                });
            }

            SaveCart(cart, userId); // 存購物車到指定用戶


            return Json(new { success = true });
        }

        // 加入expert商品到購物車
        [HttpPost]
        public IActionResult AddToExCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            int userId = GetAccountID(); //使用者ID

            var cart = GetExCart(userId); // 用戶ID購物車
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cart.Add(new CCartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitPrice = (decimal)product.UnitPrice,
                    Quantity = 1,
                    ImageUrl = product.CoverPhoto,
                    GetPoint = product.GetPoint,
                    TopDate = product.TopDate,
                });
            }

            SaveExCart(cart, userId); // 存購物車到指定用戶


            return Json(new { success = true });
        }

        // Case購物車頁面
        public IActionResult CaseCart()
        {
            int userId = GetAccountID();
            ViewBag.Points = GetAccumulatedPoints(userId);

            var cart = GetCart(userId);
            ViewBag.Cart = cart;

            decimal total = cart.Sum(item => item.UnitPrice * item.Quantity);
            ViewBag.Total = total;

            return View(cart);
        }

        // Expert購物車頁面
        public IActionResult ExpertCart()
        {
            int userId = GetAccountID();
            ViewBag.Points = GetAccumulatedPoints(userId);

            var cart = GetExCart(userId);
            ViewBag.Cart = cart;

            decimal total = cart.Sum(item => item.UnitPrice * item.Quantity);
            ViewBag.Total = total;

            return View(cart);
        }

        //更改Case商品數量
   
        [HttpPost]
        public IActionResult updateQua(int productId, int newQuantity)
        {
            int userId = GetAccountID();
            var cart = GetCart(userId);

            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = newQuantity; // 更新商品数量
                SaveCart(cart, userId); // 保存整个购物车，包括更新后的 cartItem
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        //更改EX商品數量

        [HttpPost]
        public IActionResult updateQuaEX(int productId, int newQuantity)
        {
            int userId = GetAccountID();
            var cart = GetExCart(userId);

            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = newQuantity; // 更新商品数量
                SaveExCart(cart, userId); // 保存整个购物车，包括更新后的 cartItem
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        //刪除Case購物車東西
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            int userId = GetAccountID();
            var cart = GetCart(userId);
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart, userId);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        //刪除Expert購物車東西
        [HttpPost]
        public IActionResult RemoveExpertCart(int productId)
        {
            int userId = GetAccountID();
            var cart = GetExCart(userId);
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveExCart(cart, userId);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        // case購物車Session
        private List<CCartItem> GetCart(int userId)
        {
            var cartJson = HttpContext.Session.GetString($"Cart_{userId}");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CCartItem>();
            }

            return JsonSerializer.Deserialize<List<CCartItem>>(cartJson);
        }

        //ExpertCart購物車Session
        private List<CCartItem> GetExCart(int userId)
        {
            var cartJson = HttpContext.Session.GetString($"ExCart_{userId}");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CCartItem>();
            }

            return JsonSerializer.Deserialize<List<CCartItem>>(cartJson);
        }
        // case保存購物車數據
        private void SaveCart(List<CCartItem> cart, int userId)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString($"Cart_{userId}", cartJson);
        }
        //Expert保存購物車數據
        private void SaveExCart(List<CCartItem> cart, int userId)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString($"ExCart_{userId}", cartJson);
        }

        //case購物車數量
        [HttpGet]
        public IActionResult GetCartItemCount()
        {
            int userId = GetAccountID();
            var cart = GetCart(userId);
            int itemCount = cart.Sum(item => item.Quantity);

            return Json(new { cartItemCount = itemCount });
        }
        //expert購物車數量
        [HttpGet]
        public IActionResult GetExCartItemCount()
        {
            int userId = GetAccountID();
            var cart = GetExCart(userId);
            int itemCount = cart.Sum(item => item.Quantity);

            return Json(new { cartItemCount = itemCount });
        }


  
        //Case 結帳
        [HttpPost]
        public IActionResult Checkout(int? CaseId, Order newOrder, List<OrderDetail> newOrderDetails)
        {
            int userId = GetAccountID();
            var cart = GetCart(userId);

            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "購物車內沒有商品，無法進行結帳。";

                // 使用SweetAlert的JavaScript代碼顯示提醒
                string sweetAlertScript = $"Swal.fire('購物車內沒有商品', '無法進行結帳。', 'warning');";
                TempData["SweetAlertScript"] = sweetAlertScript;

                return RedirectToAction("CaseCart");
            }

            TempData["SweetAlertScript"] = null;
            foreach (var cartItem in cart)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
                if (product == null || product.UnitsInStock < cartItem.Quantity)
                {
                    // 使用SweetAlert顯示提示消息
                    string errorMessage = "【" + product.ProductName + "】庫存不足，無法完成結帳。";
                    TempData["ErrorMessage"] = errorMessage;

                    // 使用SweetAlert的JavaScript代碼顯示警告框
                    string sweetAlertScript = $"Swal.fire('庫存不足', '{errorMessage}', 'warning');";
                    TempData["SweetAlertScript"] = sweetAlertScript;

                    return RedirectToAction("CaseCart");
                }
            }

            // 創建新的訂單
            newOrder.AccountId = userId;
            newOrder.PayWayId = 1;
            newOrder.CategoryId = 2;
            newOrder.CreateTime = DateTime.Now;
            newOrder.Status = "已付款";
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // 將每個購物車項目關聯到訂單
            foreach (var cartItem in cart)
            {
                var newOrderDetail = new OrderDetail
                {
                    OrderId = newOrder.OrderId,
                    ProductId = cartItem.ProductId,
                    UnitPrice = (int)cartItem.UnitPrice,
                    Quantity = cartItem.Quantity,
                    TopDate = cartItem.TopDate,
                    TopType = "任務置頂",
                    GetPoint = cartItem.GetPoint,
                    CaseId = CaseId,
                };

                // 更新任務的OnTop
                var task = _context.TaskLists.FirstOrDefault(t => t.CaseId == newOrderDetail.CaseId);
                if (task != null && newOrderDetail.TopDate.HasValue)
                {
                    int day = (int)(newOrderDetail.TopDate * newOrderDetail.Quantity);
                    if (task.OnTop == null || task.OnTop < DateTime.Now)
                    {
                        task.OnTop = DateTime.Now.AddDays(day);
                    }
                    else
                    {
                        task.OnTop = task.OnTop.Value.AddDays(day);
                    }
                }

                newOrderDetails.Add(newOrderDetail);
            }

            // 將所有新的 OrderDetail 加入 _context
            _context.OrderDetails.AddRange(newOrderDetails);
            _context.SaveChanges();

            //更新庫存
            foreach (var orderDetail in newOrderDetails)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == orderDetail.ProductId);
                if (product != null)
                {
                    product.UnitsInStock -= orderDetail.Quantity;
                }
            }

            // 更新會員點數
            int orderGetPoint = Convert.ToInt32(Request.Form["OrderGetPoint"]);
            int orderUsePoint = Convert.ToInt32(Request.Form["OrderUsePoint"]);

            var memberAccount = _context.MemberAccounts.FirstOrDefault(p => p.AccountId == userId);
            if (memberAccount != null)
            {   // 保證不會扣除超過會員現有的點數
                if (orderUsePoint > memberAccount.MemberTotalPoint)
                {
                    orderUsePoint = memberAccount.MemberTotalPoint; // 將扣除點數調整為會員現有的點數
                }

                memberAccount.MemberTotalPoint -= orderUsePoint;
                memberAccount.MemberTotalPoint += orderGetPoint;
            }

            // 清空購物車
            cart.Clear();
            SaveCart(cart, userId);

            Email(newOrderDetails, "cocopanisadog@gmail.com", "Recipient Name",newOrder);

            _context.SaveChanges();

            return RedirectToAction("PayPalCheckOut");
        }

        //EX 結帳
        [HttpPost]
        public IActionResult CheckoutEx(int? ResumeId, Order newOrder, List<OrderDetail> newOrderDetails)
        {
            int userId = GetAccountID();
            var cart = GetExCart(userId);

            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "購物車內沒有商品，無法進行結帳。";
                return RedirectToAction("ExpertCart");
            }

            TempData["SweetAlertScript"] = null;
            foreach (var cartItem in cart)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
                if (product == null || product.UnitsInStock < cartItem.Quantity)
                {
                    // 使用SweetAlert顯示提示消息
                    string errorMessage = "【" + product.ProductName + "】庫存不足，無法進行結帳。";
                    TempData["ErrorMessage"] = errorMessage;

                    // 使用SweetAlert的JavaScript代碼顯示警告框
                    string sweetAlertScript = $"Swal.fire('庫存不足', '{errorMessage}', 'warning');";
                    TempData["SweetAlertScript"] = sweetAlertScript;

                    return RedirectToAction("ExpertCart");
                }
            }

            // 創建新的訂單
            newOrder.AccountId = userId;
            newOrder.PayWayId = 1;
            newOrder.CategoryId = 1;
            newOrder.CreateTime = DateTime.Now;
            newOrder.Status = "已付款";
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // 將每個購物車項目關聯到訂單
            foreach (var cartItem in cart)
            {
                var newOrderDetail = new OrderDetail
                {
                    OrderId = newOrder.OrderId,
                    ProductId = cartItem.ProductId,
                    UnitPrice = (int)cartItem.UnitPrice,
                    Quantity = cartItem.Quantity,
                    TopDate = cartItem.TopDate,
                    TopType = "專家置頂",
                    GetPoint = cartItem.GetPoint,
                    ResumeId = ResumeId,
                };

                // 更新專家的OnTop
                var resume = _context.Resumes.FirstOrDefault(t => t.ResumeId == newOrderDetail.ResumeId);
                if (resume != null && newOrderDetail.TopDate.HasValue)
                {
                    int day = (int)(newOrderDetail.TopDate * newOrderDetail.Quantity);
                    if (resume.OnTop == null || resume.OnTop < DateTime.Now)
                    {
                        resume.OnTop = DateTime.Now.AddDays(day);
                    }
                    else
                    {
                        resume.OnTop = resume.OnTop.Value.AddDays(day);
                    }
                }

                newOrderDetails.Add(newOrderDetail);
            }

            // 將所有新的 OrderDetail 加入 _context
            _context.OrderDetails.AddRange(newOrderDetails);
            _context.SaveChanges();

            //更新庫存
            foreach (var orderDetail in newOrderDetails)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == orderDetail.ProductId);
                if (product != null)
                {
                    product.UnitsInStock -= orderDetail.Quantity;
                }
            }

            // 更新會員點數
            int orderGetPoint = Convert.ToInt32(Request.Form["OrderGetPoint"]);
            int orderUsePoint = Convert.ToInt32(Request.Form["OrderUsePoint"]);

            var memberAccount = _context.MemberAccounts.FirstOrDefault(p => p.AccountId == userId);
            if (memberAccount != null)
            {   // 保證不會扣除超過會員現有的點數
                if (orderUsePoint > memberAccount.MemberTotalPoint)
                {
                    orderUsePoint = memberAccount.MemberTotalPoint; // 將扣除點數調整為會員現有的點數
                }

                memberAccount.MemberTotalPoint -= orderUsePoint;
                memberAccount.MemberTotalPoint += orderGetPoint;
            }

            // 清空購物車
            cart.Clear();
            SaveExCart(cart, userId);


            _context.SaveChanges();

            return RedirectToAction("PayPalCheckOut");
        }

        public IActionResult OrderCompleted()
        {
            int userId = GetAccountID();
            ViewBag.Points = GetAccumulatedPoints(userId);

            var latestOrder = _context.Orders
       .Include(o => o.OrderDetails)
       .ThenInclude(o => o.Product)
       .Where(o => o.AccountId == userId)
       .OrderByDescending(o => o.CreateTime)
       .FirstOrDefault();

            List<Order> ordersList = new List<Order>();
            if (latestOrder != null)
            {
                ordersList.Add(latestOrder);
            }

            return View(ordersList);

        }

        public void Email(List<OrderDetail> newOrderDetails, string recipientEmail, string recipientName, Order newOrder)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("WantWant", "andy26625325@gmail.com"));
                message.To.Add(new MailboxAddress(recipientName, recipientEmail));
                message.Subject = "WantWant 曝光加值完成";

                var bodyBuilder = new BodyBuilder();

                string htmlBody = @"
<!DOCTYPE html>
<html>
<head>
  <style>
        body {
            font-family: Arial, sans-serif;
            background: #fffdfd;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 600px;
            margin: auto;
            padding: 20px;
            background-color: #fff;
            box-shadow: 0 6px 20px 0 rgba(0, 0, 0, 0.19);
            border-radius: 10px;
        }

        .title {
            font-size: 18px;
            margin-bottom: 10px;
            font-weight: bold;
        }

        .message {
            font-size: 14px;
            margin-bottom: 20px;
        }

        .table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

        .table th, .table td {
            border: 1px solid #ddd;
            padding: 8px;
        }

        .summary {
            background-color: #ddd;
            padding: 20px;
            color: #414141;
            border-radius: 10px;
          
        }

        .summary-row {
            display: flex;
            justify-content: space-between;
            margin-bottom: 10px;
        }

        .btn {
            background-color: #000;
            color: #fff;
            border: none;
            padding: 10px 20px;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            text-decoration: none;
        }

        .btn:hover {
            background-color: #333;
        }
    </style>

</head>
<body>
    <div class=""container"">
        <div class=""title"">訂單內容</div>
        <div class=""message"">
            感謝您的購買，您在【WantWant】購買的『曝光訂單』已成立，詳情可由下方按鈕查詢，並歡迎您再次選購 謝謝！<br>
        </div>
        <table class=""table"">
            <thead>
                <tr>
                    <th>商品</th>
                    <th>單價</th>
                    <th>數量</th>
                    <th>小計</th>
                </tr>
            </thead>
            <tbody>";

                foreach (var orderDetail in newOrderDetails)
                {
                    var product = _context.Products.FirstOrDefault(p => p.ProductId == orderDetail.ProductId);
                    if (product != null)
                    {
                        htmlBody += $@"
                <tr> 
                    <td>{product.ProductName}</td>
                    <td>{orderDetail.UnitPrice}</td>
                    <td>{orderDetail.Quantity}</td>
                    <td>{orderDetail.UnitPrice * orderDetail.Quantity}</td>
                </tr>";
                    }
                }
                      htmlBody += @"
          
             </tbody>
        </table>
        <div class=""summary"">
            <div class=""summary-row"">
                <div>商品總計</div>
                <div>" + newOrder.OrderPrice+ @"</div>
            </div>  
            <div class=""summary-row"">
                <div>獲得紅利</div>
                <div>" + newOrder.OrderGetPoint + @"</div>
            </div>
            <div class=""summary-row"" style=""color: brown;"">
                <div>紅利折抵</div>
                <div>" + newOrder.OrderUsePoint+ @"</div>
            </div>
            <div class=""summary-row"">
                <div>訂單金額</div>
                <div style=""font-size: 1.5rem;"">" + newOrder.PaidAmount+ @"</div>
            </div>
            <a href=""https://localhost:7042/Member/Login"" class=""btn"">查看詳情</a>
        </div>
  <div class=""message"">
           *注意本郵件是由【WantWant】自動產出與發送，請勿直接回覆。<br>
*如欲查詢訂單狀況請至【WantWant】首頁左上方，進入會員專區，點選【曝光紀錄】，即可查詢。
    </div>

        </div>
</body>
</html>";

                // 設置信件為HTML格式
                bodyBuilder.HtmlBody = htmlBody;

                // 將信件添加到郵件消息中
                message.Body = bodyBuilder.ToMessageBody();

                // 創建SmtpClient 對象用於發送信件
                using (var client = new SmtpClient())
                {
                    // 連接到SMTP，參數為伺服器地址、端口和是否使用 SSL
                    client.Connect("smtp.gmail.com", 587, false);

                    string e = "andy26625325@gmail.com";
                    string p = "mhsbmjzwrguxoejo";
                    // 使用發件人信箱地址和密碼
                    client.Authenticate(e, p);

                    // 發送信件
                    client.Send(message);

                    // 斷開連結
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                // 發送失敗的處理
                Console.WriteLine("發送信件失敗：" + ex.Message);
            }
        }



    }
}
