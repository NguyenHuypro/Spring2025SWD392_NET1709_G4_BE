using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class RescueStation
    {
        [Key]
        public Guid RescueStationId { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; } // Tên trạm cứu hộ

        [Required, MaxLength(500)]
        public string Address { get; set; } // Địa chỉ

        public double Latitude { get; set; } // Vĩ độ
        public double Longitude { get; set; } // Kinh độ

        [Required]
        public string PhoneNumber { get; set; } // Số điện thoại liên hệ

        public string Email { get; set; } // Email liên hệ

        public virtual ICollection<User> Staffs { get; set; } = new HashSet<User>(); // Danh sách nhân viên làm việc tại trạm

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>(); // Booking liên quan đến trạm này
    }
}