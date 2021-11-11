using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class Profile
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Key]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Gender { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }
        [Display(Name = "Receive Promotion Email")]
        public Boolean IsPromotion { get; set; }
    }
    public enum GenderList
    {
        Male,
        Female,
        None
    }
}
