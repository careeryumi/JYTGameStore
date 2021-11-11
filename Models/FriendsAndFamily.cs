using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class FriendsAndFamily
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int friendsAndFamilyId { get; set; }

        public string memberEmail { get; set; }

        [Display(Name = "Friends and Family Email")]
        public string FriendsAndFamilyEmail { get; set; }

        [Display(Name = "Friend or Family")]
        public string isFriendOrFamily { get; set; }

        public string bothAccepted { get; set; }
    }
}
