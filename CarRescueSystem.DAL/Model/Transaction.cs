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
        public Guid id { get; set; }

        [Required]
        public Guid userId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal amount { get; set; }

        [Required]
        public DateTime createdAt { get; set; }

 
        // ✅ Thêm trạng thái giao dịch
        [Required]
        [MaxLength(20)]
        public TransactionStatus status { get; set; } // Mặc định là "Pending"

        //// Khóa ngoại
        //[ForeignKey("userId")]
        //public virtual Wallet Wallet { get; set; }

        // 🆕 Khóa ngoại liên kết với Booking
        public Guid? bookingId { get; set; }
        [ForeignKey("bookingId")]
        public virtual Booking? Booking { get; set; }

        // 🆕 Khóa ngoại liên kết với Package
        public Guid? packageId { get; set; }
        [ForeignKey("packageId")]
        public virtual Package? Package { get; set; }

        public Guid? carId { get; set; }
        [ForeignKey("carId")]
        public virtual Vehicle? Vehicle { get; set; }

        public enum TransactionStatus
        {
            PENDING,
            SUCCESS,
            FAILED
        }

    }
}
