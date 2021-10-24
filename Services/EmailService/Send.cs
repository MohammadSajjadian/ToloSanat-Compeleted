using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public class Send : IEmail
    {
        void IEmail.Send(string subject, string body, string receiver)
        {
            var mailmessage = new MailMessage("rexongame1@gmail.com", receiver)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("rexongame1@gmail.com", "RexonGame_1100"),
                EnableSsl = true
            };

            smtpClient.Send(mailmessage);
        }
    }
}
