using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class Vehicle
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid customerId { get; set; }

        [Required, MaxLength(100)]
        public string model { get; set; }

        [MaxLength(50)]
        public string color { get; set; }

        [Required, MaxLength(100)]
        public string brand { get; set; }
        

        [Required]
        public int numberOfSeats { get; set; }
        [Required, MaxLength(15)]
        public string licensePlate { get; set; }
        public virtual User Customer { get; set; }

        // Gói dịch vụ hiện tại của xe (quan hệ 1-N)
        [ForeignKey("Package")]
        public Guid? packageId { get; set; }
        public virtual Package? Package { get; set; }

        // 🆕 Ngày hết hạn của gói dịch vụ
        public DateTime? expirationDate { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();


    }
}
