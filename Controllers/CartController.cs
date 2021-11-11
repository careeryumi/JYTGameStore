/*
 * CartController.cs
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.18: Created
 *      Jeonghwan Ju, 2021.10.20: Updated
 *      Jeonghwan Ju, 2021.10.21: Updated
 *      Jeonghwan Ju, 2021.11.02: Updated
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [Authorize(Roles = "Admin, Employee, Member")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public CartController(ApplicationDbContext db,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            dbContext = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            // if (Shared.isLogin())
            {
                return Redirect("/");
            }

            // ViewBag
            ViewBag.Title = User.Identity.Name + "'s Cart";

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;

            var cartList = dbContext.Cart
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.cartId).ToList();

            foreach (Cart c in cartList)
            {
                var game = dbContext.Game
                    .FirstOrDefault(m => m.gameId == c.gameId);

                if (game != null)
                {
                    c.Game = game;
                }
            }

            return View(cartList.ToList());
        }

        // Create
        public ActionResult Create([Bind("gameId")] Game game)
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            var gameTemp = dbContext.Game.Find(game.gameId);
            if (gameTemp == null)
            {
                return NotFound();
            }
                        
            Cart cart = new Cart();

            cart.UserId = dbContext.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
            cart.regDate = DateTime.Now;
            cart.gameId = gameTemp.gameId;
            cart.Game = gameTemp;

            // Check duplication in cart
            var cartDuplication = dbContext.Cart.FirstOrDefault(a => a.UserId == cart.UserId && a.gameId == cart.gameId);
            if (cartDuplication == null)
            {
                // cart.gameId
                dbContext.Cart.Add(cart);
                dbContext.SaveChanges();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Delete (Get)
        public IActionResult Delete(int? CartId)
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            var cart = dbContext.Cart.Find(CartId);
            if (cart == null)
            {
                return NotFound();
            }

            dbContext.Remove(cart);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
