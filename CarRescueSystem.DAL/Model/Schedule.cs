using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRescueSystem.DAL.Model
{
    public class Schedule
    {
        [Key]
        [Required]
        public Guid ScheduleId { get; set; }

        [Required]
        public Guid UserId { get; set; } // Nhân viên làm việc

        [ForeignKey("UserId")]
        public virtual User Staff { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public ShiftType Shift { get; set; } // Ca làm sáng/tối
    }

    public enum ShiftType
    {
        Morning,
        Evening
    }
}
