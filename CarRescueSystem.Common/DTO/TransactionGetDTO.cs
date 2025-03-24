using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class TransactionGetDTO
    {
        public Guid id { get; set; }

        public decimal amount { get; set; }

        public string? packageName { get; set; }
        public string licensePlate { get; set; }

        public string status { get; set; }
        public DateTime createdAt { get; set; }



    }
}
