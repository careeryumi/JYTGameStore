/*
 *  Revision History
 *      Yumi Lee, Nov 09, 2021: Updated
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    public class ReviewGameReviewsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public ReviewGameReviewsController(ApplicationDbContext db,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            dbContext = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        // GET: Review List
        public async Task<IActionResult> Index()
        {
            // getting user id
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;

            var ReviewGameContext = dbContext.GameReview.Include(w => w.Game).OrderByDescending(k=>k.gameReviewId);

            return View(await ReviewGameContext.ToListAsync());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ApproveGameReviews(string[] AreChecked)
        {
            if (AreChecked.Count() != 0)
            {
                
                int checkedId;
                //List<GameReview> reviewList = new List<GameReview>();

                foreach (var item in AreChecked)
                {
                    GameReviewModel gameReviewModel = new GameReviewModel();
                    checkedId = Int32.Parse(item);

                    var reviewContext = dbContext.GameReview.AsNoTracking().FirstOrDefault(x=>x.gameReviewId == checkedId);
                    reviewContext.isApproved = true;
                    dbContext.GameReview.Update(reviewContext);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("index", "ReviewGameReviews");
        }
    }
}
