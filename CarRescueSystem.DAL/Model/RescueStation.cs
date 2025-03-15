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
        public Guid id { get; set; }

        [Required, MaxLength(255)]
        public string name { get; set; } // Tên trạm cứu hộ

        [Required, MaxLength(500)]
        public string address { get; set; } // Địa chỉ

        public double latitude { get; set; } // Vĩ độ
        public double longitude { get; set; } // Kinh độ

        [Required]
        public string phone { get; set; } // Số điện thoại liên hệ

        public string email { get; set; } // Email liên hệ

        public virtual ICollection<User> Staffs { get; set; } = new HashSet<User>(); // Danh sách nhân viên làm việc tại trạm

        public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>(); // Booking liên quan đến trạm này
    }
}