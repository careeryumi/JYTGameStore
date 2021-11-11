/*
 * OrdersController.cs
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.11.09: Created
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
// using System.Web.Mvc;

namespace JYTGameStore.Controllers
{
    [Authorize(Roles = "Admin, Employee, Member")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public OrdersController(ApplicationDbContext db,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            dbContext = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        // Order List
        public async Task<IActionResult> Index()
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            // if (Shared.isLogin())
            {
                return Redirect("/");
            }

            // ViewBag
            ViewBag.Title = User.Identity.Name + "'s Orders";

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;
            string userEmail = user.Email;

            // Orders
            var ordersList = dbContext.Orders
                .Where(a => a.UserId == userEmail && a.OrderStatus != "0")
                .OrderByDescending(a => a.OrderId).ToList();

            // .Where(a => a.UserId == userEmail && a.OrderStatus != "0")

            foreach (Orders o in ordersList)
            {
                var card = dbContext.CreditCard
                    .FirstOrDefault(a => a.CreditCardId == o.CreditCardId);

                if (card != null)
                {
                    o.CreditCard = card;
                }
            }
            
            return View(ordersList.ToList());
        }

        public async Task<IActionResult> CheckoutAsync()
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            // if (Shared.isLogin())
            {
                return Redirect("/");
            }

            // ViewBag
            ViewBag.Title = "Checkout";

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;
            string userEmail = user.Email;

            // Select List (Credit Card)
            var types = new List<SelectListItem>();
            types.Add(new SelectListItem() { Text = "-- Select Credit Card --", Value = string.Empty });

            var creditCardList = dbContext.CreditCard.Where(a => a.userId == userId).OrderBy(a => a.CreditCardId).ToList();
            foreach (CreditCard c in creditCardList)
            {
                // mask credit card number
                var ccNumber = "****" + "-" +
                                c.CCNumber.Substring(4, 4) + "-" +
                                "****" + "-" +
                                c.CCNumber.Substring(12, 4);

                types.Add(new SelectListItem() { Text = ccNumber, Value = c.CreditCardId.ToString() });
            }
            ViewBag.SelectCreditCard = types;

            // Total Amount
            float totalAmount = 0;            

            // Cart
            var cartList = dbContext.Cart
                .Where(c => c.UserId == userEmail)
                .OrderBy(c => c.cartId).ToList();

            foreach (Cart c in cartList)
            {
                var game = dbContext.Game
                    .FirstOrDefault(m => m.gameId == c.gameId);

                if (game != null)
                {
                    c.Game = game;

                    totalAmount += float.Parse(c.Game.Price.ToString());
                }
            }

            @ViewData["userName"] = User.Identity.Name;
            @ViewData["email"] = userEmail;
            @ViewData["totalAmount"] = string.Format("{0:C}", totalAmount);
            @ViewData["cartListData"] = cartList.ToList();            

            return View();
        }

        // PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrderAsync([Bind("CreditCardId")] Orders _orders)
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userEmail = user.Email;
                        
            // Insert Orders info
            Orders orders = new Orders();

            orders.UserId = userEmail;
            orders.OrderDate = System.DateTime.Now;
            orders.OrderStatus = ((int)OrderStatusValues.Pending).ToString();
            orders.IsPhysicalShipping = 0;
            orders.TotalAmount = 0;
            orders.CreditCardId = _orders.CreditCardId;

            dbContext.Orders.Add(orders);
            dbContext.SaveChanges();
            int orderId = orders.OrderId;

            // Insert Order Items from Cart            
            float totalAmount = 0;      // Total Amount

            var cartList = dbContext.Cart
                .Where(c => c.UserId == userEmail)
                .OrderBy(c => c.cartId).ToList();
            
            foreach (Cart c in cartList)
            {
                var game = dbContext.Game
                    .FirstOrDefault(m => m.gameId == c.gameId);

                if (game != null)
                {
                    c.Game = game;

                    // Insert Order Item
                    OrderItem orderItem = new OrderItem();
                    orderItem.ItemPrice = float.Parse(c.Game.Price.ToString());
                    orderItem.Quantity = 1;
                    orderItem.OrderId = orderId;
                    orderItem.GameId = c.Game.gameId;

                    dbContext.OrderItem.Add(orderItem);
                    dbContext.SaveChanges();

                    // Total Amount
                    totalAmount += float.Parse(c.Game.Price.ToString());
                }

                // Delete from cart
                var cartDelete = dbContext.Cart.Find(c.cartId);
                dbContext.Cart.Remove(cartDelete);
                dbContext.SaveChanges();
            }

            // Update Orders info
            var ordersUpdate = dbContext.Orders.FirstOrDefault(a => a.OrderId == orderId);
            if (ordersUpdate == null)
            {
                TempData["message"] = "Order failed. Please, Try again.";
                return RedirectToAction(nameof(CheckoutAsync));
            }

            // ordersUpdate.OrderStatus = OrderStatusList.Purchased.ToString();
            ordersUpdate.OrderStatus = ((int) OrderStatusValues.Purchased).ToString();
            ordersUpdate.TotalAmount = totalAmount;

            dbContext.Orders.Update(ordersUpdate);
            dbContext.SaveChanges();

            @TempData["Message"] = "Your order has been successfully placed.";

            return RedirectToAction(nameof(Index));
        }


    }
}
