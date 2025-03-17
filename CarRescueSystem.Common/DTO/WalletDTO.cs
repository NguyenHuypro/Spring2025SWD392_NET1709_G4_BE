using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class WalletDTO
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
