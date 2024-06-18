using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Serilog;
using ABC.Shared.Services;

namespace ABC.Shared.Utility
{
    // EmailSender.cs
    public class EmailSender(string mail, string pass, string smtp, int port) : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var client = new SmtpClient(smtp, port)

            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pass)
            };


            var message = new MailMessage(mail, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };


            await client.SendMailAsync(message);
        }
    }
}