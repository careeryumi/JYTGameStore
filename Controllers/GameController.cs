using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [Authorize(Roles ="Admin,Employee")]
    public class GameController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHostingEnvironment hostingEnvironment;
        
        public GameController(ApplicationDbContext db,
            IHostingEnvironment hostingEnvironment) //dependency injection
        {
            dbContext = db;
            this.hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Game> objList = dbContext.Game.ToList();

            foreach (Game g in objList)
            {
                var cartegory = dbContext.Category.FirstOrDefault(a => a.CategoryId == g.CategoryID);                
                if (cartegory != null)
                {
                    g.Category = cartegory;
                }
            }

            return View(objList.ToList());
        }

        //Create Get: Go to Create view page 
        public IActionResult Create()
        {
            // Category Select List
            ViewBag.GameCategoryID = new SelectList(
                dbContext.Category.OrderBy(a => a.CategoryName).ToList(),
                "CategoryId", "CategoryName");

            return View();
        }

        //Create Post: when the new game is added
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GameCreate gameObj)
        {
            //Validation of fields
            if (ModelState.IsValid)
            {
                // Image 
                string uniqueFileName = null;
                if (gameObj.ImageUpload != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + gameObj.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    gameObj.ImageUpload.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Game newGame = new Game
                {
                    GameName = gameObj.GameName,
                    GameDescription = gameObj.GameDescription,
                    imageUrl = uniqueFileName,
                    Price = gameObj.Price,
                    IsActive = null,
                    releaseDate = DateTime.Now,
                    CategoryID = gameObj.CategoryID
                };

                dbContext.Game.Add(newGame);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(gameObj);
            }
        }

        //Edit Get, for parameter, should use lowercase id
        [HttpGet]
        public IActionResult Edit(int? id, GameCreate editGame)
        {
            if (id == null)
            {
                return NoContent();
            }
            else
            {
                var theGame = dbContext.Game.Find(id);
                if (theGame == null)
                {
                    return NoContent();
                }
                else
                {
                    // Category Select List
                    ViewBag.GameCategoryID = new SelectList(
                        dbContext.Category.OrderBy(a => a.CategoryName).ToList(),
                        "CategoryId", "CategoryName", theGame.CategoryID);

                    editGame = new GameCreate
                    {
                        gameId = theGame.gameId,
                        GameName = theGame.GameName,
                        CategoryID = theGame.CategoryID,
                        Price = theGame.Price,
                        GameDescription = theGame.GameDescription,
                        imageUrl = theGame.imageUrl,
                        IsActive = theGame.IsActive,
                        releaseDate = theGame.releaseDate,
                        Category = theGame.Category
                    };

                    return View(editGame);
                }
            }
        }

        //Edit Post: when the new game is edited
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GameCreate gameObj)
        {
            //Validation of fields
            if (ModelState.IsValid)
            {
                // old game info
                var oldGame = dbContext.Game.Find(gameObj.gameId);

                // Image 
                string uniqueFileName = null;
                if (gameObj.ImageUpload != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + gameObj.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    gameObj.ImageUpload.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                else
                {   
                    uniqueFileName = oldGame.imageUrl;
                }

                oldGame.GameName = gameObj.GameName;
                oldGame.GameDescription = gameObj.GameDescription;
                oldGame.imageUrl = uniqueFileName;
                oldGame.Price = gameObj.Price;
                oldGame.IsActive = gameObj.IsActive;
                oldGame.releaseDate = gameObj.releaseDate;
                oldGame.CategoryID = gameObj.CategoryID;

                dbContext.Game.Update(oldGame);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(gameObj);
            }
        }

        //Delete Get, for parameter, should use lowercase id
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NoContent();
            }
            else
            {
                var theGame = dbContext.Game.Find(id);
                if (theGame == null)
                {
                    return NoContent();
                }
                else
                {
                    // Category Select List
                    ViewBag.GameCategoryID = new SelectList(
                        dbContext.Category.OrderBy(a => a.CategoryName).ToList(),
                        "CategoryId", "CategoryName", theGame.CategoryID);

                    return View(theGame);
                }
            }
        }

        //Delete Post: when the new game is deleted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostDelete(int? gameId)
        {
            var theGame = dbContext.Game.Find(gameId);

            if (theGame == null)
            {
                return NoContent();
            }
            else
            {
                dbContext.Game.Remove(theGame);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

        }
    }
}
