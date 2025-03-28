﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.Common.DTO
{
    public class PackageDTO
    {
        public Guid? id { get; set; }

        public string? description { get; set; }

        [Required, MaxLength(100)]
        public string? name { get; set; }

        [Required]
        public decimal? price { get; set; }

        public List<string>? services { get; set; } 
    }
}
