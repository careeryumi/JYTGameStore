using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JYTGameStore.Models
{
    public class WishList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int wishListId { get; set; }

        public int? gameId { get; set; }
        public virtual Game Game { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "nvarchar(max)")]
        public string Email { get; set; }
    }
}
