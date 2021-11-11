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
using Microsoft.AspNetCore.Authorization;

namespace JYTGameStore.Controllers
{
    public class FriendsAndFamilyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FriendsAndFamilyController(ApplicationDbContext context)
        {
            _context = context;

        }

        //string searchedReult = "";
        public IActionResult Index(string searchString)
        {
            var users = from m in _context.Users
                        select m;
            string currentUserEmail = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;


            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Email == searchString);

                var fandflist = _context.FriendsAndFamily.Where(m => m.memberEmail == currentUserEmail && m.FriendsAndFamilyEmail == searchString).FirstOrDefault();

                if (users.Count() == 0)
                {
                    TempData["notMatched"] = "There is no user ID for that search";
                    return View(null);
                }
                else if (searchString == currentUserEmail)
                {
                    TempData["notMatched"] = "Please use a different ID of yours for searching friends and family";
                    return View(null);
                }
                else if (fandflist != null)
                {
                    TempData["notMatched"] = "The ID is already in your friends and family list";
                    return View(null);
                }
            }
            else
            {

                users = users.Where(s => s.Email == "");
            }

            return View(users);
        }

        public async Task<IActionResult> ShowFriendsAndFamilyList()
        {
            string currentUserEmail = _context.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
            var fandflist = _context.FriendsAndFamily.Where(m => m.memberEmail == currentUserEmail).ToListAsync();
            return View(await fandflist);
        }

        //Create Post: when the new game is added
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFriendsAndFamily(string email)
        {
            //Validation of fields
            if (ModelState.IsValid)
            {
                FriendsAndFamily friendAndFamily = new FriendsAndFamily();
                friendAndFamily.FriendsAndFamilyEmail = email;
                string memberEmail = _context.Users.AsNoTracking().FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
                friendAndFamily.memberEmail = memberEmail;

                //Finding reverse friends and family
                var checkReverseFriendsAandFamily = _context.FriendsAndFamily.AsNoTracking().FirstOrDefault(m => m.memberEmail == email && m.FriendsAndFamilyEmail == memberEmail);

                if (checkReverseFriendsAandFamily != null)
                {
                    friendAndFamily.bothAccepted = "Yes";
                }

                _context.FriendsAndFamily.Add(friendAndFamily);
                _context.SaveChanges();

                if (checkReverseFriendsAandFamily != null)
                {
                    //updating the current -reverse friends and family
                    FriendsAndFamily friendAndFamilyReverse = new FriendsAndFamily();
                    friendAndFamilyReverse.friendsAndFamilyId = _context.FriendsAndFamily.AsNoTracking().FirstOrDefault(m => m.memberEmail == email && m.FriendsAndFamilyEmail == memberEmail).friendsAndFamilyId;
                    friendAndFamilyReverse.memberEmail = email;
                    friendAndFamilyReverse.FriendsAndFamilyEmail = memberEmail;
                    friendAndFamilyReverse.bothAccepted = "Yes";
                    _context.FriendsAndFamily.Update(friendAndFamilyReverse);
                    _context.SaveChanges();

                }
                //return RedirectToAction("Index");
                return RedirectToAction("ShowFriendsAndFamilyList", "FriendsAndFamily", new { area = "" }); //go to friends and family list
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendsAndFamily = await _context.FriendsAndFamily
                //.Include(w => w.)
                .FirstOrDefaultAsync(m => m.friendsAndFamilyId == id);
            if (friendsAndFamily == null)
            {
                return NotFound();
            }

            return View(friendsAndFamily);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var friendsAndFamily = await _context.FriendsAndFamily.FindAsync(id);
            _context.FriendsAndFamily.Remove(friendsAndFamily);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowFriendsAndFamilyList));
        }


        // GET: WishLists/Details/5
        public async Task<IActionResult> SeeFriendsAndFamilyWishList(int? id)
        {
            var exist = _context.FriendsAndFamily.FirstOrDefault(x => x.friendsAndFamilyId == id);
            if (id == null || exist == null)
            {
                return NotFound();
            }

            var friendsAndFamilyEmail = _context.FriendsAndFamily.FirstOrDefault(m => m.friendsAndFamilyId == id).FriendsAndFamilyEmail;
            var applicationDbContext = _context.WishList.Include(w => w.Game).Where(m => m.Email == friendsAndFamilyEmail);

            return View(await applicationDbContext.ToListAsync());
        }

    }
}
