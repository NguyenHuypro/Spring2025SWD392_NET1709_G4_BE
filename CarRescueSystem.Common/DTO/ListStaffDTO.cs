using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class ListStaffDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Phone {  get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string RescueStationName { get; set; }

    }
}
