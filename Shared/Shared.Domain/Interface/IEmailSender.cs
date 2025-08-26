using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.ApplicationUsers
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string titre, string htmlMessage);
    }
}
