using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Id is required")]
        public Guid id { get; set; }

        [Required, MaxLength(100)]
        public string fullName { get; set; }

        public string phone { get; set; }

        [Required, EmailAddress, MaxLength(255)]
        public string email { get; set; }

        public string password { get; set; }

        public string passwordSalt { get; set; }

        [Required]
        public RoleType role { get; set; }

        public staffStatus? staffStatus { get; set; }

        // Quan hệ với BookingStaff (Staff phụ trách bookings)
        public virtual ICollection<BookingStaff> BookingsStaffs { get; set; } = new HashSet<BookingStaff>();

        // Nhân viên thuộc về một trạm cứu hộ
        public Guid? rescueStationId { get; set; }
        public virtual RescueStation? RescueStation { get; set; }

        // Quan hệ 1-N với bảng Schedule
     
    }

    public enum staffStatus
    {
        ACTIVE,
        INACTIVE,
    }

    public enum RoleType
    {
        CUSTOMER,
        STAFF,
        RECEPTIONIST,
        ADMIN,
        MANAGER, 
        GUEST
    }
}
