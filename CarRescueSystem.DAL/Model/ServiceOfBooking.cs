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
        public Guid id { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public Guid bookingId { get; set; }

        [Required]
        [ForeignKey("Service")]
        public Guid serviceId { get; set; }

        [JsonIgnore]
        public virtual Booking Booking { get; set; }
        public virtual Service Service { get; set; }
    }
}
