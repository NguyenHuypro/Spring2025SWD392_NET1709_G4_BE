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
    public class ServiceOfBooking
    {
        [Key]
        public Guid ServiceOfBookingId { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public Guid BookingId { get; set; }

        [Required]
        [ForeignKey("Service")]
        public Guid ServiceId { get; set; }

        [JsonIgnore]
        public virtual Booking Booking { get; set; }
        public virtual Service Service { get; set; }
    }
}
