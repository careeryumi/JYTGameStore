/*
 * CreditCardsController.cs
 * JYTGameStore Project
 * 
 *  Revision History
 *      Tan Cuong Luong, 2021.11.05: Created
 *      Jeonghwan Ju, 2021.11.07: Updated
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

namespace JYTGameStore.Controllers
{
    public class CreditCardsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        //Validation method
        bool ValidateCreditCardNumber(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return false;
            System.Collections.Generic.IEnumerable<char> rev = cardNumber.Reverse();
            int sum = 0, i = 0;
            foreach (char c in rev)
            {
                if (c < '0' || c > '9')
                    return false;
                int tmp = c - '0';
                if ((i & 1) != 0)
                {
                    tmp <<= 1;
                    if (tmp > 9)
                        tmp -= 9;
                }
                sum += tmp;
                i++;
            }
            // return ((sum % 10) == 0);
            return true;        // checking only numbers
        }
        bool ValidateExpiry(int month, int inputYear)
        {
            string strYear = "20" + Convert.ToString(inputYear);
            int year = Int32.Parse(strYear);
            var DateTimeNow = DateTime.Now;
            var MonthNow = DateTimeNow.Month;
            var YearNow = DateTimeNow.Year;

            if (year >= YearNow)
            {
                if (year > YearNow)
                {
                    return true;
                }

                if (year == YearNow)
                {
                    if (month >= MonthNow)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public CreditCardsController(ApplicationDbContext context, 
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        // GET: CreditCards
        public async Task<IActionResult> Index()
        {
            // getting user id
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;

            // ViewBag
            ViewBag.Title = User.Identity.Name + "'s Credit Card";

            var applicationDbContext = _context.CreditCard.Where(x => x.userId == userId).OrderBy(a => a.CreditCardId);
            var cards = await applicationDbContext.ToListAsync();            
            return View(cards.ToList());
        }

        // GET: CreditCards/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var creditCard = await _context.CreditCard
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CCNumber == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            return View(creditCard);
        }

        // GET: CreditCards/Create
        public IActionResult Create()
        {
            // ViewBag
            ViewBag.Title = "Add New Credit Card";

            return View();
        }

        // POST: CreditCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CCNumber,CCMonth,CCYear,CCPIN")] CreditCard creditCard)
        {
            // getting user id
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;

            //validation
            CreditCard card = new CreditCard();
            card.CCNumber = creditCard.CCNumber;
            card.CCMonth = creditCard.CCMonth;
            card.CCYear = creditCard.CCYear;
            card.CCPIN = creditCard.CCPIN;
            card.userId = userId;

            // Validate 'Credit Card Number' and 'Expire'
            if (ValidateCreditCardNumber(card.CCNumber))
            {
                if (ValidateExpiry(int.Parse(card.CCMonth), int.Parse(card.CCYear)))
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(card);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }            
            
            return View(creditCard);
        }

        // GET: CreditCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // ViewBag
            ViewBag.Title = "Edit Credit Card";

            var creditCard = await _context.CreditCard.FindAsync(id);
            if (creditCard == null)
            {
                return NotFound();
            }

            // mask credit card number
            var ccNumber = "****" +
                            creditCard.CCNumber.Substring(4, 4) +
                            "****" +
                            creditCard.CCNumber.Substring(12, 4);
            creditCard.CCNumber = ccNumber;

            return View(creditCard);
        }

        // POST: CreditCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("CreditCardId,CCNumber,CCMonth,CCYear,CCPIN,userId")] CreditCard creditCard)
        {
            // getting user id
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Id;

            //validation
            CreditCard card = new CreditCard();

            card.CreditCardId = creditCard.CreditCardId;        // PK
            card.CCNumber = creditCard.CCNumber;
            card.CCMonth = creditCard.CCMonth;
            card.CCYear = creditCard.CCYear;
            card.CCPIN = creditCard.CCPIN;
            card.userId = userId;

            // Validate 'Credit Card Number' and 'Expire'
            if (ValidateCreditCardNumber(card.CCNumber))
            {
                if (ValidateExpiry(int.Parse(card.CCMonth), int.Parse(card.CCYear)))
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(creditCard);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!CreditCardExists(creditCard.CCNumber))
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
                }
            }

            return View(creditCard);
        }

        // GET: CreditCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // ViewBag
            ViewBag.Title = "Delete Credit Card";

            var creditCard = await _context.CreditCard
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CreditCardId == id);
            if (creditCard == null)
            {
                return NotFound();
            }

            // mask credit card number
            var ccNumber = "****" + "-" +
                            creditCard.CCNumber.Substring(4, 4) + "-" +
                            "****" + "-" +
                            creditCard.CCNumber.Substring(12, 4);
            creditCard.CCNumber = ccNumber;

            return View(creditCard);
        }

        // POST: CreditCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var creditCard = await _context.CreditCard.FindAsync(id);
            _context.CreditCard.Remove(creditCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CreditCardExists(string id)
        {
            return _context.CreditCard.Any(e => e.CCNumber == id);
        }
    }
}
