using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class RoleAndUser
    {
        [Required]
        public string RoleName { get; set; }
    }
}
