using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class AllInOneBookingDTO
    {
        public string status { get; set; }
        public CreatingBookingDTO? CreatingBookingDto { get; set; }
        public List<Guid>? StaffIds { get; set; }
        public List<Guid>? ServiceIds { get; set; }
    }
}
