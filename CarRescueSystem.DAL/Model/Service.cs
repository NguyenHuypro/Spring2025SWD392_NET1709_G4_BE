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
        public Guid id { get; set; }

        [Required, MaxLength(100)]
        public string name { get; set; }

        [Required]
        public decimal price { get; set; }

        public virtual ICollection<ServiceOfBooking> ServiceOfBookings { get; set; } = new HashSet<ServiceOfBooking>();
        public virtual ICollection<ServicePackage> ServicePackages { get; set; } = new HashSet<ServicePackage>();
    }
}
