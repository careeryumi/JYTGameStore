using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class GameReview
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int gameReviewId { get; set; }

        public string userId { get; set; }
        [ForeignKey("userId")]
        public virtual ApplicationUser User { get; set; }

        public int? gameId { get; set; }
        [ForeignKey("gameId")]
        public virtual Game Game { get; set; }

        public string gameReviewDescription { get; set; }

        public bool isApproved { get; set; }

        [DataType(DataType.Date)]
        public DateTime reviewDate2 { get; set; }
    }

    public class GameReviewModel
    {
        public List<GameReview> gameReviewList { get; set; }
    }
}
