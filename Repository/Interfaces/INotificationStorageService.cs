using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INotificationStorageService
    {
        Task<Notification> SaveNotificationAsync(Notification notification);
        Task<List<Notification>> GetUndeliveredNotificationsAsync(string userId, int limit = 100);
        Task MarkAsDeliveredAsync(int notificationId);
        Task MarkAsReadAsync(int notificationId);
    }

}
