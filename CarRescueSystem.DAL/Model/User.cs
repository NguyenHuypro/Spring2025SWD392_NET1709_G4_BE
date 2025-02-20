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
        public virtual ICollection<BookingStaff> AssignedBookings { get; set; } = new HashSet<BookingStaff>();
    }
    public enum StaffStatus
    {
        Active,
        Inactive,
    }
}
