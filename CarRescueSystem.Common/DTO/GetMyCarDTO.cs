using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class GetMyCarDTO
    {
        public Guid id { get; set; }
        public string model { get; set; }
        public string brand { get; set; }
        public string color { get; set; }
        public int numberOfSeats { get; set; }
        public string licensePlate { get; set; }
        public string expiredDate { get; set; }

        public PackageOfCarDTO package { get; set; }
    }
    public class PackageOfCarDTO
    {
        public Guid id { get; set; }
        public string name { get; set; }

    }
}
