using Android.App;
using Android.Content;
using static System.DateTime;
using Android.Media;
using System;

namespace RMYS
{
    [Service(Exported = false, Icon = "@drawable/Icon", Label = "خدمة الصلاة")]
    public class Prayers : IntentService
    {
        public Prayers() : base("Prayers")
        {

        }
        protected override void OnHandleIntent(Intent intent)
        {

        }
        private bool IsAppInstalled(string packageName)
        {
            bool installed = false;
            try
            {
                PackageManager.GetPackageInfo(packageName, Android.Content.PM.PackageInfoFlags.Activities);
                installed = true;
            }
            catch
            {
                installed = false;
            }
            return installed;
        }
        public override void OnCreate()
        {
            base.OnCreate();
        }
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable CS0618 // Type or member is obsolete
        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore CS0618 // Type or member is obsolete
            Database DB = new Database("DB", 1);
            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
            DB.DBCursor.MoveToFirst();
            string Prayers = DB.DBCursor.GetString(2);
            bool Continue = true;
            Intent intent2;
            if (IsAppInstalled("com.app.orsozoxi"))
            {
                intent2 = PackageManager.GetLaunchIntentForPackage("com.app.orsozoxi");
            }
            else if (IsAppInstalled("appinventor.ai_andrewBekhiet.Agpeya"))
            {
                intent2 = PackageManager.GetLaunchIntentForPackage("appinventor.ai_andrewBekhiet.Agpeya");
            }
            else
            {
                intent2 = new Intent(this, typeof(MainActivity));
            }
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 77, intent2, PendingIntentFlags.OneShot);
            //PackageManager.ex
            Notification.Builder builder = new Notification.Builder(this)
                    .SetContentTitle("كونوا مستعدين")
                    .SetContentIntent(pendingIntent)
                    .SetAutoCancel(true)
                    .SetDefaults(NotificationDefaults.Sound)
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
            Notification.BigTextStyle textStyle = new Notification.BigTextStyle();

            if (Prayers.Contains("باكر") & Now.Hour == 7)
            {
                builder.SetContentText("الآن ... صلاة باكر");
                textStyle.BigText("صلاة باكر من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if(Prayers.Contains("الساعة الثالثة") & Now.Hour == 9)
            {
                builder.SetContentText("الآن ... صلاة الساعة الثالثة");
                textStyle.BigText("تسبحة الساعة الثالثة من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الساعة السادسة") & Now.Hour == 12)
            {
                builder.SetContentText("الآن ... صلاة الساعة السادسة");
                textStyle.BigText("تسبحة الساعة السادسة من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الساعة التاسعة") & Now.Hour == 15)
            {
                builder.SetContentText("الآن ... صلاة الساعة التاسعة");
                textStyle.BigText("تسبحة الساعة التاسعة من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الغروب") & Now.Hour == 17)
            {
                builder.SetContentText("الآن ... صلاة الغروب");
                textStyle.BigText("تسبحة الغروب من اليوم المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("النوم") & Now.Hour == 18)
            {
                builder.SetContentText("الآن ... صلاة النوم");
                textStyle.BigText("تسبحة النوم من اليوم المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الخدمة الأولى") & Now.Hour == 0)
            {
                builder.SetContentText("الآن ... صلاة نصف الليل: الخدمة الأولى");
                textStyle.BigText("تسبحة نصف الليل من اليوم المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الخدمة الثانية") & Now.Hour == 1)
            {
                builder.SetContentText("الآن ... صلاة نصف الليل: الخدمة الثانية");
                textStyle.BigText("تسبحة نصف الليل المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الخدمة الثالثة") & Now.Hour == 2)
            {
                builder.SetContentText("الآن ... صلاة نصف الليل: الخدمة الثالثة");
                textStyle.BigText("تسبحة نصف الليل المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else
            {
                intent2.Dispose();
                pendingIntent.Dispose();
                builder.Dispose();
                textStyle.Dispose();
                Continue = false;
            }
            if (Continue)
            {
                builder.SetStyle(textStyle);
                Notification notification = builder.Build();
                NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager.Notify(3, notification); 
            }
        }
    }
}