using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;
using Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class NotificationStorageService : INotificationStorageService
    {
        private readonly ApplicationDbcontext _context;
        private readonly ILogger<Notification> _logger;

        public NotificationStorageService(ApplicationDbcontext context, ILogger<Notification> logger)
        {
            _context = context;
            _logger = logger;
        }



        public async Task<Notification> SaveNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            _logger.LogInformation($" Notification {notification.Id} created for user {notification.UserId}", notification.Id, notification.UserId);
            return notification;
        }

        public async  Task<List<Notification>> GetUndeliveredNotificationsAsync(string userId, int limit = 100)
        {
            var now = DateTime.UtcNow;
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsDelivered  )
                .OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation($" Found {notifications.Count} undelivered notifications for user {userId}", notifications.Count, userId);
            return notifications;
        }

        public async Task MarkAsDeliveredAsync(int notificationId)
        {

            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null && !notification.IsDelivered)
            {
                notification.IsDelivered = true;
                notification.DeliveredAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification {notification.Id} marked as delivered", notificationId);
            }

        }

        public  async Task MarkAsReadAsync(int notificationId)
        {

            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification is not null )
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
           
        }
    }
}
