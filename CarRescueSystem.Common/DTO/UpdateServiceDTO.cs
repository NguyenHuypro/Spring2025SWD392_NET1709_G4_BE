using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class UpdateServiceDTO
    {
        public Guid? id { get; set; }
        public string? name { get; set; }
        public decimal? price { get; set; }
    }
}
