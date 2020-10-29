using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using LocalNotifications.Droid;
using TimeManagement.Interfaces;
using Xamarin.Forms;

namespace TimeManagement.Droid
{
    [Activity(Label = "TimeManagement"), ]
    // , Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.SetFlags(new string[] { "CarouselView_Experimental", "MediaElement_Experimental", "SwipeView_Experimental" });
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            CreateNotificationFromIntent(Intent);
            //CreateNotificationChannel("MainChannel", "Nejaka Sracka");
            //AddNotification("Test", "FUnhuje to?");
        }
        
        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        void CreateNotificationChannel(string channelName, string channelDescription, string channelId="TimeManagementApp")
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }
            
            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (Android.App.NotificationManager) GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void AddNotification(string title, string text, string channelId = "TimeManagementApp", int notificationId= 0)
        {
            // Set up an intent so that tapping the notifications returns to this app:
            //Intent intent = new Intent (this, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            // PendingIntent pendingIntent =
            //     PendingIntent.GetActivity (this, pendingIntentId, intent, PendingIntentFlags.OneShot);
            // Instantiate the builder and set notification elements:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, channelId)
                .SetContentTitle (title)
                .SetContentText (text)
                .SetDefaults ((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate))
                .SetSmallIcon (Resource.Drawable.splash_logo)
                //.SetContentIntent (pendingIntent)
                .SetSound (RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            Android.App.NotificationManager notificationManager =
                GetSystemService (Context.NotificationService) as Android.App.NotificationManager;

            // Publish the notification:
            notificationManager.Notify (notificationId, notification);
            builder.SetWhen (Java.Lang.JavaSystem.CurrentTimeMillis());
        }
    }
}