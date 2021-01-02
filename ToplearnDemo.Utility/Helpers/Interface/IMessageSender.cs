using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace ToplearnDemo.Utility.Helpers.Interface
{
    public interface IMessageSender
    {
        Task SendEmailAsync(string toEmail, string subject, string emailBody, bool isHtml = false);
    }
}
