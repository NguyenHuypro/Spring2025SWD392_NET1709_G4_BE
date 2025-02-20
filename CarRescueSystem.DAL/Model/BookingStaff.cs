using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class BookingStaff
    {
        [Key]
        public Guid BookingStaffId { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public Guid BookingId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid StaffId { get; set; } // Nhân viên phụ trách

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow; // Thời điểm được giao

        public virtual Booking Booking { get; set; }
        public virtual User Staff { get; set; }
    }

}
