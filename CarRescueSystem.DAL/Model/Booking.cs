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

        
        public Guid? VehicleId { get; set; }


        public BookingStatus Status { get; set; }
        public string Description { get; set; }
        public string Evidence { get; set; }
        public string Location { get; set; }

        // Thêm tọa độ để tính khoảng cách
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public decimal? TotalPrice { get; set; }

        public virtual User Customer { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime? StartAt { get; set; } // Có thể null nếu chưa bắt đầu

        //user

        public string? LicensePlate { get; set; }
        public string? PhoneNumber { get; set; }

       //

        public Guid? PackageId { get; set; }
        public virtual Package? Package { get; set; } // Liên kết với Package

        // Danh sách staff phụ trách booking này
        public virtual ICollection<BookingStaff> BookingStaffs { get; set; } = new HashSet<BookingStaff>();

        public virtual ICollection<ServiceOfBooking> ServiceBookings { get; set; } = new HashSet<ServiceOfBooking>();

        // 🆕 Thêm quan hệ với RescueStation
        public Guid? RescueStationId { get; set; } // Trạm cứu hộ gần nhất
        public virtual RescueStation? RescueStation { get; set; }
    }
    public enum BookingStatus
    {
        PENDING,
        CONFIRMED,
        INPROGRESS,
        CANCELLED,
        COMPLETE
    }
}



//AIzaSyA8SeiRUqFhXUClPTM8Ljw_yTC0ahmRPTE