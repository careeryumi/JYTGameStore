/*
 * Home.cs (Controller)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.28: Created
 *      Jeonghwan Ju, 2021.11.02: Updated
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext db)
        {
            dbContext = db;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // Model
            dynamic homeModel = new ExpandoObject();

            // Event
            DateTime eventDate = DateTime.Now;
            var eventList = dbContext.Events
                // .Where(a => a.StartDate <= eventDate && a.EndDate >= eventDate)
                .OrderByDescending(a => a.EventId).ToList().Take(5);

            // Game Category
            var categoryList = dbContext.Category
                .OrderBy(a => a.CategoryId).ToList();

            // Game Review
            var gameReviewList = dbContext.GameReview
                .Where(a => a.isApproved == true)
                .OrderByDescending(a => a.gameReviewId).ToList().Take(5);

            // Game
            var gameList = dbContext.Game
                .Join(dbContext.Category, g => g.CategoryID, c => c.CategoryId, (g, c) => g)
                .OrderByDescending(g => g.gameId)
                .ToList().Take(9);

            // Add models to 'homeModel'
            homeModel.eventList = eventList;
            homeModel.categoryList = categoryList;
            homeModel.gameReviewList = gameReviewList;
            homeModel.gameList = gameList;

            return View(homeModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
