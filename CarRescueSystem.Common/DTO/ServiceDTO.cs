﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class ServiceDTO
    {
        public Guid ServiceId { get; set; } 


        public string ServiceName { get; set; } 


        public decimal ServicePrice { get; set; } 
    }
}
