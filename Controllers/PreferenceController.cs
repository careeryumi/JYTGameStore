/*
 * PreferenceController.cs (Controller)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.26: Created
 *      Jeonghwan Ju, 2021.10.27: Updated
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [Authorize(Roles = "Admin, Employee, Member")]
    public class PreferenceController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public PreferenceController(ApplicationDbContext db, 
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            dbContext = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // Check login status
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            // ViewBag
            ViewBag.Title = User.Identity.Name + "'s Preference";

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;

            // Model
            dynamic preferenceModel = new ExpandoObject();

            // Flatform
            var favoritePlatformList = dbContext.Platform.Select(p => new CheckBoxItem()
            {
                CheckId = p.PlatformId,
                CheckTitle = p.PlatformName,
                IsChecked = dbContext.FavoritePlatform
                            .Where(m => m.UserId == userId)
                            .Any(m => m.PlatformId == p.PlatformId) ? true : false
            }).ToList();

            // Category
            var favoriteCategoryList = dbContext.Category.Select(c => new CheckBoxItem()
            {
                CheckId = c.CategoryId,
                CheckTitle = c.CategoryName,
                IsChecked = dbContext.FavoriteCategory
                            .Where(m => m.UserId == userId)
                            .Any(m => m.CategoryId == c.CategoryId) ? true : false
            }).ToList();

            // Add to preferenceModel
            preferenceModel.favoritePlatformList = favoritePlatformList;            
            preferenceModel.favoriteCategoryList = favoriteCategoryList;

            return View(preferenceModel);
        }

        // Update Favorite Platform 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateFavoritePlatformAsync(string[] checkFavoritePlatform)
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;
            
            // Add checked data
            foreach (var item in checkFavoritePlatform)
            {   
                // If it is not exist, add to database.
                if (!dbContext.FavoritePlatform
                            .Where(m => m.UserId == userId)
                            .Any(m => m.PlatformId == int.Parse(item)))
                {
                    FavoritePlatform favoritePlatform = new FavoritePlatform();

                    favoritePlatform.UserId = userId;
                    favoritePlatform.PlatformId = int.Parse(item);

                    dbContext.FavoritePlatform.Add(favoritePlatform);
                    dbContext.SaveChanges();
                }
            }

            // Delete unchecked data
            // Unchecked value
            List<string> uncheckedPlatformIdList = new List<string>();
            var platform = dbContext.Platform.OrderBy(a => a.PlatformId);
            foreach (var p in platform)
            {
                if (!checkFavoritePlatform.Contains(p.PlatformId.ToString()))
                {
                    uncheckedPlatformIdList.Add(p.PlatformId.ToString());
                }
            }

            foreach (var deletePlatformId in uncheckedPlatformIdList)
            {
                var deleteFavoritePlatform = dbContext.FavoritePlatform
                    .FirstOrDefault(m => m.UserId == userId && m.PlatformId == int.Parse(deletePlatformId));
                if (deleteFavoritePlatform != null)
                {
                    dbContext.FavoritePlatform.Remove(deleteFavoritePlatform);
                    dbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }


        // Update Favorite Category 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateFavoriteCategoryAsync(string[] checkFavoriteCategory)
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;

            // Add checked data
            foreach (var item in checkFavoriteCategory)
            {
                // If it is not exist, add to database.
                if (!dbContext.FavoriteCategory
                            .Where(m => m.UserId == userId)
                            .Any(m => m.CategoryId == int.Parse(item)))
                {
                    FavoriteCategory favoriteCategory = new FavoriteCategory();

                    favoriteCategory.UserId = userId;
                    favoriteCategory.CategoryId = int.Parse(item);

                    dbContext.FavoriteCategory.Add(favoriteCategory);
                    dbContext.SaveChanges();
                }
            }

            // Delete unchecked data
            // Unchecked value
            List<string> uncheckedCategoryIdList = new List<string>();
            var category = dbContext.Category.OrderBy(a => a.CategoryName);
            foreach (var c in category)
            {
                if (!checkFavoriteCategory.Contains(c.CategoryId.ToString()))
                {
                    uncheckedCategoryIdList.Add(c.CategoryId.ToString());
                }
            }

            foreach (var deleteCategoryId in uncheckedCategoryIdList)
            {
                var deleteFavoriteCategory = dbContext.FavoriteCategory
                    .FirstOrDefault(m => m.UserId == userId && m.CategoryId == int.Parse(deleteCategoryId));
                if (deleteFavoriteCategory != null)
                {
                    dbContext.FavoriteCategory.Remove(deleteFavoriteCategory);
                    dbContext.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
