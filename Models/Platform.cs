/*
 * Platform.cs (Model)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.26: Created
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class Platform
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Number")]
        public int PlatformId { get; set; }

        [Display(Name = "Platform Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Platform Name cannot be longer than 50 characters.")]
        public string PlatformName { get; set; }
    }
}
