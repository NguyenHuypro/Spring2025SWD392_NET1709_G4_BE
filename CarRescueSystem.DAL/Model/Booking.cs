using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid CustomerId { get; set; }

        [Required]
        [ForeignKey("Vehicle")]
        public Guid? VehicleId { get; set; }


        public BookingStatus Status { get; set; }
        public string Description { get; set; }
        public string Evidence { get; set; }
        public string Location { get; set; }

        public decimal? TotalPrice { get; set; }

        public virtual User Customer { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartAt { get; set; } // Có thể null nếu chưa bắt đầu


        public virtual Package? Package { get; set; } // Liên kết với Package

        // Danh sách staff phụ trách booking này
        public virtual ICollection<BookingStaff> Staffs { get; set; } = new HashSet<BookingStaff>();

        public virtual ICollection<ServiceOfBooking> Services { get; set; } = new HashSet<ServiceOfBooking>();
    }
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        InProgress,
        Cancelled,
        Completed
    }
}
