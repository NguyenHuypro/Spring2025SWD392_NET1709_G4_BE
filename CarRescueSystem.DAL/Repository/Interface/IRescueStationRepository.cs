﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;

namespace CarRescueSystem.DAL.Repository.Interface
{
    public interface IRescueStationRepository : IGenericRepository<RescueStation>
    {
        Task<List<RescueStation>> GetAllAsync();
    }
}
