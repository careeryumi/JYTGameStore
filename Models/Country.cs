/*
 * Country.cs (Model)
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
    public class Country
    {
        [Key]
        [Display(Name = "Country Code")]
        [StringLength(2, ErrorMessage = "Country Code cannot be longer than 2 characters.")]
        public string CountryCode { get; set; }

        [Display(Name = "Country Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Country Name cannot be longer than 50 characters.")]
        public string CountryName { get; set; }
    }
}
