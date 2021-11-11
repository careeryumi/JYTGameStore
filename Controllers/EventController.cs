using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using JYTGameStore.Data;

namespace JYTGameStore.Controllers
{
    [Authorize(Roles = "Admin,Employee,Member")]
    public class EventController : Controller
    {
        private readonly JYTGameStoreDBContext _context;
        private readonly ApplicationDbContext _context1;
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public EventController(JYTGameStoreDBContext context, ApplicationDbContext context1, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _context1 = context1;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        // GET: Event
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.OrderBy(x => x.Name).ToListAsync());
        }
        [AllowAnonymous]
        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? eventId)
        {
            if (eventId == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == eventId);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Event/Create
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create([Bind("EventId,Name,Description,StartDate,EndDate,PublishDate,Publisher")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Event/Edit/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Description,StartDate,EndDate,PublishDate,Publisher")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            return View(@event);
        }

        // GET: Event/Delete/5
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }

        [Authorize]
        public ActionResult Add(int id)
        {
            RegisterEvent eventlist = new RegisterEvent();
            eventlist.EventId = id;
            eventlist.Email = _context1.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
            eventlist.RegisterDate = DateTime.Now;
            // email && id=true false
            var ifExist = _context1.RegisterEvents.Where(e => e.Email == eventlist.Email && e.EventId == eventlist.EventId);

            if (ifExist.Count() != 0) //check if exist
            {
                TempData["message"] = "You already enrolled this event.";
                return RedirectToAction("Index", "Event");
            }
            TempData["message"] = "Successfully enrolled.";
            _context1.RegisterEvents.Add(eventlist);
            _context1.SaveChanges();
            return RedirectToAction("Index", "Event");
        }
    }
}
