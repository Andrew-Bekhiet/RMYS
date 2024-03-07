using System;
using Android.App;
using Android.Content;
using static RMYS.Resource;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using System.IO;
using Java.Util;
using static System.DateTime;
using Android.Media;
using Android.Animation;

namespace RMYS.Properties
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootR : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                if (intent.Action.Equals(Intent.ActionBootCompleted))
                {
                    Database DB = new Database("DB", 1);
                    DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                    context.StartService(new Intent(context, typeof(MKS)));
                    context.StartService(new Intent(context, typeof(Prayers)));
                    context.StartService(new Intent(context, typeof(HBS)));
                    context.StartService(new Intent(context, typeof(FastsS)));
                    if (DB.DBCursor.MoveToFirst())
                    {
                        Spinner JPSpin = new Spinner(context);
                        TextView JPNum = new TextView(context);
                        var adapter = ArrayAdapter.CreateFromResource(context, Resource.Array.JPray, Android.Resource.Layout.SimpleSpinnerItem);
                        adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                        JPSpin.Adapter = adapter;

                        JPSpin.SetSelection(int.Parse(DB.DBCursor.GetString(5).Split('-')[1]));
                        JPNum.Text = DB.DBCursor.GetString(5).Split('-')[0];
                        Intent JPIntent = new Intent(context, typeof(JesusPrayS));
                        PendingIntent JPpending = PendingIntent.GetService(context, 34, JPIntent, PendingIntentFlags.UpdateCurrent);
                        ContextWrapper d = new ContextWrapper(context);
                        AlarmManager JPalarmMgr = (AlarmManager)d.GetSystemService("alarm");

                        Calendar cal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                        cal.Set(CalendarField.Year, Now.Year);
                        cal.Set(CalendarField.Month, Now.Month - 1);
                        cal.Set(CalendarField.DayOfMonth, Now.Day);
                        cal.Set(CalendarField.HourOfDay, Now.Hour);
                        cal.Set(CalendarField.Minute, Now.Minute);
                        cal.Set(CalendarField.Second, Now.Second + 1);
                        cal.Set(CalendarField.Millisecond, 0);

                        JPalarmMgr.Cancel(JPpending);

                        if (JPSpin.SelectedItemPosition == 0)
                        {
                            JPalarmMgr.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, int.Parse(JPNum.Text) * 1000, JPpending);
                        }
                        else if (JPSpin.SelectedItemPosition == 1)
                        {
                            JPalarmMgr.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, int.Parse(JPNum.Text) * 60 * 1000, JPpending);
                        }
                        else
                        {
                            JPalarmMgr.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, int.Parse(JPNum.Text) * 60 * 60 * 1000, JPpending);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}