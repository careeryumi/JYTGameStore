/*
 * AddressController.cs (Controller)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.27: Created
 *      Jeonghwan Ju, 2021.10.28: Updated
 *      Jeonghwan Ju, 2021.10.29: Updated
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [Authorize(Roles = "Admin, Employee, Member")]
    public class AddressController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;

        public AddressController(ApplicationDbContext db,
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
            ViewBag.Title = User.Identity.Name + "'s Address";

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;

            // Model
            AddressCreate addressCreate = new AddressCreate();

            // Initialize
            addressCreate.MailingAddressId = 0;
            addressCreate.MailingUserId = userId;
            addressCreate.MailingAddressType = "M";
            addressCreate.MailingStreetAddress = "";
            addressCreate.MailingStreetAddress2 = "";
            addressCreate.MailingCity = "";
            addressCreate.MailingPostalCode = "";
            addressCreate.MailingProvinceCode = "ON";
            addressCreate.MailingCountryCode = "CA";

            addressCreate.ShippingAddressId = 0;
            addressCreate.ShippingUserId = userId;
            addressCreate.ShippingAddressType = "S";
            addressCreate.ShippingStreetAddress = "";
            addressCreate.ShippingStreetAddress2 = "";
            addressCreate.ShippingCity = "";
            addressCreate.ShippingPostalCode = "";
            addressCreate.ShippingProvinceCode = "ON";
            addressCreate.ShippingCountryCode = "CA";

            // MailingAddress
            var mailingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "M");
            if (mailingAddress != null)
            {
                addressCreate.MailingAddressId = mailingAddress.AddressId;
                addressCreate.MailingUserId = mailingAddress.UserId;
                addressCreate.MailingAddressType = mailingAddress.AddressType;
                addressCreate.MailingStreetAddress = mailingAddress.StreetAddress;
                addressCreate.MailingStreetAddress2 = mailingAddress.StreetAddress2;
                addressCreate.MailingCity = mailingAddress.City;
                addressCreate.MailingPostalCode = mailingAddress.PostalCode;
                addressCreate.MailingProvinceCode = mailingAddress.ProvinceCode;
                addressCreate.MailingCountryCode = mailingAddress.CountryCode;
            }

            // ShippingAddress
            var shippingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "S");
            if (shippingAddress != null)
            {
                addressCreate.ShippingAddressId = shippingAddress.AddressId;
                addressCreate.ShippingUserId = shippingAddress.UserId;
                addressCreate.ShippingAddressType = shippingAddress.AddressType;
                addressCreate.ShippingStreetAddress = shippingAddress.StreetAddress;
                addressCreate.ShippingStreetAddress2 = shippingAddress.StreetAddress2;
                addressCreate.ShippingCity = shippingAddress.City;
                addressCreate.ShippingPostalCode = shippingAddress.PostalCode;
                addressCreate.ShippingProvinceCode = shippingAddress.ProvinceCode;
                addressCreate.ShippingCountryCode = shippingAddress.CountryCode;
            }

            // mailing and shipping address are the same
            if (addressCreate.MailingAddressId != 0 && addressCreate.ShippingAddressId != 0)
            {
                if ((addressCreate.MailingStreetAddress == addressCreate.ShippingStreetAddress) &&
                    (addressCreate.MailingStreetAddress2 == addressCreate.ShippingStreetAddress2) &&
                    (addressCreate.MailingCity == addressCreate.ShippingCity) &&
                    (addressCreate.MailingPostalCode == addressCreate.ShippingPostalCode) &&
                    (addressCreate.MailingProvinceCode == addressCreate.ShippingProvinceCode) &&
                    (addressCreate.MailingCountryCode == addressCreate.ShippingCountryCode))
                {
                    ViewBag.MailingShippingSame = true;
                }
                else
                {
                    ViewBag.MailingShippingSame = false;
                }
            }

            // Select List            
            ViewBag.MailingProvinceCode = new SelectList(
                dbContext.Province.Where(a => a.Country.CountryCode == addressCreate.MailingCountryCode), 
                "ProvinceCode", "CountryName", addressCreate.MailingProvinceCode);

            ViewBag.ShippingProvinceCode = new SelectList(
                dbContext.Province.Where(a => a.Country.CountryCode == addressCreate.ShippingCountryCode),
                "ProvinceCode", "CountryName", addressCreate.ShippingProvinceCode);

            return View(addressCreate);
        }

        public async Task<IActionResult> EditAsync()
        {
            // Check login status
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            // Default Object
            Province defaultProvince = new Province();
            Country defaultCountry = new Country();

            // ViewBag
            ViewBag.Title = User.Identity.Name + "'s Address";

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;

            // Model
            AddressCreate addressCreate = new AddressCreate();

            // Initialize
            addressCreate.MailingAddressId = 0;
            addressCreate.MailingUserId = userId;
            addressCreate.MailingAddressType = "M";
            addressCreate.MailingStreetAddress = "";
            addressCreate.MailingStreetAddress2 = "";
            addressCreate.MailingCity = "";
            addressCreate.MailingPostalCode = "";
            addressCreate.MailingProvinceCode = "ON";
            addressCreate.MailingCountryCode = "CA";

            addressCreate.ShippingAddressId = 0;
            addressCreate.ShippingUserId = userId;
            addressCreate.ShippingAddressType = "S";
            addressCreate.ShippingStreetAddress = "";
            addressCreate.ShippingStreetAddress2 = "";
            addressCreate.ShippingCity = "";
            addressCreate.ShippingPostalCode = "";
            addressCreate.ShippingProvinceCode = "ON";
            addressCreate.ShippingCountryCode = "CA";

            // MailingAddress
            var mailingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "M");
            if (mailingAddress != null)
            {
                addressCreate.MailingAddressId = mailingAddress.AddressId;
                addressCreate.MailingUserId = mailingAddress.UserId;
                addressCreate.MailingAddressType = mailingAddress.AddressType;
                addressCreate.MailingStreetAddress = mailingAddress.StreetAddress;
                addressCreate.MailingStreetAddress2 = mailingAddress.StreetAddress2;
                addressCreate.MailingCity = mailingAddress.City;
                addressCreate.MailingPostalCode = mailingAddress.PostalCode;
                addressCreate.MailingProvinceCode = mailingAddress.ProvinceCode;
                addressCreate.MailingCountryCode = mailingAddress.CountryCode;
            }

            // ShippingAddress
            var shippingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "S");
            if (shippingAddress != null)
            {
                addressCreate.ShippingAddressId = shippingAddress.AddressId;
                addressCreate.ShippingUserId = shippingAddress.UserId;
                addressCreate.ShippingAddressType = shippingAddress.AddressType;
                addressCreate.ShippingStreetAddress = shippingAddress.StreetAddress;
                addressCreate.ShippingStreetAddress2 = shippingAddress.StreetAddress2;
                addressCreate.ShippingCity = shippingAddress.City;
                addressCreate.ShippingPostalCode = shippingAddress.PostalCode;
                addressCreate.ShippingProvinceCode = shippingAddress.ProvinceCode;
                addressCreate.ShippingCountryCode = shippingAddress.CountryCode;
            }

            // mailing and shipping address are the same
            if (addressCreate.MailingAddressId != 0 && addressCreate.ShippingAddressId != 0)
            {
                if ((addressCreate.MailingStreetAddress == addressCreate.ShippingStreetAddress) &&
                    (addressCreate.MailingStreetAddress2 == addressCreate.ShippingStreetAddress2) &&
                    (addressCreate.MailingCity == addressCreate.ShippingCity) &&
                    (addressCreate.MailingPostalCode == addressCreate.ShippingPostalCode) &&
                    (addressCreate.MailingProvinceCode == addressCreate.ShippingProvinceCode) &&
                    (addressCreate.MailingCountryCode == addressCreate.ShippingCountryCode))
                {
                    ViewBag.MailingShippingSame = true;
                }
                else
                {
                    ViewBag.MailingShippingSame = false;
                }
            }

            // Select List            
            ViewBag.MailingProvinceCode = new SelectList(
                dbContext.Province.Where(a => a.Country.CountryCode == addressCreate.MailingCountryCode),
                "ProvinceCode", "CountryName", addressCreate.MailingProvinceCode);

            ViewBag.ShippingProvinceCode = new SelectList(
                dbContext.Province.Where(a => a.Country.CountryCode == addressCreate.ShippingCountryCode),
                "ProvinceCode", "CountryName", addressCreate.ShippingProvinceCode);

            return View(addressCreate);
        }

        // Save Address
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAddressAsync(AddressCreate addressCreate)
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }
                        
            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;

            ////////////////////////////////////////////////////////////////////////////////
            // Save Mailing Address
            
            Address updateMailingAddress = new Address();

            updateMailingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "M");

            // Update or Insert
            if (updateMailingAddress != null)
            {
                updateMailingAddress.UserId = addressCreate.MailingUserId;
                updateMailingAddress.AddressType = addressCreate.MailingAddressType;
                updateMailingAddress.StreetAddress = addressCreate.MailingStreetAddress;
                updateMailingAddress.StreetAddress2 = addressCreate.MailingStreetAddress2;
                updateMailingAddress.City = addressCreate.MailingCity;
                updateMailingAddress.PostalCode = addressCreate.MailingPostalCode;
                updateMailingAddress.ProvinceCode = addressCreate.MailingProvinceCode;
                updateMailingAddress.CountryCode = addressCreate.MailingCountryCode;

                dbContext.Address.Update(updateMailingAddress);
                dbContext.SaveChanges();
            }
            else
            {
                Address insertMailingAddress = new Address();

                insertMailingAddress.UserId = addressCreate.MailingUserId;
                insertMailingAddress.AddressType = addressCreate.MailingAddressType;
                insertMailingAddress.StreetAddress = addressCreate.MailingStreetAddress;
                insertMailingAddress.StreetAddress2 = addressCreate.MailingStreetAddress2;
                insertMailingAddress.City = addressCreate.MailingCity;
                insertMailingAddress.PostalCode = addressCreate.MailingPostalCode;
                insertMailingAddress.ProvinceCode = addressCreate.MailingProvinceCode;
                insertMailingAddress.CountryCode = addressCreate.MailingCountryCode;

                dbContext.Address.Add(insertMailingAddress);
                dbContext.SaveChanges();
            }

            ////////////////////////////////////////////////////////////////////////////////
            // Save Shipping Address

            Address updateShippingAddress = new Address();

            updateShippingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "S");

            // Update or Insert
            if (updateShippingAddress != null)
            {
                updateShippingAddress.UserId = addressCreate.ShippingUserId;
                updateShippingAddress.AddressType = addressCreate.ShippingAddressType;
                updateShippingAddress.StreetAddress = addressCreate.ShippingStreetAddress;
                updateShippingAddress.StreetAddress2 = addressCreate.ShippingStreetAddress2;
                updateShippingAddress.City = addressCreate.ShippingCity;
                updateShippingAddress.PostalCode = addressCreate.ShippingPostalCode;
                updateShippingAddress.ProvinceCode = addressCreate.ShippingProvinceCode;
                updateShippingAddress.CountryCode = addressCreate.ShippingCountryCode;

                dbContext.Address.Update(updateShippingAddress);
                dbContext.SaveChanges();
            }
            else
            {
                Address insertShippingAddress = new Address();

                insertShippingAddress.UserId = addressCreate.ShippingUserId;
                insertShippingAddress.AddressType = addressCreate.ShippingAddressType;
                insertShippingAddress.StreetAddress = addressCreate.ShippingStreetAddress;
                insertShippingAddress.StreetAddress2 = addressCreate.ShippingStreetAddress2;
                insertShippingAddress.City = addressCreate.ShippingCity;
                insertShippingAddress.PostalCode = addressCreate.ShippingPostalCode;
                insertShippingAddress.ProvinceCode = addressCreate.ShippingProvinceCode;
                insertShippingAddress.CountryCode = addressCreate.ShippingCountryCode;

                dbContext.Address.Add(insertShippingAddress);
                dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }


        // Delete Address
        public async Task<ActionResult> DeleteAddressAsync()
        {
            // Login check
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            // Get userId(Email) of login user
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            string userId = user.Email;

            ////////////////////////////////////////////////////////////////////////////////
            // Delete Mailing Address

            var deleteMailingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "M");

            if (deleteMailingAddress != null)
            {
                dbContext.Address.Remove(deleteMailingAddress);
                dbContext.SaveChanges();
            }

            ////////////////////////////////////////////////////////////////////////////////
            // Delete Shipping Address

            var deleteShippingAddress = dbContext.Address
                            .FirstOrDefault(m => m.UserId == userId && m.AddressType == "S");

            if (deleteShippingAddress != null)
            {
                dbContext.Address.Remove(deleteShippingAddress);
                dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
