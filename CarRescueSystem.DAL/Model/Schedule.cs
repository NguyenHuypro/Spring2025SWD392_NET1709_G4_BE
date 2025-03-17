using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRescueSystem.DAL.Model
{
    public class Schedule
    {
        [Key]
        [Required]
        public Guid id { get; set; }

        [Required]
        public Guid userId { get; set; } // Nhân viên làm việc

        [ForeignKey("userId")]
        public virtual User Staff { get; set; }

        [Required]
        public DateTime startTime { get; set; }

        [Required]
        public DateTime endTime { get; set; }

        [Required]
        public ShiftType shift { get; set; } // Ca làm sáng/tối
    }

    public enum ShiftType
    {
        MORBING,
        EVENING
    }
}
