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


            CreateNotificationChannel();
            AddNotification();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = "Resources.GetString(Resource.String.channel_name)";
            var channelDescription =" GetString(Resource.String.channel_description)";
            var channel = new NotificationChannel("CHANNEL_ID", channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager) GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void AddNotification()
        {
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent (this, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity (this, pendingIntentId, intent, PendingIntentFlags.OneShot);
            // Instantiate the builder and set notification elements:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, "CHANNEL_ID")
                .SetContentTitle ("Sample Notification")
                .SetContentText ("Hello World! This is my first notification!")
                .SetDefaults ((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate))
                .SetSmallIcon (Resource.Drawable.splash_logo)
                .SetContentIntent (pendingIntent)
                .SetSound (RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService (Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify (notificationId, notification);
            builder.SetWhen (Java.Lang.JavaSystem.CurrentTimeMillis());
        }
    }
}