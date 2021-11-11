/*
 * OrderItem.cs
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
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Order Item Number")]
        public int OrderItemId { get; set; }

        [Display(Name = "Item Price")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(0, 9999999999)]
        public float ItemPrice { get; set; }

        [Display(Name = "Item Quantity")]
        [DisplayFormat(DataFormatString = "{0}")]
        [Range(0, 9999)]
        public int Quantity { get; set; }

        [Display(Name = "Order")]
        public int OrderId { get; set; }
        // public virtual Orders Orders { get; set; }

        [Display(Name = "Game")]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
