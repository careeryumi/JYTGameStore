using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class GameCreate
    {
        public int gameId { get; set; }

        [Display(Name = "Game Name")]
        [Required]
        public string GameName { get; set; }

        [Display(Name = "Game Description")]
        [Required]
        [StringLength(1000)]
        public string GameDescription { get; set; }

        [Display(Name = "Image URL")]
        public string imageUrl { get; set; }

        [Display(Name = "Image Upload")]
        public IFormFile ImageUpload { get; set; }            // File Upload

        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(0, 9999999999)]
        public float? Price { get; set; }

        public string IsActive { get; set; }

        public DateTime releaseDate { get; set; }

        [Display(Name = "Game Category")]
        [Required]
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }
}
