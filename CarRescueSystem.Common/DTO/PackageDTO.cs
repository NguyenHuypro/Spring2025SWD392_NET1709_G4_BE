using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class PackageDTO
    {
       

        [Required, MaxLength(100)]
        public string PackageName { get; set; }

        [Required]
        public decimal PackagePrice { get; set; }

        public List<Guid> ServiceIds { get; set; } 
    }
}
