using static System.DateTime;
using Android.App;
using Android.Content;
using Android.Media;

namespace RMYS
{
    [Service(Exported = false, Icon = "@drawable/Icon", Label = "خدمة قراءة الكتاب المقدس")]
    public class HBS : IntentService
    {
        public HBS() : base("HBS")
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
        public string SIF(string s, bool b)
        {
            if (b)
            {
                return s;
            }
            else
            {
                return "";
            }
        }
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable CS0618 // Type or member is obsolete
        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore CS0618 // Type or member is obsolete
            Database DB = new Database("DB", 0);
            DB.DBCursor = DB.GetRecordCursor("Type", "HB", 1);
            if (DB.DBCursor.Count != 0)
            {
                DB.DBCursor.MoveToFirst();
                if (DB.DBCursor.GetString(5) == Now.Hour + ":" + Now.Minute)
                {
                    Intent intent2 = new Intent(this, typeof(MainActivity));
                    PendingIntent pendingIntent = PendingIntent.GetActivity(this, 1, intent2, PendingIntentFlags.OneShot);

                    // Instantiate the builder and set notification elements, including pending intent:
                    Notification.Builder builder = new Notification.Builder(this)
                        .SetContentTitle("كونوا مستعدين")
                        .SetContentIntent(pendingIntent)
                        .SetAutoCancel(true)
                        .SetContentText("حان موعد قراءة الكتاب المقدس")
                        .SetDefaults(NotificationDefaults.Sound)
                        .SetSmallIcon(Resource.Drawable.Icon)
                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
                    // Build the notification:
                    Notification notification = builder.Build();

                    // Get the notification manager:
                    NotificationManager notificationManager =
                        GetSystemService(Context.NotificationService) as NotificationManager;

                    // Publish the notification:
                    notificationManager.Notify(1, notification);
                } 
            }
        }
    }
}