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
        public Guid id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid customerId { get; set; }
        public Guid? vehicleId { get; set; }


        public BookingStatus status { get; set; }
        public string description { get; set; }
        public string evidence { get; set; }
        public string location { get; set; }

        // Thêm tọa độ để tính khoảng cách
        public double? latitude { get; set; }
        public double? longitude { get; set; }

        public decimal? totalPrice { get; set; }

        public virtual User Customer { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
       
        public DateTime bookingDate { get; set; }
        public DateTime? arrivalDate { get; set; } // Có thể null nếu chưa bắt đầu

        public DateTime? completedDate { get; set; }

        //user
        public string? licensePlate { get; set; }
        public string? phone { get; set; }

        

        public Guid? packageId { get; set; }
        public virtual Package? Package { get; set; } // Liên kết với Package

        // Danh sách staff phụ trách booking này
        public virtual ICollection<BookingStaff> BookingStaffs { get; set; } = new HashSet<BookingStaff>();

        
        

        public virtual ICollection<ServiceOfBooking> ServiceBookings { get; set; } = new HashSet<ServiceOfBooking>();

        // 🆕 Thêm quan hệ với RescueStation
        public Guid? rescueStationId { get; set; } // Trạm cứu hộ gần nhất
        public virtual RescueStation? RescueStation { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

    }
    public enum BookingStatus
    {
        DEPOSIT, // KHI CREATE BOOKING - 
        PENDING, // đã thanh toán 100k -> ĐỢI MANAGER ASSIGN STAFF
        COMING, // ASSIGN STAFF XONG
        CHECKING, // 2 THẰNG CONFIRM ĐÃ TỚI - ADD SERVICE - BÁO GIÁ
        IN_PROGRESS, // CUS CHẤP NHẬN SỬA
        CANCELLED, // CUS TỪ CHỐI - TRƯỜNG HỢP KHÁC
        FINISHED, // SỬA XONG - PAY THÀNH CÔNG
        PENDING_PAYMENT
    }
}



