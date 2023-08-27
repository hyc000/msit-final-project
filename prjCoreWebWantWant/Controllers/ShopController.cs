using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using prjCoreWebWantWant.Models;
using System.Text.Json;

namespace prjShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly NewIspanProjectContext _context;
        private readonly IWebHostEnvironment _host = null;

        public ShopController(NewIspanProjectContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }
    
     

        public IActionResult ExpertShop()
        {
            int id = GetAccountID();

            ViewBag.Points = GetAccumulatedPoints(id);

            int cartItemCount = GetExCartItemCount();
            ViewBag.CartItemCount = cartItemCount;

            var q = _context.Products
                .Include(t => t.Category)
                .Where(p => p.Status == "上架" && p.CategoryId == 1);

            return View(q);
        }
        public IActionResult CaseShop()
        {



            int id = GetAccountID();

            ViewBag.Points = GetAccumulatedPoints(id);

            int cartItemCount = GetCartItemCount();
            ViewBag.CartItemCount = cartItemCount;

            var q = _context.Products
              .Include(t => t.Category)
              .Where(p => p.Status == "上架" && p.CategoryId == 2);
            return View(q);
        }

        //Casepaypal結帳
        public IActionResult PayPalCheckOut()
        {
            int userId = GetAccountID();
            ViewBag.Points = GetAccumulatedPoints(userId);

            var latestOrder = _context.Orders
       .Include(o => o.OrderDetails)
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
        public IActionResult Order()
        {
            return View();
        }

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
                .Where(r => r.AccountId == id && r.IsExpertOrNot == true)
                .Select(r => new { r.ResumeId, r.ExpertResume.Introduction })
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


            return RedirectToAction("CaseShop");
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


            return RedirectToAction("ExpertShop");
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
        public int GetCartItemCount()
        {
            int userId = GetAccountID();
            var cart = GetCart(userId);
            int itemCount = cart.Sum(item => item.Quantity);
            return itemCount;
        }
        //expert購物車數量
        public int GetExCartItemCount()
        {
            int userId = GetAccountID();
            var cart = GetExCart(userId);
            int itemCount = cart.Sum(item => item.Quantity);
            return itemCount;
        }


        //        [HttpPost]
        //        public IActionResult Checkout(int? CaseId, Order newOrder, List<OrderDetail> newOrderDetails)
        //        {


        //            int userId = GetAccountID();
        //            var cart = GetCart(userId);

        //            if (cart.Count == 0)
        //            {
        //                TempData["ErrorMessage"] = "購物車內沒有商品，無法進行結帳。";
        //                return RedirectToAction("CaseCart");
        //            }

        //            TempData["SweetAlertScript"] = null;
        //            foreach (var cartItem in cart)
        //            {
        //                var product = _context.Products.FirstOrDefault(p => p.ProductId == cartItem.ProductId);
        //                if (product == null || product.UnitsInStock < cartItem.Quantity)
        //                {
        //                    // 使用SweetAlert2顯示提示消息
        //                    string errorMessage = "【" + product.ProductName + "】庫存不足，無法完成結帳。";
        //                    TempData["ErrorMessage"] = errorMessage;

        //                    // 使用SweetAlert2的JavaScript代碼顯示警告框
        //                    string sweetAlertScript = $"Swal.fire('庫存不足', '{errorMessage}', 'warning');";
        //                    TempData["SweetAlertScript"] = sweetAlertScript;

        //                    return RedirectToAction("CaseCart");
        //                }
        //            }

        //            // 創建新的訂單
        //            newOrder.AccountId = userId;
        //            newOrder.PayWayId = 1;
        //            newOrder.CategoryId = 2;
        //            newOrder.CreateTime = DateTime.Now;
        //            _context.Orders.Add(newOrder);
        //            _context.SaveChanges();

        //            // 將每個購物車項目關聯到訂單
        //            foreach (var cartItem in cart)
        //            {
        //                var newOrderDetail = new OrderDetail
        //                {
        //                    OrderId = newOrder.OrderId,
        //                    ProductId = cartItem.ProductId,
        //                    UnitPrice = (int)cartItem.UnitPrice,
        //                    Quantity = cartItem.Quantity,
        //                    TopDate = cartItem.TopDate,
        //                    TopType = "任務置頂",
        //                    GetPoint = cartItem.GetPoint,
        //                    CaseId = CaseId,

        //                };
        //                newOrderDetails.Add(newOrderDetail);
        //            }

        //            // 將所有新的 OrderDetail 加入 _context
        //            _context.OrderDetails.AddRange(newOrderDetails);
        //            _context.SaveChanges();

        //            //更新庫存
        //            foreach (var orderDetail in newOrderDetails)
        //            {
        //                var product = _context.Products.FirstOrDefault(p => p.ProductId == orderDetail.ProductId);
        //                if (product != null)
        //                {
        //                    product.UnitsInStock -= orderDetail.Quantity;
        //                    _context.SaveChanges();
        //                }
        //            }

        //            // 更新會員點數
        //            int orderGetPoint = Convert.ToInt32(Request.Form["OrderGetPoint"]);
        //            int orderUsePoint = Convert.ToInt32(Request.Form["OrderUsePoint"]);

        //            var memberAccount = _context.MemberAccounts.FirstOrDefault(p => p.AccountId == userId);
        //            if (memberAccount != null)
        //            {   // 保證不會扣除超過會員現有的點數
        //                if (orderUsePoint > memberAccount.MemberTotalPoint)
        //                {
        //                    orderUsePoint = memberAccount.MemberTotalPoint; // 將扣除點數調整為會員現有的點數
        //                }

        //                memberAccount.MemberTotalPoint -= orderUsePoint;
        //                memberAccount.MemberTotalPoint += orderGetPoint;
        //                _context.SaveChanges();
        //            }

        //            // 更新top
        //foreach (var orderDetail in newOrderDetails)
        //            {
        //                var product = _context.Products.FirstOrDefault(p => p.ProductId == orderDetail.ProductId);
        //                if (product != null)
        //                {
        //                    if (orderDetail.TopDate != null) // 只处理不为null的TopDate
        //                    {
        //                        var task = _context.TaskLists.FirstOrDefault(t => t.CaseId == orderDetail.CaseId);
        //                        if (task != null)
        //                        {
        //                            DateTime newOnTop;
        //                            int day = (int)(orderDetail.TopDate * orderDetail.Quantity);
        //                            if (task.OnTop == null || task.OnTop < DateTime.Now)
        //                            {

        //                                newOnTop = DateTime.Now.AddDays(day);
        //                            }
        //                            else
        //                            {
        //                                newOnTop = task.OnTop.Value.AddDays(day);
        //                            }

        //                            task.OnTop = newOnTop;

        //                            // 更新数据库中的任务表
        //                            _context.TaskLists.Update(task);
        //                        }
        //                    }
        //                }
        //            }

        //           
        //            _context.SaveChanges();

        //            // 清空購物車
        //            cart.Clear();
        //            SaveCart(cart, userId);

        //            return RedirectToAction("Order");
        //        }


        //Case 結帳
        [HttpPost]
        public IActionResult Checkout(int? CaseId, Order newOrder, List<OrderDetail> newOrderDetails)
        {
            int userId = GetAccountID();
            var cart = GetCart(userId);

            if (cart.Count == 0)
            {
                TempData["ErrorMessage"] = "購物車內沒有商品，無法進行結帳。";
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


            _context.SaveChanges();

            return RedirectToAction("PayPalCheckOut");
        }

        //Case 結帳
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

            return RedirectToAction("CheckOut");
        }

        public IActionResult OrderCompleted()
        {

            return View();
        }
    }
}
