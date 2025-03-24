using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRescueSystem.BLL.Service.Interface;
using CarRescueSystem.Common.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CarRescueSystem.BLL.Service.Implement
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string token)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Car Rescue System", _emailSettings.From));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = "Xác nhận email";

            // 🔹 Link xác nhận email → Gửi đến API
            string confirmLink = $"https://localhost:7040/api/auth/confirm-email?email={Uri.EscapeDataString(toEmail)}&token={Uri.EscapeDataString(token)}";
            email.Body = new TextPart("html")
            {
                Text = $"<p>Click vào link để xác nhận email:</p> <a href='{confirmLink}'>Xác nhận Email</a>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

    }
}
