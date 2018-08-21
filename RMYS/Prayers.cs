﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static System.DateTime;
using Android.Media;

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

        public override void OnCreate()
        {
            base.OnCreate();
        }
        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
            Database DB = new Database("DB", 0);
            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 0);
            DB.DBCursor.MoveToFirst();
            string Prayers = DB.DBCursor.GetString(2);
            bool Continue = true;

            Intent intent2 = new Intent(this, typeof(MainActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 77, intent2, PendingIntentFlags.OneShot);

            Notification.Builder builder = new Notification.Builder(this)
                    .SetContentTitle("ذكرني لأبديتي")
                    .SetContentIntent(pendingIntent)
                    .SetAutoCancel(true)
                    .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                    .SetSmallIcon(Resource.Drawable.Icon)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
            Notification.BigTextStyle textStyle = new Notification.BigTextStyle();

            if (Prayers.Contains("باكر") & Now.Hour == 7)
            {
                builder.SetContentText("حان موعد صلاة باكر");
                textStyle.BigText("صلاة باكر من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if(Prayers.Contains("الساعة الثالثة") & Now.Hour == 9)
            {
                builder.SetContentText("حان موعد صلاة الساعة الثالثة");
                textStyle.BigText("تسبحة الساعة الثالثة من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الساعة السادسة") & Now.Hour == 12)
            {
                builder.SetContentText("حان موعد صلاة الساعة السادسة");
                textStyle.BigText("تسبحة الساعة السادسة من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الساعة التاسعة") & Now.Hour == 15)
            {
                builder.SetContentText("حان موعد صلاة الساعة التاسعة");
                textStyle.BigText("تسبحة الساعة التاسعة من النهار المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الغروب") & Now.Hour == 17)
            {
                builder.SetContentText("حان موعد صلاة الغروب");
                textStyle.BigText("تسبحة الغروب من اليوم المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("النوم") & Now.Hour == 18)
            {
                builder.SetContentText("حان موعد صلاة النوم");
                textStyle.BigText("تسبحة النوم من اليوم المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الخدمة الأولى") & Now.Hour == 0)
            {
                builder.SetContentText("حان موعد صلاة نصف الليل: الخدمة الأولى");
                textStyle.BigText("تسبحة نصف الليل من اليوم المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الخدمة الثانية") & Now.Hour == 1)
            {
                builder.SetContentText("حان موعد صلاة نصف الليل: الخدمة الثانية");
                textStyle.BigText("تسبحة نصف الليل المبارك أقدمها للمسيح ملكي وإلهي وأرجوه أن يغفر لي خاطاياي.");
                textStyle.SetSummaryText("عدد مزامير الصلاة: " + DB.DBCursor.GetString(6));
            }
            else if (Prayers.Contains("الخدمة الثالثة") & Now.Hour == 2)
            {
                builder.SetContentText("حان موعد صلاة نصف الليل: الخدمة الثالثة");
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