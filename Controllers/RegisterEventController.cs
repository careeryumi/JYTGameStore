using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [Authorize]
    public class RegisterEventController : Controller
    {
        private readonly JYTGameStoreDBContext _context;
        private readonly ApplicationDbContext _context1;

        public RegisterEventController(JYTGameStoreDBContext context, ApplicationDbContext context1)
        {
            _context = context;
            _context1 = context1;
        }

        public IActionResult Index()
        {
            string email = _context1.Users.FirstOrDefault(e => e.UserName == User.Identity.Name).Email;

            IList<RegisterEvent> registerEvents = _context1.RegisterEvents.ToList().Where(em => em.Email == email).ToList();
            IList<Event> allEvent = _context.Events.ToList();

            IList<Event> inEvent = new List<Event>();
            foreach (var item in registerEvents)
            {
                var event2 = _context.Events.FirstOrDefault(a => a.EventId == item.EventId);
                item.Event = event2;
            }
            return View(registerEvents);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            string email = _context1.Users.FirstOrDefault(e => e.UserName == User.Identity.Name).Email;
            var enrolled = await _context1.RegisterEvents
               .FirstOrDefaultAsync(m => m.EventId == id && m.Email == email);
            _context1.RegisterEvents.Remove(enrolled);
            await _context1.SaveChangesAsync();
            TempData["message"] = "Opt Out Event Success.";
            return RedirectToAction("Index","RegisterEvent");
        }
    }
}