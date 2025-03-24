using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class RescueStationDTO
    {
        public Guid id { get; set; }

        
        public string name { get; set; } 


        public string address { get; set; } 

        public string phone { get; set; } // Số điện thoại liên hệ

        public string email { get; set; } // Email liên hệ

    }
}
