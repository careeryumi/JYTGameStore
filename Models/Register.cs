using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class Register
    {
        [Required(ErrorMessage = "UserName is Required")]
        [Display(Name = "User Name")]
        [Remote(action: "IsUserNameTaken", controller: "Account")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email is not valid")]
        [Remote(action: "IsEmailTaken", controller:"Account")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }


    }
}
