using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using System.Threading.Tasks;
using TimeManagement.Droid;
using TimeManagement.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TimeManagement.Droid.Notifications))]
namespace TimeManagement.Droid
{
    public class Notifications:INotifications
    {
        public async Task CreateNotificationChannel(string channelName, string channelDescription, string channelId="TimeManagementApp")
        {
            // if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            // {
            //     // Notification channels are new in API 26 (and not a part of the
            //     // support library). There is no need to create a notification
            //     // channel on older versions of Android.
            //     return;
            // }
            //
            // var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default)
            // {
            //     Description = channelDescription
            // };
            //
            // var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            // notificationManager.CreateNotificationChannel(channel);
        }

        public void AddNotification(string title, string text, string channelId = "TimeManagementApp", int notificationId= 0)
        {
            // int defaults = (int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate);
            // int icon = Resource.Drawable.splash_logo;
            // // Set up an intent so that tapping the notifications returns to this app:
            // Intent intent = new Intent(this, typeof(MainActivity));
            // // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            // const int pendingIntentId = 0;
            // PendingIntent pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);
            //
            // // Instantiate the builder and set notification elements:
            // NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "CHANNEL_ID")
            //     .SetContentTitle(title)
            //     .SetContentText(text)
            //     .SetSmallIcon(icon)
            //     .SetDefaults (defaults)
            //     .SetContentIntent (pendingIntent)
            //     .SetSound (RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));;
            //
            // // Build the notification:
            // Notification notification = builder.Build();
            //
            // // Get the notification manager:
            // NotificationManager notificationManager =
            //     GetSystemService (Context.NotificationService) as NotificationManager;
            //
            //
            // builder.SetWhen (Java.Lang.JavaSystem.CurrentTimeMillis());
            //
            // notificationManager.Notify(notificationId, notification);
            new MainActivity().AddNotification("Test", "FUnhuje to?");
        }
    }
}