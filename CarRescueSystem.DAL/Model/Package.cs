using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class Package
    {
        [Key]
        public Guid PackageId { get; set; }

        [Required, MaxLength(100)]
        public string PackageName { get; set; }

        [Required]
        public decimal PackagePrice { get; set; }



        public virtual ICollection<ServicePackage> ServicePackages { get; set; } = new HashSet<ServicePackage>();
        // Danh sách các Booking sử dụng Package này
        
    }
}
