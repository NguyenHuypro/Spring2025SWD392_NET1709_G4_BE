using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class Vehicle
    {
        [Key]
        public Guid VehicleId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid CustomerId { get; set; }

        [Required, MaxLength(100)]
        public string VehicleName { get; set; }

        [MaxLength(50)]
        public string VehicleColor { get; set; }

        [Required, MaxLength(100)]
        public string VehicleBrand { get; set; }

        [Required]
        public int NumberOfSeats { get; set; }

        public virtual User Customer { get; set; }
        

    }
}
