using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.ApplicationUsers
{
    public interface INotificationService
    {
        Task NotifyAsync(string IdReceveur, string title, string message);
        Task NotifyByEmailAsync(string IdReceveur, string titre, string message);
        Task MarkAsReadAsync(int IdNotification, string IdReceveur);
        Task<IEnumerable<NotificationDto>> GetUnreadAsync(string IdReceveur);
    }
}
