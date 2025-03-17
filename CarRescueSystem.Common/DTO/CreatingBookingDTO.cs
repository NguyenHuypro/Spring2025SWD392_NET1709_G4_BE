using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class CreatingBookingDTO
    {
        
        //public Guid CustomerId { get; set; }
        public string? customerName { get; set; }

        public Guid? carId { get; set; } // Có thể null nếu khách chưa có xe

      
        public string description { get; set; }

        public string? evidence { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string location { get; set; }
        public string? licensePlate { get; set; }
        public string? phone { get; set; }
    }
}
