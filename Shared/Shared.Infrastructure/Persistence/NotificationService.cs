////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using Shared.Domain.ApplicationUsers;
////using Shared.Infrastructure.Persistence;
////using Microsoft.EntityFrameworkCore;
////using Microsoft.Extensions.Logging;
////using Microsoft.Extensions.Configuration;
////using Shared.Infrastructure.Data;

////namespace Shared.Infrastructure.Notifications
////{
////    public class NotificationService : INotificationService
////    {
////        private readonly SharedDbContext _context;

////        public NotificationService(SharedDbContext context)
////        {
////            _context = context;
////        }

////        public async Task NotifyAsync(string IdReceveur, string titre, string message)
////        {
////            var notification = new ViewNotificationPlat
////            {
////                IdReceveur = IdReceveur,
////                Titre = titre,
////                Message = message,
////                DateNotification = DateTime.UtcNow,
////                EstLuChar = "0" // Oracle NUMBER(1,0) stocke 0/1
////            };

////            _context.ViewNotificationPlats.Add(notification);
////            await _context.SaveChangesAsync();
////        }

////        public async Task NotifyByEmailAsync(string IdReceveur, string titre, string message)
////        {
////            // Ici tu peux brancher ton service SMTP existant
////            await Task.CompletedTask;
////        }

////        public async Task MarkAsReadAsync(int IdNotification, string IdReceveur)
////        {
////            var notif = await _context.ViewNotificationPlats
////                .FirstOrDefaultAsync(n => n.IdNotification == IdNotification && n.IdReceveur == IdReceveur);

////            if (notif != null)
////            {
////                notif.EstLuChar = "1"; // 1 pour lu
////                await _context.SaveChangesAsync();
////            }
////        }

////        public async Task<IEnumerable<NotificationDto>> GetUnreadAsync(string IdReceveur)
////        {
////            return await _context.ViewNotificationPlats
////                .Where(n => n.IdReceveur == IdReceveur && n.EstLuChar == "0")
////                .Select(n => new NotificationDto
////                {
////                    IdNotification = n.IdNotification,
////                    Titre = n.Titre,
////                    Message = n.Message,
////                    DateNotification = n.DateNotification
////                })
////                .ToListAsync();
////        }
////    }
////}
