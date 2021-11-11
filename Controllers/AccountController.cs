using DNTCaptcha.Core;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace JYTGameStore.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }

        [AcceptVerbs("Get", "POST")]
        public async Task<IActionResult> IsEmailTaken(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"{email} is already taken");
            }
        }

        [AcceptVerbs("Get", "POST")]
        public async Task<IActionResult> IsUserNameTaken(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"{username} is already taken");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter the security code.",
                    CaptchaGeneratorLanguage = Language.English,
                    CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                

                if (result.Succeeded)
                {
                    var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    await userManager.AddToRoleAsync(user, "Member");

                    var emailUrl = Url.Action("ConfirmEmail", "Account", new { Id = user.Id, token = emailToken }, Request.Scheme);


                    MailMessage mm = new MailMessage();
                    mm.To.Add(new MailAddress(model.Email));
                    mm.From = new MailAddress("JYTGamestore@gmail.com");
                    mm.Subject = "Email Confirmation";
                    mm.Body = $"Hi {user.UserName}, \n You recently registered a account with JYTGameStore. " +
                        $"Click the link below to activite your account.\n\n" +
                        $"{emailUrl}\n\nIf you did not register this account, please ignore this email.\n\nThank you.";
                    SendEmail(mm);
                    TempData["message"] = "Please Confirm your email to login.";
                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, false, true);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("index", "Home");
                    }
                    else if (user.AccessFailedCount >= 3)
                    {
                        await userManager.SetLockoutEndDateAsync(user, DateTime.Now + TimeSpan.FromMinutes(2));
                        ModelState.AddModelError("", "Please try again in 2 mins");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid Email or Password");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Invalid Email or Password");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    MailMessage mm = new MailMessage();
                    mm.To.Add(new MailAddress(model.Email));
                    mm.From = new MailAddress("JYTGamestore@gmail.com");
                    mm.Subject = "Forget Password";
                    mm.Body = $"Hi {user.UserName}, \n You recently requested to reset your password for your account. " +
                        $"Click the link below to reset it.\n\n" +
                        $"{passResetLink}\n\nIf you did not request a password reset, please ignore this email or reply to let us know.\n\nThank you.";

                    mm.IsBodyHtml = false;

                    SendEmail(mm);
                    TempData["message"] = $"Email has been send. Please check {model.Email}.";
                }

                else
                {

                    ModelState.AddModelError("", "Email not found.");
                }
            }
            return RedirectToAction("login", "Account");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (!User.Identity.IsAuthenticated && (token == null || email == null))
            {
                ModelState.AddModelError("", "Invalid password reset token.");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user;
                if (User.Identity.IsAuthenticated)
                {
                    user = await userManager.FindByNameAsync(User.Identity.Name);
                    ViewData["email"] = user.Email;
                    model.Token = await userManager.GeneratePasswordResetTokenAsync(user);
                }
                else
                {
                    user = await userManager.FindByEmailAsync(model.Email);
                }
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        await userManager.SetLockoutEndDateAsync(user, null);
                        return RedirectToAction("index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return RedirectToAction("index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string Id, string token)
        {
            try
            {
                if (Id == null || token == null)
                {
                    ModelState.AddModelError("", "User not found or Invalid email reset token.");
                }
                var user = await userManager.FindByIdAsync(Id);
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData["message"] = "Account activated. You may login now.";
                    return RedirectToAction("Index","Home");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "User not found or Invalid email reset token.");
            }
            return RedirectToAction("Index", "Home");
        }


        public void SendEmail(MailMessage mm)
        {
            var smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("JYTGamestore@gmail.com", "JYTstore2021")
            };
            smtp.Send(mm);
        }
    }
}