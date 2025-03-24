using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class CreateStaffDTO
    {
        public string email { get; set; }
        public string fullName { get; set; }
        public string password { get; set; }
        public string passwordConfirmed { get; set; }
        public string phone { get; set; }

        public string role { get; set; }

        public Guid? rescueStation { get; set; }


    }
}
