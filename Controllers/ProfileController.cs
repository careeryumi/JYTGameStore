
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string email = _context.Users.FirstOrDefault(e => e.UserName == User.Identity.Name).Email;
            var exist = _context.Profile.Where(x => x.Email == email).FirstOrDefault();
            ViewData["email"] = email;
            ViewData["userName"] = User.Identity.Name;
            if (exist == null)
            {
                ViewData["exist"] = "Create";
                ViewData["firstName"] = Convert.ToString("");
                ViewData["lastName"] = Convert.ToString("");
                ViewData["gender"] = Convert.ToString("");
                ViewData["dob"] = Convert.ToString("");
                ViewData["promotion"] = Convert.ToBoolean(false);
            }
            else
            {
                ViewData["exist"] = "Edit";
                var userProfile = _context.Profile.Select(p => new Profile()
                {
                    Email = p.Email,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    DOB = p.DOB,
                    IsPromotion = p.IsPromotion
                }).Where(x => x.Email == email).SingleOrDefault();
                ViewData["firstName"] = Convert.ToString(userProfile.FirstName);
                ViewData["lastName"] = Convert.ToString(userProfile.LastName);
                ViewData["gender"] = Convert.ToString(userProfile.Gender);
                if (userProfile.DOB == null)
                {
                    ViewData["dob"] = "";
                }
                else
                {
                    ViewData["dob"] = Convert.ToDateTime(userProfile.DOB).ToString("yyyy-MM-dd");
                }
                ViewData["promotion"] = Convert.ToBoolean(userProfile.IsPromotion);
            }
            return View();
        }

        public IActionResult Create()
        {
            string email = _context.Users.FirstOrDefault(e => e.UserName == User.Identity.Name).Email;
            ViewData["userName"] = User.Identity.Name;
            ViewData["email"] = email;

            var genders = from GenderList e in Enum.GetValues(typeof(GenderList))
                          select new
                          {
                              Id = e,
                              Name = e.ToString()
                          };

            ViewBag.Gender = new SelectList(genders, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,UserName,FirstName,LastName,Gender,DOB,IsPromotion")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }


        public async Task<IActionResult> Edit()
        {
            string email = _context.Users.FirstOrDefault(e => e.UserName == User.Identity.Name).Email;
            if (email == null)
            {
                return NotFound();
            }
            var profile = await _context.Profile.FindAsync(email);
            if (profile == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["email"] = email;
                ViewData["userName"] = User.Identity.Name;
                var userProfile = _context.Profile.Select(p => new Profile()
                {
                    Email = p.Email,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Gender = p.Gender,
                    DOB = p.DOB,
                    IsPromotion = p.IsPromotion
                }).Where(x => x.Email == email).SingleOrDefault();
                ViewData["firstName"] = Convert.ToString(userProfile.FirstName);
                ViewData["lastName"] = Convert.ToString(userProfile.LastName);
                var genders = from GenderList e in Enum.GetValues(typeof(GenderList))
                              select new
                              {
                                  Id = e,
                                  Name = e.ToString()
                              };

                ViewBag.Gender = new SelectList(genders, "Id", "Name",userProfile.Gender);
                if (userProfile.DOB == null)
                {
                    ViewData["dob"] = "";
                }
                else
                {
                    ViewData["dob"] = Convert.ToDateTime(userProfile.DOB).ToString("yyyy-MM-dd");
                }
                
                ViewData["promotion"] = Convert.ToBoolean(userProfile.IsPromotion);
            }
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Email,UserName,FirstName,LastName,Gender,DOB,IsPromotion")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                _context.Update(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "Profile");
            }
            return View(profile);
        }
    }
}
