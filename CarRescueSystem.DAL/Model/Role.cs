using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class Role
    {
        [Key]
        public Guid RoleID { get; set; }

        [Required, MaxLength(50)]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
