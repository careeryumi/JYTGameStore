using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class GameRatings
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int gameRatingId { get; set; }

        public string userId { get; set; }
        [ForeignKey("userId")]
        public virtual ApplicationUser User { get; set; }

        public int? gameId { get; set; }
        [ForeignKey("gameId")]
        public virtual Game Game { get; set; }

        public int gameRating { get; set; }
    }
}
