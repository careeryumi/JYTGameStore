/*
 * Address.cs (Model)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.28: Created
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Number")]
        public int AddressId { get; set; }

        [Display(Name = "User ID")]
        [Required]
        [StringLength(256, ErrorMessage = "User ID cannot be longer than 256 characters.")]
        public string UserId { get; set; }

        [Display(Name = "Address Type")]
        [Required]
        [StringLength(1, ErrorMessage = "Address Type cannot be longer than 1 characters.")]
        public string AddressType { get; set; }

        [Display(Name = "Street Address")]
        [Required]
        [StringLength(50, ErrorMessage = "Street Address cannot be longer than 50 characters.")]
        public string StreetAddress { get; set; }

        [Display(Name = "Street Address2")]
        [StringLength(60, ErrorMessage = "Street Address2 cannot be longer than 50 characters.")]
        public string StreetAddress2 { get; set; }

        [Display(Name = "City")]
        [Required]
        [StringLength(40, ErrorMessage = "City cannot be longer than 40 characters.")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(6, ErrorMessage = "Postal Code cannot be longer than 6 characters.")]
        public string PostalCode { get; set; }

        [Display(Name = "Province")]
        [StringLength(2, ErrorMessage = "Province Code cannot be longer than 2 characters.")]
        public string? ProvinceCode { get; set; }
        // public virtual Province Province { get; set; }

        [Display(Name = "Country")]
        [StringLength(2, ErrorMessage = "Country Code cannot be longer than 2 characters.")]
        public string? CountryCode { get; set; }
        // public virtual Country Country { get; set; }
        
    }
}
