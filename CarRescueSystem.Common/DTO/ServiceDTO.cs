using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class ServiceDTO
    {
        public Guid ServiceId { get; set; } // Sử dụng cho cả Update và Get

        [Required]
        public string ServiceName { get; set; } // Sử dụng cho Create và Update

        [Required]
        public decimal ServicePrice { get; set; } // Sử dụng cho Create và Update
    }
}
