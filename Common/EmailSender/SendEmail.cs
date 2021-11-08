using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Services.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EmailSender
{
    public class SendEmail
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmail email;

        public SendEmail(UserManager<ApplicationUser> _userManager, IEmail _email)
        {
            userManager = _userManager;
            email = _email;
        }

        public void Send(string url, string subject, string message)
        {
            foreach (var item in userManager.Users)
            {
                email.Send(subject, $"{message} <br/> {url}", item.Email);
            }
        }
    }
}
