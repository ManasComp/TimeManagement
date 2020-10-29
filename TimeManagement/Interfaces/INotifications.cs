using System.Threading.Tasks;

namespace TimeManagement.Interfaces
{
    public interface INotifications
    {
        Task CreateNotificationChannel(string channelName, string channelDescription, string channelId="TimeManagementApp");
        void AddNotification(string title, string text, string channelId = "TimeManagementApp", int notificationId = 0);
    }
}