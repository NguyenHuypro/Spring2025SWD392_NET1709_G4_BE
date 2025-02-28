using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class ScheduleDTO
    {
        
        public Guid UserId { get; set; }
        public string StaffName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Shift { get; set; }
    }
}
