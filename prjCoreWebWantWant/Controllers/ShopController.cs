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
        public IActionResult CheckOut()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
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

        public IActionResult loadCase()
        {
            int id = GetAccountID();

            var cases = _context.TaskLists
                .Where(t => t.AccountId == id && t.PublishOrNot == "立刻上架")
                .Select(t => new { t.CaseId, t.TaskTitle })
                .ToList();
            return Json(new { success = true, tasks = cases });
        }

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
        // 添加商品到購物車
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            int userId = GetAccountID(); // 獲取使用者ID

            var cart = GetCart(userId); // 使用用戶ID獲取購物車
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
                });
            }

            SaveCart(cart, userId); // 存購物車到指定用戶


            return RedirectToAction("CaseShop");
        }

        // 購物車頁面
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

        //刪除購物車東西
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            int userId = GetAccountID(); // 
            var cart = GetCart(userId); //
            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart, userId); //
            }

            return RedirectToAction("CaseCart");
        }

        // 購物車Session
        private List<CCartItem> GetCart(int userId)
        {
            var cartJson = HttpContext.Session.GetString($"Cart_{userId}");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CCartItem>();
            }

            return JsonSerializer.Deserialize<List<CCartItem>>(cartJson);
        }

        // 保存購物車數據
        private void SaveCart(List<CCartItem> cart, int userId)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString($"Cart_{userId}", cartJson);
        }


        public int GetCartItemCount()
        {
            int userId = GetAccountID(); 
            var cart = GetCart(userId); 
            int itemCount = cart.Sum(item => item.Quantity);
            return itemCount;
        }









    }
}
