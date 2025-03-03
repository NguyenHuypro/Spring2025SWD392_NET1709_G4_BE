using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.DAL.Model;
using Org.BouncyCastle.Bcpg.Sig;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IOsmService
    {
        Task<LocationData?> GetCoordinatesFromAddressAsync(string address);
    }

}
