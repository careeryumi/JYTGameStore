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

namespace JYTGameStore.Controllers
{
    public class WishListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WishLists
        public async Task<IActionResult> Index()
        {
            string email = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
            //var userEmail = _context.Users.ToList();
            var applicationDbContext = _context.WishList.Include(w => w.Game).Where(m => m.Email == email);

            //This is for wish list adding to view bag share on index
            var wishListForTweet = applicationDbContext.ToList();
            var wishListString ="My wish list: ";
            var itemNo = 0;

            foreach (var item in wishListForTweet)
            {
                itemNo = itemNo + 1;
                wishListString += itemNo.ToString() +". "+ item.Game.GameName.ToString() +" ";
            }
            ViewBag.WishListForTweeter = wishListString;

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WishLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .Include(w => w.Game)
                .FirstOrDefaultAsync(m => m.wishListId == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // GET: WishLists/Create
        public IActionResult Create()
        {
            ViewData["gameId"] = new SelectList(_context.Game, "gameId", "GameDescription");
            return View();
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("wishListId,gameId,Email")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["gameId"] = new SelectList(_context.Game, "gameId", "GameDescription", wishList.gameId);
            return View(wishList);
        }

        // GET: WishLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList.FindAsync(id);
            if (wishList == null)
            {
                return NotFound();
            }
            ViewData["gameId"] = new SelectList(_context.Game, "gameId", "GameDescription", wishList.gameId);
            return View(wishList);
        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("wishListId,gameId,Email")] WishList wishList)
        {
            if (id != wishList.wishListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListExists(wishList.wishListId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["gameId"] = new SelectList(_context.Game, "gameId", "GameDescription", wishList.gameId);
            return View(wishList);
        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishList
                .Include(w => w.Game)
                .FirstOrDefaultAsync(m => m.wishListId == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishList = await _context.WishList.FindAsync(id);
            _context.WishList.Remove(wishList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WishListExists(int id)
        {
            return _context.WishList.Any(e => e.wishListId == id);
        }
    }
}
