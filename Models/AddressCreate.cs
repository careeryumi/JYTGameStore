/*
 * AddressCreate.cs (Model)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.29: Created
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class AddressCreate
    {
        ////////////////////////////////////////////////////////////////////////////////
        // Mailing Address
        [Display(Name = "Number")]
        public int MailingAddressId { get; set; }

        [Display(Name = "User ID")]
        [Required]
        [StringLength(256, ErrorMessage = "User ID cannot be longer than 256 characters.")]
        public string MailingUserId { get; set; }

        [Display(Name = "Address Type")]
        [Required]
        [StringLength(1, ErrorMessage = "Address Type cannot be longer than 1 characters.")]
        public string MailingAddressType { get; set; }
        
        [Display(Name = "Street Address")]
        [Required]
        [StringLength(50, ErrorMessage = "Street Address cannot be longer than 50 characters.")]
        public string MailingStreetAddress { get; set; }

        [Display(Name = "Street Address2")]
        [StringLength(60, ErrorMessage = "Street Address2 cannot be longer than 50 characters.")]
        public string MailingStreetAddress2 { get; set; }

        [Display(Name = "City")]
        [Required]
        [StringLength(40, ErrorMessage = "City cannot be longer than 40 characters.")]
        public string MailingCity { get; set; }

        [Display(Name = "Postal Code")]
        [Required]
        [StringLength(6, ErrorMessage = "Postal Code cannot be longer than 6 characters.")]
        [RegularExpression(@"(^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$)", 
            ErrorMessage = "Postal Code is not a valid Canadian postal code.")]
        public string MailingPostalCode { get; set; }

        [Display(Name = "Province")]
        [Required]
        [StringLength(2, ErrorMessage = "Province Code cannot be longer than 2 characters.")]
        public string MailingProvinceCode { get; set; }
        public virtual Province MailingProvince { get; set; }

        [Display(Name = "Country")]
        [Required]
        [StringLength(2, ErrorMessage = "Country Code cannot be longer than 2 characters.")]
        public string MailingCountryCode { get; set; }
        public virtual Country MailingCountry { get; set; }


        ////////////////////////////////////////////////////////////////////////////////
        // Mailing Shipping Same
        [Display(Name = "Mailing and Shipping Address are Same")]
        public bool MailingShippingSame { get; set; }

        ////////////////////////////////////////////////////////////////////////////////
        // Shipping Address
        [Display(Name = "Number")]
        public int ShippingAddressId { get; set; }

        [Display(Name = "User ID")]
        [Required]
        [StringLength(256, ErrorMessage = "User ID cannot be longer than 256 characters.")]
        public string ShippingUserId { get; set; }

        [Display(Name = "Address Type")]
        [Required]
        [StringLength(1, ErrorMessage = "Address Type cannot be longer than 1 characters.")]
        public string ShippingAddressType { get; set; }

        [Display(Name = "Street Address")]
        [Required]
        [StringLength(50, ErrorMessage = "Street Address cannot be longer than 50 characters.")]
        public string ShippingStreetAddress { get; set; }

        [Display(Name = "Street Address2")]
        [StringLength(60, ErrorMessage = "Street Address2 cannot be longer than 50 characters.")]
        public string ShippingStreetAddress2 { get; set; }

        [Display(Name = "City")]
        [Required]
        [StringLength(40, ErrorMessage = "City cannot be longer than 40 characters.")]
        public string ShippingCity { get; set; }

        [Display(Name = "Postal Code")]
        [Required]
        [StringLength(6, ErrorMessage = "Postal Code cannot be longer than 6 characters.")]
        public string ShippingPostalCode { get; set; }

        [Display(Name = "Province")]
        [Required]
        [StringLength(2, ErrorMessage = "Province Code cannot be longer than 2 characters.")]
        public string ShippingProvinceCode { get; set; }
        // public virtual Province ShippingProvince { get; set; }

        [Display(Name = "Country")]
        [Required]
        [StringLength(2, ErrorMessage = "Country Code cannot be longer than 2 characters.")]
        public string ShippingCountryCode { get; set; }
        // public virtual Country ShippingCountry { get; set; }
    }
}
