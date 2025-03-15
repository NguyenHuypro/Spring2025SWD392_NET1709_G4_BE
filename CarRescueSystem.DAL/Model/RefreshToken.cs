using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class RefreshToken
    {
        [Key] // Đánh dấu là khóa chính
        public Guid id { get; set; } // Khóa chính cho RefreshToken

        [Required(ErrorMessage = "UserId is required")]
        public Guid userId { get; set; } // Khóa ngoại liên kết đến người dùng

        [Required(ErrorMessage = "RefreshTokenId is required")]
        public string refreshTokenKey { get; set; } // Giá trị refresh token
        public bool isRevoked { get; set; }

        public DateTime createdAt { get; set; } // Thời gian tạo refresh token
    }
}
