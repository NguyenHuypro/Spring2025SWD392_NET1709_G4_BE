using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class BookingByUserIdDTO
    {
        public Guid id { get; set; }
        public DateTime? arrivalDate { get; set; } // Thời điểm đặt lịch
        public DateTime? completedDate { get; set; } // Thời điểm kết thúc
        public string Description { get; set; } = "Không xác định"; // Tình trạng
        public string Status { get; set; } = "Không xác định"; // Trạng thái
        public decimal TotalPrice { get; set; } = 0; // Tổng tiền
        public string LicensePlate { get; set; } = "Không xác định"; // Biển số xe
    }

}
