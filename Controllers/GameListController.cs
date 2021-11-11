/*
 *  Revision History
 *      Yumi Lee, Nov 09, 2021: Updated
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Identity;

namespace JYTGameStore.Controllers
{
    public class GameListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public GameListController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        // GET: GameList
        public async Task<IActionResult> Index(string searchString)
        {
            var games = from m in _context.Game
                        select m;
            ViewBag.searchStringOnTextBox = "Search games here";

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.searchStringOnTextBox = searchString;
                games = games.Where(s => s.GameName.Contains(searchString) || s.GameDescription.Contains(searchString));
            }

            return View(await games.ToListAsync());
        }

        // GET: GameList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // getting user id and email
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;
            //string userEmail = user.Email;
            //ViewBag.userEmailWithMasking = userEmail.Substring(0, 3) + "***";

            var sumRatingsForGameId = _context.GameRatings.Where(x => x.gameId == id).Sum(k => k.gameRating);
            var countRatingsForGameId = _context.GameRatings.Where(x => x.gameId == id).Count();
            double dOverallRating = (double)sumRatingsForGameId / (double)countRatingsForGameId;
            string sOverallRating = dOverallRating.ToString("n1");

            //Showing over all
            if (sumRatingsForGameId == 0)
            {
                ViewBag.OverAll = "No ratings yet";
            }
            else
            {
                ViewBag.OverAll = sOverallRating;
            }
            

            //game context
            var gameContext = _context.Game.FirstOrDefault(x => x.gameId == id);

            //if there is no gameId exist then return to the game list
            if (id == null || gameContext == null)
            {
                return RedirectToAction("Index", "GameList", new { area = "" });
            }

            ViewBag.gameId = id;
            ViewBag.gameName = gameContext.GameName;
            ViewBag.gameDescription = gameContext.GameDescription;
            ViewBag.imgUrl = gameContext.imageUrl;

            //Showing selected rating
            var selectedRating = _context.GameRatings.FirstOrDefault(x => x.gameId == id && x.userId == userId);

            if (selectedRating != null)
            {
                var selectedRatingPerUserPerGame = selectedRating.gameRating;
                ViewBag.selectedRatingPerUserPerGame = selectedRatingPerUserPerGame;
            }

            var reviews = _context.GameReview.Where(m => m.gameId == id);
            var reviewsList = reviews.ToList();

            if (reviews == null)
            {
                return NotFound();
            }

            return View(await reviews.ToListAsync());
        }

        public ActionResult AddToWishList([Bind("gameId")] Game game)
        {
            var userEmail = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
            var alreadyInWishList = _context.WishList.FirstOrDefault(m => m.gameId == game.gameId && m.Email == userEmail);

            if (alreadyInWishList != null)
            {
                TempData["Message"] = "The item is already in your wish list";
                return RedirectToAction("Index", "WishLists", new { area = "" }); //go to wish list
            }
            else
            {
                WishList wishObj = new WishList();
                wishObj.gameId = game.gameId;
                wishObj.Email = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
                _context.WishList.Add(wishObj);
                _context.SaveChanges();
                TempData["alreadyAddedInWishList"] = "";
                return RedirectToAction("Index", "WishLists", new { area = "" }); //go to wish list
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddGameRatings(string YourRadioButton, [Bind("gameId")] Game game)
        {
            //getting game id
            var gameId = game.gameId;

            // getting user id
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;

            if (YourRadioButton != null)
            {
                GameRatings gameRatings = new GameRatings();
                gameRatings.gameId = gameId;
                gameRatings.gameRating = Int32.Parse(YourRadioButton);
                gameRatings.userId = userId;

                //getting ratings to check if the rating already exists
                var alreadyRated = _context.GameRatings.AsNoTracking().FirstOrDefault(m => m.userId == userId && m.gameId == gameId);

                if (alreadyRated != null)
                {
                    gameRatings.gameRatingId = alreadyRated.gameRatingId;
                    _context.GameRatings.Update(gameRatings);
                    _context.SaveChanges();
                }
                else
                {
                    _context.GameRatings.Add(gameRatings);
                    _context.SaveChanges();
                }
            }
            else
            {
                TempData["Message"] = "Please select a rating";
            }

            return RedirectToAction("Details", "GameList", new { id = gameId }); //go to the game detail with the current page
        }

        [HttpPost]
        public async Task<ActionResult> AddGameReview(string ReviewTextAreaName, [Bind("gameId")] Game game)
        {
            //getting game id
            var gameId = game.gameId;
            // getting user id
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;

            GameReview gameReview = new GameReview();
            gameReview.userId = userId;
            gameReview.gameId = gameId;
            gameReview.gameReviewDescription = ReviewTextAreaName;
            gameReview.reviewDate2 = DateTime.Now;
            gameReview.isApproved = false;

            _context.GameReview.Add(gameReview);
            _context.SaveChanges();

            TempData["Message"] = "The review has been successfully registered. It will be displayed after checking with the employees.";

            return RedirectToAction("Details", "GameList", new { id = game.gameId }); //go to the game detail with the current page
        }

    }
}
