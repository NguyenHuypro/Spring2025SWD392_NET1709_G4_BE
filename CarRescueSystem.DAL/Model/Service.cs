using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class Service
    {
        [Key]
        public Guid ServiceId { get; set; }

        [Required, MaxLength(100)]
        public string ServiceName { get; set; }

        [Required]
        public decimal ServicePrice { get; set; }

        public virtual ICollection<ServiceOfBooking> ServiceBookings { get; set; } = new HashSet<ServiceOfBooking>();
        public virtual ICollection<ServicePackage> ServicePackages { get; set; } = new HashSet<ServicePackage>();
    }
}
