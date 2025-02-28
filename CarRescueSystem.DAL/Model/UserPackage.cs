using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
        public class UserPackage
        {
            public Guid UserPackageId { get; set; }
            public Guid UserId { get; set; }
            public User User { get; set; }

            public Guid PackageId { get; set; }
            public Package Package { get; set; }

            

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
            public int Quantity { get; set; } // Số lần sử dụng còn lại
        }

}
