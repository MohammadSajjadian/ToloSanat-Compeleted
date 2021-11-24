using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public interface IEmail
    {
        Task Send(string subject, string body, string receiver);
    }
}
