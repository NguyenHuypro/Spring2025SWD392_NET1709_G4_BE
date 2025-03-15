using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CarRescueSystem.DAL.Model
{
    public class BookingStaff
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        [ForeignKey("booking")]
        public Guid bookingId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid staffId { get; set; } // Nhân viên phụ trách

        public DateTime assignedAt { get; set; } = DateTime.UtcNow; // Thời điểm được giao

        public bool? confirmStaff { get; set; } = false;

        [JsonIgnore]
        public virtual Booking Booking { get; set; }
        public virtual User Staff { get; set; }
    }

}
