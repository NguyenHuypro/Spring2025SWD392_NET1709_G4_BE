using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRescueSystem.BLL.Service.Interface
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string token);
    }

}
