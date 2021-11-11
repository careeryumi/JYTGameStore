/*
 * Province.cs (Model)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.27: Created
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class Province
    {
        [Key]
        [Display(Name = "Province Code")]
        [StringLength(2, ErrorMessage = "Province Code cannot be longer than 2 characters.")]
        public string ProvinceCode { get; set; }

        [Display(Name = "Province Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Province Name cannot be longer than 50 characters.")]
        public string CountryName { get; set; }

        public virtual Country Country { get; set; }
    }
}
