/*
 *  Revision History
 *      Yumi Lee, Nov 09, 2021: Updated
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [Authorize(Roles ="Admin,Employee,Member")]
    public class SelectingGamesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SelectingGamesController(ApplicationDbContext db) //dependency injection
        {
            dbContext = db;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString)
        {
            var games = from m in dbContext.Game
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                games = games.Where(s => s.GameName.Contains(searchString) || s.GameDescription.Contains(searchString));
            }

            return View(await games.ToListAsync());
        }

        //Parameter passed by name in checkbox
        [HttpPost]
        [AllowAnonymous]
        public IActionResult ShowGameDetails(string[] AreChecked)
        {
            if (AreChecked.Count() != 0)
            {
                GameModel gameModel = new GameModel();
                int checkedId;
                List<Game> listtest = new List<Game>();

                foreach (var item in AreChecked)
                {
                    checkedId = Int32.Parse(item);
                    listtest.Add(dbContext.Game.Find(checkedId));
                    gameModel.gameList = listtest;
                }
                return View(gameModel);
            }
            return RedirectToAction("index", "SelectingGames");
        }
    }
}
