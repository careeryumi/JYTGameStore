/*
 * Util.cs
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.21: Created
 */
using JYTGameStore.Data;
using JYTGameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
/*
namespace JYTGameStore.Utils
{
    public class Util : Controller
    {
        public ApplicationDbContext dbContext;
        public SignInManager<IdentityUser> signInManager;
        public ClaimsPrincipal user;

        Util(ApplicationDbContext db, SignInManager<IdentityUser> signInManager)
        {
            dbContext = db;
            signInManager = signInManager;
            
        }

        public static bool isLogin() 
        {
            return signInManager.IsSignedIn(User);
            // return signInManager.IsSignedIn(User);
        }

        public static string getUserEmail()
        {
            return dbContext.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
            // return dbContext.Users.FirstOrDefault(m => m.UserName == User.Identity.Name).Email;
        }
    }
}
*/