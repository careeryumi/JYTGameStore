/*
 * FavoritePlatform.cs (Model)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.26: Created
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class FavoritePlatform
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Number")]
        public int FavoritePlatformId { get; set; }

        [Display(Name = "User ID")]
        [Required]
        [StringLength(256, ErrorMessage = "User ID cannot be longer than 256 characters.")]
        public string UserId { get; set; }

        [Display(Name = "Platform")]
        public int PlatformId { get; set; }
        public virtual Platform Platform { get; set; }
    }
}
