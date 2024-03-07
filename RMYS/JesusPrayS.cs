using System;
using Android.App;
using Android.Content;
using Android.Media;

namespace RMYS
{
    [Service(Exported = false, Icon = "@drawable/Icon", Label = "خدمة الصلاة السهمية")]
    public class JesusPrayS : IntentService
    {
        public JesusPrayS() : base("JesusPrayS") { }
        protected override void OnHandleIntent(Intent intent) { }

        public override void OnCreate() { base.OnCreate(); }
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable CS0618 // Type or member is obsolete
        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore CS0618 // Type or member is obsolete
            string[] JesusPrayArray = Resources.GetStringArray(Resource.Array.JesusPrayList);
            int RandomPray = new Random().Next(JesusPrayArray.Length);

            Intent main = new Intent(this, typeof(MainActivity));
            PendingIntent pendingMain = PendingIntent.GetActivity(this, 0, main, PendingIntentFlags.OneShot);

            // Instantiate the Big Text style:
            Notification.BigTextStyle textStyle = new Notification.BigTextStyle();
            textStyle.BigText(JesusPrayArray[RandomPray]);

            // Instantiate the builder and set notification elements, including pending intent:
            Notification.Builder NBuilder = new Notification.Builder(this)
                .SetContentTitle("صلاة يسوع")
                .SetContentIntent(pendingMain)
                .SetAutoCancel(true)
                .SetContentText(JesusPrayArray[RandomPray])
                .SetDefaults(NotificationDefaults.Sound)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
            // Build the notification:
            NBuilder.SetStyle(textStyle);
            Notification notification = NBuilder.Build();

            // Get the notification manager:
            NotificationManager NPublisher =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            NPublisher.Notify(2, notification);
            
        }
    }
}