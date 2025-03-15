using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class ServicePackage
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        [ForeignKey("Service")]
        public Guid serviceId { get; set; }

        [Required]
        [ForeignKey("Package")]
        public Guid packageID { get; set; }



        public virtual Service Service { get; set; }
        public virtual Package Package { get; set; }
    }
}
