using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class RegisterEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Register Event ID")]
        public int RegisterEventId { get; set; }

        [Display(Name = "Event ID")]
        public int EventId { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

        public virtual Event Event { get; set; }

        [Display(Name = "Register Date")]
        public DateTime RegisterDate { get; set; }
    }
}
