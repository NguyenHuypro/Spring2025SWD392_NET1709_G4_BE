using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class AddPackageDTO
    {


        public string description { get; set; }

    
        public string name { get; set; }

      
        public string price { get; set; }

        public List<string> services { get; set; }
    }
}
