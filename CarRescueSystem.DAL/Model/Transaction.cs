using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        // Khóa ngoại
        [ForeignKey("UserId")]
        public virtual Wallet Wallet { get; set; }
    }
}
