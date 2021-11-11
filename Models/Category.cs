/*
 * Category.cs (Model)
 * JYTGameStore Project
 * 
 *  Revision History
 *      Jeonghwan Ju, 2021.10.15: Created
 *      Jeonghwan Ju, 2021.10.26: Updated
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
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        [Required]
        [StringLength(50, ErrorMessage = "Category Name cannot be longer than 50 characters.")]
        public string CategoryName { get; set; }
    }
}
