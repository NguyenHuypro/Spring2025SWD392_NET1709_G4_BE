using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Id is required")]
        public Guid UserId { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey("Role")]
        public Guid RoleID { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public virtual Role Role { get; set; }


        public StaffStatus ?StaffStatus { get; set; }
        // Quan hệ với BookingStaff (Staff phụ trách bookings)
        public virtual ICollection<BookingStaff> BookingsStaffs { get; set; } = new HashSet<BookingStaff>();
        // Quan hệ N-N với Package thông qua bảng UserPackage
       

        // 🆕 Nhân viên thuộc về một trạm cứu hộ
        public Guid? RescueStationId { get; set; }
        public virtual RescueStation? RescueStation { get; set; }
        // Quan hệ 1-N với bảng Schedule
        public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();
    }
    public enum StaffStatus
    {
        ACTIVE,
        INACTIVE,
    }
}
