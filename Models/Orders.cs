/*
 * Order.cs
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.11.01: Created
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
    public class Orders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Order Number")]
        public int OrderId { get; set; }

        [Display(Name = "User ID")]
        [Required]
        [StringLength(256, ErrorMessage = "User ID cannot be longer than 256 characters.")]
        public string UserId { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        // 0: Pending
        // 1: Purchased
        // 2: Proceed
        // 3. Completed
        [Display(Name = "Order Status")]
        [Required]
        [StringLength(1, ErrorMessage = "Order Status cannot be longer than 1 characters.")]
        public string OrderStatus { get; set; }

        [Display(Name = "Is Physical Shipping")]
        [Required]
        // [StringLength(1, ErrorMessage = "Is Physical Shipping cannot be longer than 1 characters.")]
        public int IsPhysicalShipping { get; set; }

        [Display(Name = "Total Amount")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(0, 9999999999)]
        public float? TotalAmount { get; set; }

        [Display(Name = "Credit Card")]
        [Required]
        public int CreditCardId { get; set; }
        public virtual CreditCard CreditCard { get; set; }
    }

    
    // Order Status Values
    public enum OrderStatusValues
    {
        Pending = 0,
        Purchased = 1,
        Processed = 2,
        Completed = 3
    }
}
