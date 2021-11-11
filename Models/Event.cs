using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace JYTGameStore.Models
{
    [Table("Event")]
    public partial class Event
    {
        [Key]
        [Column("eventId")]
        [Display(Name="Event ID")]
        public int EventId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Publish Date")]
        public DateTime PublishDate { get; set; }
        [Display(Name = "Publisher")]
        public string Publisher { get; set; }
    }
}
