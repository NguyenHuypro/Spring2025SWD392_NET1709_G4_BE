using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class UpdateUserDTO
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string phone { get; set; }
    }
}
