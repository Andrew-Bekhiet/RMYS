using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.DateTime;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using Android.Util;

namespace RMYS
{
    [Service(Exported = false, Icon = "@drawable/Icon", Label = "خدمة الأصوام")]
    public class FastsS : IntentService
    {
        public FastsS() : base("FastsS")
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
        public void Msgbox(string msg, ToastLength dur)
        {
            Toast.MakeText(this, msg, dur).Show();
            Log.Debug("Toast", msg);
        }
#pragma warning disable CS0672 // Member overrides obsolete member
#pragma warning disable CS0618 // Type or member is obsolete
        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
#pragma warning restore CS0672 // Member overrides obsolete member
#pragma warning restore CS0618 // Type or member is obslolete
            Database DB = new Database("DB", 0);
            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 0);
            if (DB.DBCursor.Count != 0)
            {
                DB.DBCursor.MoveToFirst();
                if (DB.DBCursor.GetString(4) != "")
                {
                    #region "Find RiseDay"
                    int CopticYear;
                    int Sun;
                    int SunA;
                    int Moon;
                    int MoonA;
                    int SheepK;
                    int RiseDay;
                    string CMonth;
                    int Difference = 283;
                    int NowYear = Now.Year;
                    DateTime DT = new DateTime(2018, 8, 5);
                    DayOfWeek FDoCY = DayOfWeek.Sunday;
                    //Get Coptic Year
                    if (new DateTime(Now.Year, Now.Month, Now.Day).CompareTo(new DateTime(Now.Year, 1, 1)) >= 0 && new DateTime(Now.Year, Now.Month, Now.Day).CompareTo(new DateTime(Now.Year, 9, 10)) <= 0)
                    {
                        Difference++;
                        CopticYear = Now.Year - Difference;
                    }
                    else if (new DateTime(Now.Year, Now.Month, Now.Day).CompareTo(new DateTime(Now.Year, 9, 11)) == 0)
                    {
                        if (Now.Year % 4 == 3 & (Now.Year - 284) % 4 == 3)
                        {
                            Difference++;
                            CopticYear = Now.Year - Difference;
                        }
                        else
                        {
                            CopticYear = Now.Year - Difference;
                        }
                    }
                    else
                    {
                        CopticYear = Now.Year - Difference;
                    }
                    Sun = (CopticYear - 4) % 28;          //إيجاد دور الشمس
                    SunA = (((Sun - (Sun % 4)) / 4) + Sun) % 7; //إيجاد أبقطي الشمس
                    DT = new DateTime(2018, 8, 5);
                    switch (SunA)
                    {
                        case 0:
                            FDoCY = DayOfWeek.Tuesday;
                            break;
                        case 1:
                            FDoCY = DayOfWeek.Wednesday;
                            break;
                        case 2:
                            FDoCY = DayOfWeek.Thursday;
                            break;
                        case 3:
                            FDoCY = DayOfWeek.Friday;
                            break;
                        case 4:
                            FDoCY = DayOfWeek.Saturday;
                            break;
                        case 5:
                            FDoCY = DayOfWeek.Sunday;
                            break;
                        case 6:
                            FDoCY = DayOfWeek.Monday;
                            break;
                        case 7:
                            FDoCY = DayOfWeek.Tuesday;
                            break;
                    }

                    Moon = Now.Year % 19;      //دور القمر
                    if (Moon == 0)
                    {
                        Moon = 19;
                    }
                    MoonA = (Moon * 11) % 30;  //أبقطي القمر

                    SheepK = 40 - MoonA;
                    if (SheepK > 30)
                    {
                        SheepK -= 30;
                    }
                    if (24 > SheepK)
                    {
                        while (DT.DayOfWeek != FDoCY)
                        {
                            DT = DT.AddDays(1);
                        }
                        DT = DT.AddDays((SheepK % 7) - 1);
                        if (DT.DayOfWeek != DayOfWeek.Sunday)
                        {
                            while (DT.DayOfWeek != DayOfWeek.Sunday)
                            {
                                DT = DT.AddDays(1);
                                SheepK += 1;
                            }
                        }
                        else
                        {
                            SheepK += 7;
                        }
                        if (SheepK > 30)
                        {
                            RiseDay = SheepK % 30;
                            int MonthsToAdd = (SheepK - (SheepK % 30)) / 30;
                            if (MonthsToAdd == 1)
                            {
                                CMonth = "بشنس";
                            }
                            else if (MonthsToAdd == 2)
                            {
                                CMonth = "بؤونة";
                            }
                            else //if (MonthsToAdd == 3)
                            {
                                CMonth = "أبيب";
                            }
                        }
                        else
                        {
                            RiseDay = SheepK;
                            CMonth = "برمودة";
                        }
                    }
                    else
                    {
                        while (DT.DayOfWeek != FDoCY)
                        {
                            DT = DT.AddDays(1);
                        }
                        DT = DT.AddDays(-2);
                        DT = DT.AddDays((SheepK % 7) - 1);
                        if (DT.DayOfWeek != DayOfWeek.Sunday)
                        {
                            while (DT.DayOfWeek != DayOfWeek.Sunday)
                            {
                                DT = DT.AddDays(1);
                                SheepK += 1;
                            }
                        }
                        else
                        {
                            SheepK += 7;
                        }
                        if (SheepK > 30)
                        {
                            RiseDay = SheepK % 30;
                            int MonthsToAdd = (SheepK - (SheepK % 30)) / 30;
                            if (MonthsToAdd == 1)
                            {
                                CMonth = "برمودة";
                            }
                            else if (MonthsToAdd == 2)
                            {
                                CMonth = "بشنس";
                            }
                            else if (MonthsToAdd == 3)
                            {
                                CMonth = "بؤونة";
                            }
                            else //if (MonthsToAdd == 4)
                            {
                                CMonth = "أبيب";
                            }
                        }
                        else
                        {
                            RiseDay = SheepK;
                            CMonth = "برمهات";
                        }
                    }
                    int Imonth = 4;
                    if (CMonth == "برمهات")
                    {
                        RiseDay += 9;
                        if (RiseDay > 31)
                        {
                            RiseDay = RiseDay % 31;
                            CMonth = "أبريل";
                            Imonth = 4;
                        }
                        else
                        {
                            CMonth = "مارس";
                            Imonth = 3;
                        }
                    }
                    else if (CMonth == "برمودة")
                    {
                        RiseDay += 8;
                        if (RiseDay > 30)
                        {
                            RiseDay = RiseDay % 30;
                            CMonth = "مايو";
                            Imonth = 5;
                        }
                        else
                        {
                            CMonth = "أبريل";
                            Imonth = 4;
                        }
                    }
                    DateTime RiseT = new DateTime(CopticYear + Difference, Imonth, RiseDay);
                    #endregion
                    string Fasts = DB.DBCursor.GetString(4);
                    if (Fasts.Contains("الأربعاء") & Today.DayOfWeek == DayOfWeek.Wednesday)
                    {
                        Notification.Builder builder = new Notification.Builder(this)
                            .SetContentTitle("ذكرني لأبديتي")
                            .SetAutoCancel(true)
                            .SetContentText("اليوم صوم الأربعاء")
                            .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                            .SetSmallIcon(Resource.Drawable.Icon)
                            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
                        Notification notification = builder.Build();
                        NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
                        notificationManager.Notify(4, notification);
                    }
                    else if (Fasts.Contains("الجمعة") & Today.DayOfWeek == DayOfWeek.Friday)
                    {
                        Notification.Builder builder = new Notification.Builder(this)
                            .SetContentTitle("ذكرني لأبديتي")
                            .SetAutoCancel(true)
                            .SetContentText("اليوم صوم الجمعة")
                            .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                            .SetSmallIcon(Resource.Drawable.Icon)
                            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
                        Notification notification = builder.Build();
                        NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
                        notificationManager.Notify(4, notification);
                    }
                    Notification.Builder NBuilder = new Notification.Builder(this)
                            .SetContentTitle("ذكرني لأبديتي")
                            .SetAutoCancel(true)
                            .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                            .SetSmallIcon(Resource.Drawable.Icon)
                            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Ringtone));
                        Notification.BigTextStyle textStyle = new Notification.BigTextStyle();
                    
                    if (Today.CompareTo(RiseT) < 0)
                    {
                        if (Today.CompareTo(RiseT.AddDays(-7)) >= 0 & Today.CompareTo(RiseT) != 0 & Fasts.Contains("الآلام"))
                        {
                            textStyle.BigText("اليوم صوم أسبوع الآلام , صوم من الدرجة الأولى , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                            textStyle.SetSummaryText("اليوم صوم أسبوع الآلام");
                        }
                        else if (Today.CompareTo(RiseT.AddDays(-55)) >= 0 & Today.CompareTo(RiseT) != 0 & Fasts.Contains("الأربعين"))
                        {
                            textStyle.BigText("اليوم الصوم الكبير , صوم من الدرجة الأولى , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                            textStyle.SetSummaryText("اليوم الصوم الكبير");
                        }
                        else if (Today.CompareTo(RiseT.AddDays(-69)) >= 0 & Today.CompareTo(RiseT.AddDays(-66)) <= 0 & Fasts.Contains("يونان"))
                        {
                            textStyle.BigText("اليوم صوم يونان , صوم من الدرجة الأولى , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                            textStyle.SetSummaryText("اليوم صوم يونان");
                        }
                    }
                    if (Today.CompareTo(RiseT.AddDays(50)) >= 0 & Today.CompareTo(new DateTime(Now.Year, 7, 12)) <= 0 & Fasts.Contains("الرسل"))
                    {
                        textStyle.BigText("اليوم صوم الرسل , صوم من الدرجة الثانية , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                        textStyle.SetSummaryText("اليوم صوم الرسل");
                    }
                    else if (Today.CompareTo(new DateTime(Now.Year, 8, 7)) >= 0 & Today.CompareTo(new DateTime(Now.Year, 8, 21)) <= 0 & Fasts.Contains("العذراء مريم"))
                    {
                        textStyle.BigText("اليوم صوم السيدة العذراء مريم , صوم من الدرجة الثانية , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                        textStyle.SetSummaryText("اليوم صوم السيدة العذراء مريم");
                    }
                    else if (CopticYear % 4 == 0 & Today.CompareTo(new DateTime(Now.Year, 11, 11).AddDays(15)) >= 0 & Today.CompareTo(new DateTime(Now.Year, 1, 6)) <= 0 & Fasts.Contains("صوم الميلاد")) //16 هاتور
                    {
                        textStyle.BigText("اليوم صوم الميلاد , صوم من الدرجة الثانية , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                        textStyle.SetSummaryText("اليوم صوم الميلاد");
                    }
                    else if (CopticYear % 4 != 0 & Today.CompareTo(new DateTime(Now.Year, 11, 10).AddDays(15)) >= 0 & Today.CompareTo(new DateTime(Now.Year, 1, 6)) <= 0 & Fasts.Contains("صوم الميلاد"))  //16 هاتور
                    {
                        textStyle.BigText("اليوم صوم الميلاد , صوم من الدرجة الثانية , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                        textStyle.SetSummaryText("اليوم صوم الميلاد");
                    }
                    else if (Today.Equals(new DateTime(Now.Year, 1, 6)) & Fasts.Contains("برمون الميلاد"))
                    {
                        textStyle.BigText("اليوم صوم برمون الميلاد , صوم من الدرجة الأولى , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                        textStyle.SetSummaryText("اليوم صوم برمون الميلاد");
                    }
                    else if (CopticYear % 4 == 0 & Today.Equals(new DateTime(Now.Year, 1, 10).AddDays(10)) & Fasts.Contains("برمون الغطاس")) //11 طوبة
                    {
                        textStyle.BigText("اليوم صوم برمون الغطاس , صوم من الدرجة الأولى , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                        textStyle.SetSummaryText("اليوم صوم برمون الغطاس");
                    }
                    else if (CopticYear % 4 != 0 & Today.Equals(new DateTime(Now.Year, 11, 9).AddDays(10)) & Fasts.Contains("برمون الغطاس"))  //11 طوبة
                    {
                        textStyle.BigText("اليوم صوم برمون الغطاس , صوم من الدرجة الأولى , الميطانيات: " + SIF("نعم", DB.DBCursor.GetString(3) == bool.TrueString) + SIF("لا", DB.DBCursor.GetString(3) == bool.FalseString));
                        textStyle.SetSummaryText("اليوم صوم برمون الغطاس");
                    }
                    NBuilder.SetStyle(textStyle);
                    Notification N = NBuilder.Build();
                    NotificationManager NManager = GetSystemService(Context.NotificationService) as NotificationManager;
                    NManager.Notify(5, N);
                }
            }
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
    }
}