using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class GetAllPackageDTO
    {
        public Guid? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public List<ServiceInPackageDTO> services { get; set; } = new List<ServiceInPackageDTO>();
    }
    public class ServiceInPackageDTO 
    {
        public string name { get; set; }
        public string price { get; set; }
    }


}
