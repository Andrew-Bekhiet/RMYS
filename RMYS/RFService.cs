using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static System.DateTime;
using Java.Util;
using Android.Util;

namespace RMYS
{
    [Service(Exported = false, Icon = "@drawable/Icon", Label = "خدمة ذكرني لأبديتي")]
    public class RFService : IntentService
    {
        Database DB = new Database();
        Android.Database.ICursor C;
        public RFService() : base("RFService")
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
            try
            {
                DB.CreateOpenDatabase("DB", 0);
                C = DB.GetRecordCursor(0, "Name", "ASC");

                C.MoveToFirst();
                for (int i = 0; i < C.Count; i++, C.MoveToNext())
                {
                    //DaysIndex 4, TimingIndex 5
                    if (C.GetString(4).Contains("-"))
                    {
                        string[] Days = C.GetString(4).Split('-');
                        for (int i2 = 0; i2 < Days.Length; i2++)
                        {
                            if (Days[i2] == new DateTime(Now.Year, Now.Month, Now.Day).ToString("dddd", new System.Globalization.CultureInfo("ar-Eg")))
                            {
                                if (DateTime.Parse(C.GetString(5)).AddMinutes(-30).ToString("HH:mm") != Now.Hour + ":" + Now.Minute & DateTime.Parse(C.GetString(5)).AddMinutes(-5).ToString("HH:mm") != Now.Hour + ":" + Now.Minute)
                                {
                                    Intent wake = new Intent(Application.Context, typeof(RFService));
                                    PendingIntent pending = PendingIntent.GetService(Application.Context, 4, wake, PendingIntentFlags.UpdateCurrent);

                                    AlarmManager alarmMgr = (AlarmManager)GetSystemService(AlarmService);

                                    Calendar cal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                                    cal.Set(Now.Year, Now.Month - 1, Now.Day, int.Parse(DateTime.Parse(C.GetString(5)).AddMinutes(-30).ToString("HH:mm").Split(':')[0]), int.Parse(DateTime.Parse(C.GetString(5)).AddMinutes(-30).ToString("HH:mm").Split(':')[1]));

                                    alarmMgr.Set(AlarmType.RtcWakeup, cal.TimeInMillis, pending);
                                    //#if DEBUG
                                    //                                Toast.MakeText(this, "1st Time", ToastLength.Long).Show();
                                    //#endif
                                }
                                else if (DateTime.Parse(C.GetString(5)).AddMinutes(-30).ToString("HH:mm") == Now.Hour + ":" + Now.Minute)
                                {
                                    // Set up an intent so that tapping the notifications returns to this app:
                                    Intent intent2 = new Intent(this, typeof(MainActivity));
                                    PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intent2, PendingIntentFlags.OneShot);

                                    // Instantiate the builder and set notification elements, including pending intent:
                                    Notification.Builder builder = new Notification.Builder(this)
                                        .SetContentTitle("ذكرني لأبديتي")
                                        .SetContentIntent(pendingIntent)
                                        .SetContentText("هل أنت مستعد للذهاب إلى " + SIF("إجتماع", !C.GetString(2).Contains("إجتماع") & !C.GetString(2).Contains("اجتماع") & C.GetString(1) == "M") + SIF("قداس ", !C.GetString(2).Contains("قداس") & C.GetString(1) == "K") + C.GetString(2) + "؟")
                                        .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                                        .SetSmallIcon(Resource.Drawable.Icon)
                                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
                                    // Build the notification:
                                    Notification notification = builder.Build();

                                    // Get the notification manager:
                                    NotificationManager notificationManager =
                                        GetSystemService(Context.NotificationService) as NotificationManager;

                                    // Publish the notification:
                                    notificationManager.Notify(0, notification);

                                    Intent wake = new Intent(Application.Context, typeof(RFService));
                                    PendingIntent pending = PendingIntent.GetService(Application.Context, 4, wake, PendingIntentFlags.UpdateCurrent);

                                    AlarmManager alarmMgr = (AlarmManager)GetSystemService(AlarmService);

                                    Calendar cal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                                    cal.Set(Now.Year, Now.Month - 1, Now.Day, int.Parse(DateTime.Parse(C.GetString(5)).AddMinutes(-5).ToString("HH:mm").Split(':')[0]), int.Parse(DateTime.Parse(C.GetString(5)).AddMinutes(-5).ToString("HH:mm").Split(':')[1]));

                                    alarmMgr.Set(AlarmType.RtcWakeup, cal.TimeInMillis, pending);
                                    //#if DEBUG
                                    //                                Toast.MakeText(this, "2nd Time", ToastLength.Long).Show();
                                    //#endif
                                }
                                else if (DateTime.Parse(C.GetString(5)).AddMinutes(-5).ToString("HH:mm") == Now.Hour + ":" + Now.Minute)
                                {
                                    Intent intent2 = new Intent(this, typeof(MainActivity));
                                    PendingIntent pendingIntent = PendingIntent.GetActivity(this, 1, intent2, PendingIntentFlags.OneShot);

                                    // Instantiate the builder and set notification elements, including pending intent:
                                    Notification.Builder builder = new Notification.Builder(this)
                                        .SetContentTitle("ذكرني لأبديتي")
                                        .SetContentIntent(pendingIntent)
                                        .SetContentText("متبقي 5 دقائق للذهاب إلى " + SIF("إجتماع", !C.GetString(2).Contains("إجتماع") & !C.GetString(2).Contains("اجتماع") & C.GetString(1) == "M") + SIF("قداس ", !C.GetString(2).Contains("قداس") & C.GetString(1) == "K") + C.GetString(2))
                                        .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                                        .SetSmallIcon(Resource.Drawable.Icon)
                                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
                                    // Build the notification:
                                    Notification notification = builder.Build();

                                    // Get the notification manager:
                                    NotificationManager notificationManager =
                                        GetSystemService(Context.NotificationService) as NotificationManager;

                                    // Publish the notification:
                                    notificationManager.Notify(0, notification);
                                    //#if DEBUG
                                    //                                Toast.MakeText(this, "3rd Time", ToastLength.Long).Show();
                                    //#endif
                                }
                            }
                        }
                    }
                    else //HolyBible
                    {
                        if (C.GetString(5) == Now.Hour + ":" + Now.Minute)
                        {
                            Intent intent2 = new Intent(this, typeof(MainActivity));
                            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 1, intent2, PendingIntentFlags.OneShot);

                            // Instantiate the builder and set notification elements, including pending intent:
                            Notification.Builder builder = new Notification.Builder(this)
                                .SetContentTitle("ذكرني لأبديتي")
                                .SetContentIntent(pendingIntent)
                                .SetContentText("حان موعد قراءة الكتاب المقدس")
                                .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
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
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Long).Show();
#if DEBUG
                Log.Debug("Error ST", e.StackTrace);
                Log.Debug("Error Msg", e.Message);
#endif
            }
        }
    }
}