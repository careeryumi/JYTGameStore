/*
 * CreditCard.cs
 * JYTGameStore Project
 * 
 *  Revision History
 *      Tan Cuong Luong, 2021.11.05: Created
 *      Jeonghwan Ju, 2021.11.07: Updated
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class CreditCard 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Number")]
        public int CreditCardId { get; set; }

        [Display(Name = "Credit Card #")]
        [Required]
        [StringLength(16, ErrorMessage = "Credit Card # Should be 16 digits")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:################}")]
        [CreditCard]
        [Range(1000000000000000, 9999999999999999, ErrorMessage = "Please check the credit card number again")]
        public string CCNumber { get;set; }

        [Display(Name = "Expiry Month")]
        [Required]
        [StringLength(2, ErrorMessage = "Expiry Month Should be 2 digits")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##}")]
        [Range(01, 12, ErrorMessage = "Please check the month again")]
        public string CCMonth { get; set; }

        [Display(Name = "Expiry Year")]
        [Required]
        [StringLength(2, ErrorMessage = "Expiry Year Should be 2 digits")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:##}")]
        [Range(21, 99, ErrorMessage = "Please check the year again")]        
        public string CCYear { get; set; }

        [Display(Name = "PIN")]
        [Required]
        [StringLength(3, ErrorMessage = "PIN Should be 3 digits")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###}")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Your PIN is the 3 digits behind your card")]        
        public string CCPIN { get; set; }

        public string userId { get; set; }
        [ForeignKey("userId")]
        public virtual ApplicationUser User { get; set; }
    }
}