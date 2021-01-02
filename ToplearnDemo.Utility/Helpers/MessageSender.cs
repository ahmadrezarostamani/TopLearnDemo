using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ToplearnDemo.Utility.Helpers.Interface;

namespace ToplearnDemo.Utility.Helpers
{
    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string emailBody, bool isHtml = false)
        {
            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName = "ahmadreza.rostamani",
                    Password = "********"
                };

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toEmail) },
                    From = new MailAddress("ahmadreza.rostamani@gmail.com"),
                    Subject = subject,
                    Body = emailBody,
                    IsBodyHtml = isHtml
                };

                client.Send(emailMessage);
            }

            return Task.CompletedTask;
        }
    }
}
