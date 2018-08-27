//For Jesus
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

namespace RMYS
{
    [Activity(Label = "كونوا مستعدين", Theme = "@android:style/Theme.Holo.Light.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Icon = "@drawable/Icon")]
    [System.Runtime.InteropServices.Guid("30F89535-5D12-4C3E-9197-FBC0ACFD2B74")]
    public class MainActivity : Activity
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Database DB = new Database();
        string[] Ms = { };
        string[] AP = { "Main" };
        string[] Data = { "", "", "", "", "" };
        string[] HData = { "", "", "", "", "" };
        string[] MOrderHelp = { "Name", "Name", "Place", "Place", "Days", "Days", "Timing", "Timing" };
        string[] SMOrder = { "Name", "ASC" };
        string type = "K";
        int selectin = 0;
        bool FM = true;
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                RequestWindowFeature(WindowFeatures.NoTitle);
                base.OnCreate(bundle);
                SetContentView(Layout.Main);
                if (!File.Exists(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DB.db")))
                {
                    Intent RFIntent = new Intent(this, typeof(MKS));
                    PendingIntent RFpending = PendingIntent.GetService(this, 3, RFIntent, PendingIntentFlags.UpdateCurrent);

                    AlarmManager RFalarmMgr = (AlarmManager)GetSystemService(AlarmService);

                    Calendar cal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                    cal.Set(CalendarField.Year, Now.Year);
                    cal.Set(CalendarField.Month, Now.Month - 1);
                    cal.Set(CalendarField.DayOfMonth, Now.Day);
                    cal.Set(CalendarField.HourOfDay, 0);
                    cal.Set(CalendarField.Minute, 0);
                    cal.Set(CalendarField.Second, 0);
                    cal.Set(CalendarField.Millisecond, 0);

                    RFalarmMgr.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, 24 * 60 * 60 * 1000, RFpending); //24 * 60 * 60 * 1000
                }
                DB.CreateOpenDatabase("DB", 1);
#if DEBUG
                File.Copy(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DB.db"), Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "RMYSDB.db"), true);
#endif
                
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ccess") & ex.Message.Contains("enied"))
                {
                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    TextView view = new TextView(this)
                    {
                        Text = "برجاء إعطاء البرنامج إذن الدخول لوحدة التخزين."
                    };
                    view.SetTextAppearance(this, Android.Resource.Style.TextAppearanceLarge);
                    AlertDialog.Builder alertbuilder = new AlertDialog.Builder(this);
                    alertbuilder.SetView(view);
                    alertbuilder.SetCancelable(false);
                    AlertDialog dialog = alertbuilder.Create();
                    dialog.Show();
                }
                else
                {
                    Msgbox(ex.Message, ToastLength.Long); 
                }
            }
            //Msgbox(Parse(C.GetString(5).Split(':')[1]).AddMinutes(30), ToastLength.Long);
        }
        public string Tosql(string date)
        {
            char[] b = { '-' };
            if (date.Split(b)[0].Length == 1)
            {
                date = "0" + date.Split(b)[0] + "-" + date.Split(b)[1] + "-" + date.Split(b)[2];
            }
            if (date.Split(b)[1].Length == 1)
            {
                date = date.Split(b)[0] + "-0" + date.Split(b)[1] + "-" + date.Split(b)[2];
            }
            if (date.Split(b)[0].Length != 4)
            {
                string ndate = date;
                ndate = date.Split(b)[2] + "-" + date.Split(b)[1] + "-" + date.Split(b)[0];
                return ndate;
            }
            else
            {
                return date;
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
        public void SetContentView(int ResId, string ContentName)
        {
            System.Array.Resize(ref AP, AP.Length + 1);
            AP[AP.Length - 1] = ContentName;
            SetContentView(ResId);
        }

        private void M_Click(object sender, EventArgs e)
        {
            DB.CreateOpenDatabase("DB", 1);
            DB.DBCursor = DB.GetRecordCursor("ID", ((TextView)sender).Tag.ToString(), 1);
            DB.DBCursor.MoveToFirst();
            Data[0] = DB.DBCursor.GetString(2);
            Data[1] = DB.DBCursor.GetString(3);
            Data[2] = DB.DBCursor.GetString(4);
            Data[3] = DB.DBCursor.GetString(5);
            Data[4] = DB.DBCursor.GetString(0);
            SetContentView(Layout.AddM, "AddM");
        }

        public override void OnBackPressed()
        {
            System.Array.Resize(ref AP, AP.Length - 1);
            if (AP.Length != 0)
            {
                string AP2 = AP[AP.Length - 1];
                if (AP2 == "Main")
                {
                    SetContentView(Layout.Main);
                }
                else if (AP2 == "AddM")
                {
                    SetContentView(Layout.AddM);
                }
                else if (AP2 == "Meetings")
                {
                    SetContentView(Layout.Meetings);
                }
                else if (AP2 == "HolyBible")
                {
                    SetContentView(Layout.HolyBible);
                }
                else if (AP2 == "HBHistory")
                {
                    SetContentView(Layout.HBHistory);
                }
                else if (AP2 == "HBHistoryV")
                {
                    SetContentView(Layout.HBHistoryV);
                }
                else if (AP2 == "SLaw")
                {
                    SetContentView(Layout.SLaw);
                }
            }
            else
            {
                base.OnBackPressed();
            }
        }
        bool Contain(string[] L, string e)
        {
            if (L.Length == 0)
            {
                return false;
            }
            else if (L[L.Length - 1] == e)
            {
                return true;
            }
            else
            {
                System.Array.Resize(ref L, L.Length - 1);
                return Contain(L, e);
            }
        }
        public void Msgbox(string msg, ToastLength dur)
        {
            Toast.MakeText(this, msg, dur).Show();
            Log.Debug("Toast", msg);
        }

        void MOrder(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spin = (Spinner)sender;
            if (FM)
            {
                SMOrder[0] = MOrderHelp[e.Position];
                if ((e.Position) % 2 == 0)
                {
                    SMOrder[1] = "DESC";
                }
                else
                {
                    SMOrder[1] = "ASC";
                }
                selectin = e.Position;
                FM = false;
                SetContentView(Layout.Meetings);
            }
            else
            {
                //spin.SetSelection(e.Position);
                FM = true;
            }
        }
        public void Msearch_search(string text)
        {

            try
            {
                Android.Database.ICursor C = DB.GetRecordCursor(1, "ID", "ASC");
                C.MoveToFirst();
                for (int i = 0; i < Ms.Length; i += 5)
                {
                    TextView MName = FindViewById<TextView>(int.Parse(Ms[i]));
                    TextView MPlace = FindViewById<TextView>(int.Parse(Ms[i + 1]));
                    TextView MDays = FindViewById<TextView>(int.Parse(Ms[i + 2]));
                    TextView MTiming = FindViewById<TextView>(int.Parse(Ms[i + 3]));
                    Space S = FindViewById<Space>(int.Parse(Ms[i + 4]));
                    C.MoveToPosition(int.Parse(MName.Tag.ToString()) - 1);
                    if ((MName.Text.Contains(text) | MPlace.Text.Contains(text) | MDays.Text.Contains(text) | MTiming.Text.Contains(text)) & C.GetString(1) == type)
                    {
                        MName.Visibility = ViewStates.Visible;
                        MPlace.Visibility = ViewStates.Visible;
                        MDays.Visibility = ViewStates.Visible;
                        MTiming.Visibility = ViewStates.Visible;
                        S.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        MName.Visibility = ViewStates.Gone;
                        MPlace.Visibility = ViewStates.Gone;
                        MDays.Visibility = ViewStates.Gone;
                        MTiming.Visibility = ViewStates.Gone;
                        S.Visibility = ViewStates.Gone;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Debug("Error", e.Message);
                Log.Debug("Error", e.StackTrace);
            }
        }
        public override void OnContentChanged()
        {
            base.OnContentChanged();
            try
            {
                string AP2 = AP[AP.Length - 1];
                if (AP2 == "Main")
                {
                    DB = new Database("DB", 2);
                    DB = new Database("DB", 1);
                    Button Meet = FindViewById<Button>(Id.Meet);
                    Button Kodas = FindViewById<Button>(Id.Kodas);
                    Button HolyB = FindViewById<Button>(Id.HolyB);
                    Button SLaw = FindViewById<Button>(Id.SLaw);
                    Button About = FindViewById<Button>(Id.About);
                    Meet.Click += delegate { type = "M"; SetContentView(Layout.Meetings, "Meetings"); };
                    Kodas.Click += delegate { type = "K"; SetContentView(Layout.Meetings, "Meetings"); };
                    HolyB.Click += delegate { SetContentView(Layout.HolyBible, "HolyBible"); };
                    SLaw.Click += delegate { SetContentView(Layout.SLaw, "SLaw"); };
                    About.Click += delegate { SetContentView(Layout.About, "About"); };
                }
                else if (AP2 == "Meetings")
                {
                    FM = false;
                    System.Array.Resize(ref Ms, 0);
                    EditText SearchBox = FindViewById<EditText>(Id.MSearch);
                    LinearLayout MLL = FindViewById<LinearLayout>(Id.MLL);
                    Button AddM = FindViewById<Button>(Id.AddM);
                    ImageView MSearchB = FindViewById<ImageView>(Id.MSearchB);
                    Spinner spinner = FindViewById<Spinner>(Id.MOrder);
                    SearchBox.AfterTextChanged += delegate { Msearch_search(SearchBox.Text); };
                    MSearchB.Click += delegate { Msearch_search(SearchBox.Text); };

                    var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.Order, Android.Resource.Layout.SimpleSpinnerItem);
                    spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(MOrder);
                    adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    spinner.Adapter = adapter;
                    spinner.SetSelection(selectin);
                    AddM.Click += delegate
                    {
                        Data[0] = "";
                        Data[1] = "";
                        Data[2] = "";
                        Data[3] = "";
                        Data[4] = "";
                        SetContentView(Layout.AddM, "AddM");
                    };
                    Android.Database.ICursor C;
                    DB.CreateOpenDatabase("DB", 1);
                    C = DB.GetRecordCursor(1, SMOrder[0], SMOrder[1]);
                    bool b = true;
                    using (TextView MName = FindViewById<TextView>(Id.MName))
                    {
                        MName.Visibility = ViewStates.Gone;
                    }
                    using (TextView MPlace = FindViewById<TextView>(Id.MPlace))
                    {
                        MPlace.Visibility = ViewStates.Gone;
                    }
                    using (TextView MDays = FindViewById<TextView>(Id.MDays))
                    {
                        MDays.Visibility = ViewStates.Gone;
                    }
                    using (TextView MTiming = FindViewById<TextView>(Id.MTiming))
                    {
                        MTiming.Visibility = ViewStates.Gone;
                    }
                    if (C != null & C.Count > 0)
                    {
                        C.MoveToFirst();
                        for (int i = 0; i < C.Count; i++)
                        {
                            TextView MName;
                            TextView MPlace;
                            TextView MDays;
                            TextView MTiming;
                            try
                            {
                                C.MoveToPosition(i);
                                if (b & C.GetString(1) == type)
                                {
                                    b = !b;
                                    MName = FindViewById<TextView>(Id.MName);
                                    MPlace = FindViewById<TextView>(Id.MPlace);
                                    MDays = FindViewById<TextView>(Id.MDays);
                                    MTiming = FindViewById<TextView>(Id.MTiming);
                                    Space S2 = FindViewById<Space>(Id.Space);

                                    MName.Visibility = ViewStates.Visible;
                                    MPlace.Visibility = ViewStates.Visible;
                                    MDays.Visibility = ViewStates.Visible;
                                    MTiming.Visibility = ViewStates.Visible;

                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MName.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MPlace.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MDays.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MTiming.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = S2.Id.ToString();

                                    MName.SetTextAppearance(this, Android.Resource.Style.TextAppearanceLarge);
                                    MPlace.SetTextAppearance(this, Android.Resource.Style.TextAppearanceMedium);
                                    MDays.SetTextAppearance(this, Android.Resource.Style.TextAppearanceSmall);
                                    MTiming.SetTextAppearance(this, Android.Resource.Style.TextAppearanceSmall);

                                    C.MoveToPosition(i);
                                    string Tag = C.GetString(0);

                                    MName.Tag = Tag;
                                    MPlace.Tag = Tag;
                                    MDays.Tag = Tag;
                                    MTiming.Tag = Tag;

                                    MName.Click += M_Click;
                                    MPlace.Click += M_Click;
                                    MDays.Click += M_Click;
                                    MTiming.Click += M_Click;
                                } //Duplicate
                                else if (C.GetString(1) == type)
                                {
                                    Space S = new Space(this)
                                    {
                                        LayoutParameters = new ViewGroup.LayoutParams(22, 8)
                                    };
                                    MName = new TextView(this);
                                    MPlace = new TextView(this);
                                    MDays = new TextView(this);
                                    MTiming = new TextView(this);

                                    MName.Clickable = true;
                                    MDays.Clickable = true;
                                    MTiming.Clickable = true;
                                    MPlace.Clickable = true;

                                    MName.Id = View.GenerateViewId();
                                    MPlace.Id = View.GenerateViewId();
                                    MDays.Id = View.GenerateViewId();
                                    MTiming.Id = View.GenerateViewId();
                                    S.Id = View.GenerateViewId();

                                    MName.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EBEBEB"));
                                    MPlace.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EBEBEB"));
                                    MDays.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EBEBEB"));
                                    MTiming.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EBEBEB"));

                                    MName.SetTextAppearance(this, Android.Resource.Style.TextAppearanceLarge);
                                    MPlace.SetTextAppearance(this, Android.Resource.Style.TextAppearanceMedium);
                                    MDays.SetTextAppearance(this, Android.Resource.Style.TextAppearanceSmall);
                                    MTiming.SetTextAppearance(this, Android.Resource.Style.TextAppearanceSmall);

                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MName.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MPlace.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MDays.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = MTiming.Id.ToString();
                                    System.Array.Resize(ref Ms, Ms.Length + 1);
                                    Ms[Ms.Length - 1] = S.Id.ToString();
                                    C.MoveToPosition(i);
                                    string Tag = C.GetString(0);

                                    MName.Tag = Tag;
                                    MPlace.Tag = Tag;
                                    MDays.Tag = Tag;
                                    MTiming.Tag = Tag;

                                    MLL.AddView(MName);
                                    MLL.AddView(MPlace);
                                    MLL.AddView(MDays);
                                    MLL.AddView(MTiming);

                                    MLL.AddView(S, new LinearLayout.LayoutParams(22, 8));

                                    MName.Click += M_Click;
                                    MPlace.Click += M_Click;
                                    MDays.Click += M_Click;
                                    MTiming.Click += M_Click;
                                }
                                else
                                {
                                    MName = new TextView(this);
                                    MPlace = new TextView(this);
                                    MDays = new TextView(this);
                                    MTiming = new TextView(this);
                                }
                                //              0   1       2             3                   4                                        5                          6
                                //DB.AddRecord(id, type, NMName.Text, NMPlace.Text, Days.Remove(Days.Length - 1), NMTime.CurrentHour + ":" + NMTime.CurrentMinute, "")
                                if (C.Count != 0 & C.GetString(1) == type)
                                {
                                    //GetData
                                    C.MoveToPosition(i);
                                    MName.Text = C.GetString(2);
                                    MPlace.Text = C.GetString(3);
                                    MDays.Text = C.GetString(4);
                                    if (int.Parse(C.GetString(5).Split(':')[0]) > 12)
                                    {
                                        MTiming.Text = (int.Parse(C.GetString(5).Split(':')[0]) - 12) + ":" + C.GetString(5).Split(':')[1] + " مساءً";
                                    }
                                    else if (int.Parse(C.GetString(5).Split(':')[0]) == 12)
                                    {
                                        MTiming.Text = C.GetString(5) + " مساءً";
                                    }
                                    else if (int.Parse(C.GetString(5).Split(':')[0]) == 0)
                                    {
                                        MTiming.Text = "12:" + C.GetString(5).Split(':')[1] + " صباحًا";
                                    }
                                    else
                                    {
                                        MTiming.Text = C.GetString(5) + " صباحًا";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Toast.MakeText(this, ex.StackTrace, ToastLength.Long).Show();
                            }
                        }
                    }
                    else
                    {
                        using (TextView MName = FindViewById<TextView>(Id.MName))
                        {
                            MName.Visibility = ViewStates.Gone;
                        }
                        using (TextView MPlace = FindViewById<TextView>(Id.MPlace))
                        {
                            MPlace.Visibility = ViewStates.Gone;
                        }
                        using (TextView MDays = FindViewById<TextView>(Id.MDays))
                        {
                            MDays.Visibility = ViewStates.Gone;
                        }
                        using (TextView MTiming = FindViewById<TextView>(Id.MTiming))
                        {
                            MTiming.Visibility = ViewStates.Gone;
                        }
                    }
                }
                else if (AP2 == "AddM")
                {
                    LinearLayout NMLL = FindViewById<LinearLayout>(Id.NMLL);
                    TextView WDD = FindViewById<TextView>(Id.WDD);
                    Button NMSave = FindViewById<Button>(Id.NMSave);
                    Button DeleteM = FindViewById<Button>(Id.DeleteM);
                    Button Cancel = FindViewById<Button>(Id.NMCancel);
                    EditText NMName = FindViewById<EditText>(Id.NMName);
                    EditText NMPlace = FindViewById<EditText>(Id.NMPlace);
                    TimePicker NMTime = FindViewById<TimePicker>(Id.NMTime);
                    NMName.Text = Data[0];
                    NMPlace.Text = Data[1];
                    if (type == "M")
                    {
                        NMName.Hint = "اسم الإجتماع";
                    }
                    else
                    {
                        NMName.Hint = "اسم القداس";
                    }
                    if (Data[2] != "")
                    {
                        string[] Days = Data[2].Split('-');
                        for (int c = 0; c < Days.Length; c++)
                        {
                            for (int i = 0; i < NMLL.ChildCount; i++)
                            {
                                CheckBox D = (CheckBox)NMLL.GetChildAt(i);
                                if (Days[c] == D.Text)
                                {
                                    D.Checked = true;
                                }
                            }
                        }
                        NMTime.CurrentHour = (Java.Lang.Integer)int.Parse(Data[3].Split(':')[0]);
                        NMTime.CurrentMinute = (Java.Lang.Integer)int.Parse(Data[3].Split(':')[1]);
                    }
                    else
                    {
                        DeleteM.Visibility = ViewStates.Gone;
                    }
                    WDD.Click += delegate
                    {
                        if (NMLL.Visibility.Equals(ViewStates.Gone))
                        {
                            //set Visible
                            NMLL.Visibility = ViewStates.Visible;
                            int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            NMLL.Measure(widthSpec, heightSpec);

                            ValueAnimator mAnimator = SlideAnimator(0, NMLL.MeasuredHeight, NMLL);
                            mAnimator.Start();

                        }
                        else
                        {
                            //collapse();
                            int finalHeight = NMLL.Height;

                            ValueAnimator mAnimator = SlideAnimator(finalHeight, 0, NMLL);
                            mAnimator.Start();
                            mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) =>
                            {
                                NMLL.Visibility = ViewStates.Gone;
                            };

                        }
                    };
                    Cancel.Click += delegate { OnBackPressed(); };
                    int count = 1;
                    DeleteM.Click += delegate
                    {
                        if (count == 0)
                        {
                            DB.DeleteRecord(int.Parse(Data[4]), 1);
                            OnBackPressed();
                        }
                        else
                        {
                            Msgbox("إضغط مرة أخرى للحذف", ToastLength.Short);
                            count--;
                        }
                    };
                    NMSave.Click += delegate
                    {
                        string Days = "";
                        for (int i = 0; i < 7; i++)
                        {
                            CheckBox D = (CheckBox)NMLL.GetChildAt(i);
                            if (D.Checked)
                            {
                                Days += D.Text + "-";
                            }
                        }
                        Android.Database.ICursor C = DB.GetRecordCursor(1, "Name", "ASC");
                        C.MoveToFirst();
                        //AddRecord(int id, string Type, string Name, string Place, string Days, string Timing, string Customs)

                        if (Days == "")
                        {
                            Msgbox("يجب تحديد يوم واحد على الأقل!", ToastLength.Short);
                        }
                        else if (NMName.Text == "" | NMPlace.Text == "")
                        {
                            Msgbox("يجب كتابة اسم الإجتماع ومكانه!", ToastLength.Short);
                        }
                        else
                        {
                            bool OK = true;
                            for (int i = 0; i < C.Count; i++, C.MoveToNext())
                            {
                                for (int i2 = 0; i2 < Days.Remove(Days.Length - 1).Split('-').Length; i2++)
                                {
                                    string[] DaysComp = C.GetString(4).Split('-');
                                    if (Contain(DaysComp, Days.Remove(Days.Length - 1).Split('-')[i2]))
                                    {
                                        if (C.GetString(5) == NMTime.CurrentHour + ":" + NMTime.CurrentMinute & !(C.GetString(2) == NMName.Text) & (Data[2] == "") & C.GetString(1) == type)
                                        {
                                            Toast.MakeText(this, "لديك " + SIF("إجتماع", type == "M") + SIF("قداس", type == "K") + " في نفس اليوم ونفس التوقيت", ToastLength.Short).Show();
                                            OK = false;
                                        }
                                    }
                                }
                            }
                            if (OK)
                            {
                                if (Data[2] == "") { DB.AddRecord(type, NMName.Text, NMPlace.Text, Days.Remove(Days.Length - 1), NMTime.CurrentHour + ":" + NMTime.CurrentMinute, ""); }
                                else { DB.UpdateRecord(int.Parse(Data[4]), type, NMName.Text, NMPlace.Text, Days.Remove(Days.Length - 1), NMTime.CurrentHour + ":" + NMTime.CurrentMinute, ""); }
                                Toast.MakeText(this, DB.Message, ToastLength.Short).Show();
                                #region "Check"
                                string[] Days2 = Days.Remove(Days.Length - 1).Split('-');
                                StartService(new Intent(this, typeof(MKS)));
                                for (int i2 = 0; i2 < Days2.Length; i2++)
                                {
                                    if (Days2[i2] == new DateTime(Now.Year, Now.Month, Now.Day).ToString("dddd", new System.Globalization.CultureInfo("ar-Eg")))
                                    {
                                        if (Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-30).ToString("HH:mm") != Now.Hour + ":" + Now.Minute & Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-5).ToString("HH:mm") != Now.Hour + ":" + Now.Minute)
                                        {
                                            Intent wake = new Intent(Application.Context, typeof(MKS));
                                            PendingIntent pending = PendingIntent.GetService(Application.Context, 4, wake, PendingIntentFlags.UpdateCurrent);

                                            AlarmManager alarmMgr = (AlarmManager)GetSystemService(AlarmService);

                                            Calendar cal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                                            cal.Set(Now.Year, Now.Month - 1, Now.Day, int.Parse(Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-30).ToString("HH:mm").Split(':')[0]), int.Parse(Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-30).ToString("HH:mm").Split(':')[1]));

                                            alarmMgr.Set(AlarmType.RtcWakeup, cal.TimeInMillis, pending);
                                            //#if DEBUG
                                            //                                Toast.MakeText(this, "1st Time", ToastLength.Long).Show();
                                            //#endif
                                        }
                                        else if (Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-30).ToString("HH:mm") == Now.Hour + ":" + Now.Minute)
                                        {
                                            // Set up an intent so that tapping the notifications returns to this app:
                                            Intent intent2 = new Intent(this, typeof(MainActivity));
                                            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 0, intent2, PendingIntentFlags.OneShot);

                                            // Instantiate the builder and set notification elements, including pending intent:
                                            Notification.Builder builder = new Notification.Builder(this)
                                                    .SetContentTitle("كونوا مستعدين")
                                                    .SetContentIntent(pendingIntent)
                                                    .SetAutoCancel(true)
                                                    .SetContentText("هل أنت مستعد للذهاب إلى " + SIF("إجتماع", !C.GetString(2).Contains("إجتماع") & !C.GetString(2).Contains("اجتماع") & C.GetString(1) == "M") + SIF("قداس ", !C.GetString(2).Contains("قداس") & C.GetString(1) == "K") + " " + C.GetString(2) + "في " + C.GetString(3) + "؟")
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

                                            Intent wake = new Intent(Application.Context, typeof(MKS));
                                            PendingIntent pending = PendingIntent.GetService(Application.Context, 4, wake, PendingIntentFlags.UpdateCurrent);

                                            AlarmManager alarmMgr = (AlarmManager)GetSystemService(AlarmService);

                                            Calendar cal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                                            cal.Set(Now.Year, Now.Month - 1, Now.Day, int.Parse(Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-5).ToString("HH:mm").Split(':')[0]), int.Parse(Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-5).ToString("HH:mm").Split(':')[1]));

                                            alarmMgr.Set(AlarmType.RtcWakeup, cal.TimeInMillis, pending);
                                            //#if DEBUG
                                            //                                Toast.MakeText(this, "2nd Time", ToastLength.Long).Show();
                                            //#endif
                                        }
                                        else if (Parse(NMTime.CurrentHour + ":" + NMTime.CurrentMinute).AddMinutes(-5).ToString("HH:mm") == Now.Hour + ":" + Now.Minute)
                                        {
                                            Intent intent2 = new Intent(this, typeof(MainActivity));
                                            PendingIntent pendingIntent = PendingIntent.GetActivity(this, 1, intent2, PendingIntentFlags.OneShot);

                                            // Instantiate the builder and set notification elements, including pending intent:
                                            Notification.Builder builder = new Notification.Builder(this)
                                                    .SetContentTitle("كونوا مستعدين")
                                                    .SetContentIntent(pendingIntent)
                                                    .SetContentText("متبقي 5 دقائق للذهاب إلى " + SIF("إجتماع", !C.GetString(2).Contains("إجتماع") & !C.GetString(2).Contains("اجتماع") & C.GetString(1) == "M") + SIF("قداس ", !C.GetString(2).Contains("قداس") & C.GetString(1) == "K") + " " + C.GetString(2) + "في " + C.GetString(3))
                                                    .SetAutoCancel(true)
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
                                #endregion
                                OnBackPressed();
                            }
                        }
                    };
                }
                else if (AP2 == "HolyBible")
                {
                    Button ShowH = FindViewById<Button>(Id.ShowH);
                    Button AddH = FindViewById<Button>(Id.AddH);
                    Button HBSave = FindViewById<Button>(Id.HBSave);
                    Button HBCancel = FindViewById<Button>(Id.HBCancel);
                    LinearLayout HBLL = FindViewById<LinearLayout>(Id.HBLL);
                    TextView HWDD = FindViewById<TextView>(Id.HWDD);
                    Spinner HBSpin = FindViewById<Spinner>(Id.HBSpin);
                    TimePicker HBTime = FindViewById<TimePicker>(Id.HBT);
                    EditText HBNum = FindViewById<EditText>(Id.HBNum);

                    HBLL.Visibility = ViewStates.Gone;
                    var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.HB, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    HBSpin.Adapter = adapter;
                    HBSpin.ItemSelected += HBSpin_ItemSelected;
                    if (DB.GetRecordCursor("Type", "HB", 1).Count != 0)
                    {
                        DB.DBCursor = DB.GetRecordCursor("Type", "HB", 1);
                        DB.DBCursor.MoveToFirst();
                        HBTime.CurrentHour = (Java.Lang.Integer)int.Parse(DB.DBCursor.GetString(5).Split(':')[0]);
                        HBTime.CurrentMinute = (Java.Lang.Integer)int.Parse(DB.DBCursor.GetString(5).Split(':')[1]);
                        HBNum.Text = DB.DBCursor.GetString(3);
                        if (DB.DBCursor.GetString(4) == "EveryHour")
                        {
                            HBSpin.SetSelection(0);
                        }
                        else if (DB.DBCursor.GetString(4) == "EveryDay")
                        {
                            HBSpin.SetSelection(1);
                        }
                        else
                        {
                            HBSpin.SetSelection(2);
                        }
                    }

                    HWDD.Click += delegate
                    {
                        if (HBLL.Visibility.Equals(ViewStates.Gone))
                        {
                            //set Visible
                            HBLL.Visibility = ViewStates.Visible;
                            int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            HBLL.Measure(widthSpec, heightSpec);

                            ValueAnimator mAnimator = SlideAnimator(0, HBLL.MeasuredHeight, HBLL);
                            mAnimator.Start();

                        }
                        else
                        {
                            //collapse();
                            int finalHeight = HBLL.Height;

                            ValueAnimator mAnimator = SlideAnimator(finalHeight, 0, HBLL);
                            mAnimator.Start();
                            mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) =>
                            {
                                HBLL.Visibility = ViewStates.Gone;
                            };

                        }
                    };
                    ShowH.Click += delegate { SetContentView(Layout.HBHistory, "HBHistory"); };
                    AddH.Click += delegate
                    {
                        HData[0] = "";
                        HData[1] = "";
                        HData[2] = "";
                        HData[3] = "";
                        HData[4] = "";
                        SetContentView(Layout.HBHistoryV, "HBHistoryV");
                    };
                    HBCancel.Click += delegate { OnBackPressed(); };
                    HBSave.Click += delegate
                    {
                        DB.CreateOpenDatabase("DB", 1);
                        if (DB.GetRecordCursor("Type", "HB", 1).Count == 0)
                        {
                            PendingIntent pending = PendingIntent.GetService(Application.Context, 4, new Intent(Application.Context, typeof(HBS)), PendingIntentFlags.UpdateCurrent);

                            AlarmManager alarmMgr = (AlarmManager)GetSystemService(AlarmService);

                            Calendar cal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                            cal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);

                            if (HBSpin.SelectedItemPosition == 0)
                            {
                                alarmMgr.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, int.Parse(HBNum.Text.ToString()) * 1000 * 60 * 60, pending);
                            }
                            else if (HBSpin.SelectedItemPosition == 1)
                            {
                                alarmMgr.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, int.Parse(HBNum.Text.ToString()) * 1000 * 60 * 60 * 24, pending);
                            }
                            else
                            {
                                alarmMgr.SetRepeating(AlarmType.RtcWakeup, cal.TimeInMillis, int.Parse(HBNum.Text.ToString()) * 1000 * 60 * 60 * 24 * 7, pending);
                            }
                            DB.AddRecord("HB", "HolyBible", HBNum.Text, SIF("EveryWeek", HBSpin.SelectedItemPosition == 2) + SIF("EveryDay", HBSpin.SelectedItemPosition == 1) + SIF("EveryHour", HBSpin.SelectedItemPosition == 0), HBTime.CurrentHour + ":" + HBTime.CurrentMinute, "");
                        }
                        else
                        {
                            PendingIntent pending = PendingIntent.GetService(Application.Context, 4, new Intent(Application.Context, typeof(HBS)), PendingIntentFlags.UpdateCurrent);
                            AlarmManager alarmMgr = (AlarmManager)GetSystemService(AlarmService);

                            alarmMgr.Cancel(pending);
                            
                            Calendar HBcal = Calendar.GetInstance(Java.Util.TimeZone.Default);

                            if (HBSpin.SelectedItemPosition == 0)
                            {
                                HBcal.Set(Now.Year, Now.Month - 1, Now.Day, Now.Hour, (int)HBTime.CurrentMinute);
                                alarmMgr.SetRepeating(AlarmType.RtcWakeup, HBcal.TimeInMillis, int.Parse(HBNum.Text.ToString()) * 1000 * 60 * 60, pending);
                            }
                            else if (HBSpin.SelectedItemPosition == 1)
                            {
                                HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                alarmMgr.SetRepeating(AlarmType.RtcWakeup, HBcal.TimeInMillis, int.Parse(HBNum.Text.ToString()) * 1000 * 60 * 60 * 24, pending);
                            }
                            else
                            {
                                RadioButton Sun = FindViewById<RadioButton>(Id.HBSunday);
                                RadioButton Mon = FindViewById<RadioButton>(Id.HBMonday);
                                RadioButton Tue = FindViewById<RadioButton>(Id.HBTuesday);
                                RadioButton Wed = FindViewById<RadioButton>(Id.HBWednesday);
                                RadioButton Thu = FindViewById<RadioButton>(Id.HBThursday);
                                RadioButton Fri = FindViewById<RadioButton>(Id.HBFriday);
                                RadioButton Sat = FindViewById<RadioButton>(Id.HBSaturday);
                                DateTime DT = new DateTime(2018, Now.Month, Now.Day);
                                #region "Week"
                                if (Sun.Checked)
                                {
                                    if(DT.DayOfWeek == DayOfWeek.Sunday)
                                    {
                                        HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                    else
                                    {
                                        while(DT.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            DT = DT.AddDays(1);
                                        }
                                        HBcal.Set(Now.Year, Now.Month - 1, DT.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                }
                                else if (Mon.Checked)
                                {
                                    if (DT.DayOfWeek == DayOfWeek.Monday)
                                    {
                                        HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                    else
                                    {
                                        while (DT.DayOfWeek != DayOfWeek.Monday)
                                        {
                                            DT = DT.AddDays(1);
                                        }
                                        HBcal.Set(Now.Year, Now.Month - 1, DT.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                }
                                else if (Tue.Checked)
                                {
                                    if (DT.DayOfWeek == DayOfWeek.Tuesday)
                                    {
                                        HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                    else
                                    {
                                        while (DT.DayOfWeek != DayOfWeek.Tuesday)
                                        {
                                            DT = DT.AddDays(1);
                                        }
                                        HBcal.Set(Now.Year, Now.Month - 1, DT.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                }
                                else if (Wed.Checked)
                                {
                                    if (DT.DayOfWeek == DayOfWeek.Wednesday)
                                    {
                                        HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                    else
                                    {
                                        while (DT.DayOfWeek != DayOfWeek.Wednesday)
                                        {
                                            DT = DT.AddDays(1);
                                        }
                                        HBcal.Set(Now.Year, Now.Month - 1, DT.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                }
                                else if (Thu.Checked)
                                {
                                    if (DT.DayOfWeek == DayOfWeek.Thursday)
                                    {
                                        HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                    else
                                    {
                                        while (DT.DayOfWeek != DayOfWeek.Thursday)
                                        {
                                            DT = DT.AddDays(1);
                                        }
                                        HBcal.Set(Now.Year, Now.Month - 1, DT.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                }
                                else if (Fri.Checked)
                                {
                                    if (DT.DayOfWeek == DayOfWeek.Friday)
                                    {
                                        HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                    else
                                    {
                                        while (DT.DayOfWeek != DayOfWeek.Friday)
                                        {
                                            DT = DT.AddDays(1);
                                        }
                                        HBcal.Set(Now.Year, Now.Month - 1, DT.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                }
                                else if (Sat.Checked)
                                {
                                    if (DT.DayOfWeek == DayOfWeek.Saturday)
                                    {
                                        HBcal.Set(Now.Year, Now.Month - 1, Now.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                    else
                                    {
                                        while (DT.DayOfWeek != DayOfWeek.Sunday)
                                        {
                                            DT = DT.AddDays(1);
                                        }
                                        HBcal.Set(Now.Year, Now.Month - 1, DT.Day, (int)HBTime.CurrentHour, (int)HBTime.CurrentMinute);
                                    }
                                }
                            #endregion
                                alarmMgr.SetRepeating(AlarmType.RtcWakeup, HBcal.TimeInMillis, int.Parse(HBNum.Text.ToString()) * 1000 * 60 * 60 * 24 * 7, pending);
                            }
                            DB.DBCursor = DB.GetRecordCursor("Type", "HB", 1);
                            DB.DBCursor.MoveToFirst();
                            DB.UpdateRecord(DB.DBCursor.GetInt(0), "HB", "HolyBible", HBNum.Text, SIF("EveryWeek", HBSpin.SelectedItemPosition == 2) + SIF("EveryDay", HBSpin.SelectedItemPosition == 1) + SIF("EveryHour", HBSpin.SelectedItemPosition == 0), HBTime.CurrentHour + ":" + HBTime.CurrentMinute, "");
                        }
                        Msgbox(DB.Message, ToastLength.Short);
                    };
                }
                else if (AP2 == "HBHistory")
                {
                    LinearLayout HLL = FindViewById<LinearLayout>(Id.HBHLL);
                    Button HAdd = FindViewById<Button>(Id.HAdd);
                    HAdd.Click += delegate
                    {
                        HData[0] = "";
                        HData[1] = "";
                        HData[2] = "";
                        HData[3] = "";
                        HData[4] = "";
                        SetContentView(Layout.HBHistoryV, "HBHistoryV");
                    };
                    DB.CreateOpenDatabase("DB", 2);
                    if (DB.GetRecordCursor(2, "", "").Count != 0)
                    {
                        DB.DBCursor = DB.GetRecordCursor(2, "", "");
                        DB.DBCursor.MoveToFirst();
                        for (int i = 0; i < DB.DBCursor.Count; i++, DB.DBCursor.MoveToNext())
                        {
                            TextView Title = new TextView(this);
                            TextView Date = new TextView(this);
                            TextView Visor = new TextView(this);
                            Space S = new Space(this);

                            HLL.AddView(Title, new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent));
                            HLL.AddView(Date, new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent));
                            HLL.AddView(Visor, new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent));
                            HLL.AddView(S, new LinearLayout.LayoutParams(22, 8));

                            Title.SetTextAppearance(this, Android.Resource.Style.TextAppearanceLarge);
                            Date.SetTextAppearance(this, Android.Resource.Style.TextAppearanceMedium);
                            Visor.SetTextAppearance(this, Android.Resource.Style.TextAppearanceMedium);

                            Title.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EBEBEB"));
                            Date.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EBEBEB"));
                            Visor.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EBEBEB"));

                            Title.Text = DB.DBCursor.GetString(1);
                            Date.Text = DB.DBCursor.GetString(2);
                            Visor.Text = DB.DBCursor.GetString(4);

                            Title.Tag = DB.DBCursor.GetInt(0) - 1;
                            Date.Tag = DB.DBCursor.GetInt(0) - 1;
                            Visor.Tag = DB.DBCursor.GetInt(0) - 1;

                            Title.Click += delegate
                            {
                                DB.DBCursor = DB.GetRecordCursor(2, "", "");
                                DB.DBCursor.MoveToPosition(int.Parse(Visor.Tag.ToString()));
                                HData[0] = Date.Text;
                                HData[1] = DB.DBCursor.GetString(3);
                                HData[2] = Visor.Text;
                                HData[3] = Date.Tag.ToString();
                                HData[4] = Title.Text;
                                SetContentView(Layout.HBHistoryV, "HBHistoryV");
                            };
                            Date.Click += delegate
                            {
                                DB.DBCursor = DB.GetRecordCursor(2, "", "");
                                DB.DBCursor.MoveToPosition(int.Parse(Date.Tag.ToString()));
                                HData[0] = Date.Text;
                                HData[1] = DB.DBCursor.GetString(3);
                                HData[2] = Visor.Text;
                                HData[3] = Date.Tag.ToString();
                                HData[4] = Title.Text;
                                SetContentView(Layout.HBHistoryV, "HBHistoryV");
                            };
                            Visor.Click += delegate
                            {
                                DB.DBCursor = DB.GetRecordCursor(2, "", "");
                                DB.DBCursor.MoveToPosition(int.Parse(Visor.Tag.ToString()));
                                HData[0] = Date.Text;
                                HData[1] = DB.DBCursor.GetString(3);
                                HData[2] = Visor.Text;
                                HData[3] = Date.Tag.ToString();
                                HData[4] = Title.Text;
                                SetContentView(Layout.HBHistoryV, "HBHistoryV");
                            };
                        }
                    }
                }
                else if (AP2 == "HBHistoryV")
                {
                    Button HSave = FindViewById<Button>(Id.HSave);
                    Button HDelete = FindViewById<Button>(Id.HDelete);
                    Button HCancel = FindViewById<Button>(Id.HCancel);
                    DatePicker HDate = FindViewById<DatePicker>(Id.HDate);
                    EditText HTitle = FindViewById<EditText>(Id.HTitle);
                    EditText HVerse = FindViewById<EditText>(Id.HVerse);
                    EditText HVisor = FindViewById<EditText>(Id.Visor);

                    if (HData[0] != "")
                    {
                        //Fill Data
                        HDate.UpdateDate(int.Parse(HData[0].Split('-')[0]), int.Parse(HData[0].Split('-')[1]) - 1, int.Parse(HData[0].Split('-')[2]));
                        HVerse.Text = HData[1];
                        HVisor.Text = HData[2];
                        HTitle.Text = HData[4];
                    }
                    else
                    {
                        HDelete.Visibility = ViewStates.Gone;
                    }

                    HCancel.Click += delegate { OnBackPressed(); };
                    HSave.Click += delegate
                    {
                        DB.CreateOpenDatabase("DB", 2);
                        if (HData[0] != "")
                        {
                            DB.UpdateHBRecord(int.Parse(HData[3]), HTitle.Text, Tosql(HDate.Year + "-" + (HDate.Month + 1) + "-" + HDate.DayOfMonth), HVerse.Text, HVisor.Text);
                        }
                        else
                        {
                            DB.AddHBRecord(HTitle.Text, Tosql(HDate.Year + "-" + (HDate.Month + 1) + "-" + HDate.DayOfMonth), HVerse.Text, HVisor.Text);
                        }
                        Msgbox(DB.Message, ToastLength.Short);
                        OnBackPressed();
                    };
                    HDelete.Click += delegate
                    {
                        DB.DeleteRecord(int.Parse(HData[3]) + 1, 2);
                        OnBackPressed();
                    };
                }
                else if (AP2 == "SLaw")
                {
                    DB = new Database("DB", 1);
                    LinearLayout SLL = FindViewById<LinearLayout>(Id.SLLL);
                    LinearLayout SLL2 = FindViewById<LinearLayout>(Id.SLLL2);
                    LinearLayout FLL = FindViewById<LinearLayout>(Id.FaLL);
                    LinearLayout PLL = FindViewById<LinearLayout>(Id.PLL);
                    Button HButton = FindViewById<Button>(Id.HBButton);
                    Button SaveP = FindViewById<Button>(Id.SaveP);
                    Button SaveF = FindViewById<Button>(Id.SaveF);
                    Button SaveA = FindViewById<Button>(Id.SaveA);
                    Button SLCancel = FindViewById<Button>(Id.SLCancel);
                    Spinner JPSpin = FindViewById<Spinner>(Id.JPSpin);
                    TextView L1 = FindViewById<TextView>(Id.L1);
                    TextView L3 = FindViewById<TextView>(Id.L3);
                    TextView Fast = FindViewById<TextView>(Id.Fast);
                    TextView Pray = FindViewById<TextView>(Id.Pray);
                    EditText JPNum = FindViewById<EditText>(Id.JPLNum);
                    EditText PalNum = FindViewById<EditText>(Id.PalNum);
                    CheckBox Metaniat = FindViewById<CheckBox>(Id.Metaniat);
                    string Prayers = "";
                    string Fasts = "";
                    int id = -1;

                    FLL.Visibility = ViewStates.Gone;
                    PLL.Visibility = ViewStates.Gone;
                    SLL.Visibility = ViewStates.Gone;
                    SLL2.Visibility = ViewStates.Gone;
                    var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.JPray, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    JPSpin.Adapter = adapter;
                    JPSpin.SetSelection(1);

                    //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                    if (DB.GetRecordCursor("Type", "SL", 1).Count != 0)
                    {
                        DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                        DB.DBCursor.MoveToFirst();
                        id = DB.DBCursor.GetInt(0);
                        if (DB.DBCursor.GetString(2) != "")
                        {
                            for (int c = 0; c < DB.DBCursor.GetString(2).Split('-').Length; c++)
                            {
                                for (int i = 0; i < SLL.ChildCount; i++)
                                {
                                    CheckBox D = (CheckBox)SLL.GetChildAt(i);
                                    if (DB.DBCursor.GetString(2).Split('-')[c] == D.Text)
                                    {
                                        D.Checked = true;
                                    }
                                }
                            }
                            PalNum.Text = DB.DBCursor.GetString(6);
                            JPSpin.SetSelection(int.Parse(DB.DBCursor.GetString(5).Split('-')[1]));
                            JPNum.Text = DB.DBCursor.GetString(5).Split('-')[0];
                        }
                        if (DB.DBCursor.GetString(4) != "")
                        {
                            for (int c = 0; c < DB.DBCursor.GetString(4).Split('-').Length; c++)
                            {
                                for (int i = 0; i < SLL2.ChildCount; i++)
                                {
                                    CheckBox D = (CheckBox)SLL2.GetChildAt(i);
                                    if (DB.DBCursor.GetString(4).Split('-')[c] == D.Text)
                                    {
                                        D.Checked = true;
                                    }
                                }
                            }
                            Metaniat.Checked = bool.Parse(DB.DBCursor.GetString(3));
                        }
                    }

                    //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                    HButton.Click += delegate { SetContentView(Layout.HolyBible, "HolyBible"); };
                    L1.Click += delegate
                    {
                        if (SLL.Visibility.Equals(ViewStates.Gone))
                        {
                            //set Visible
                            SLL.Visibility = ViewStates.Visible;
                            int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            SLL.Measure(widthSpec, heightSpec);

                            ValueAnimator mAnimator = SlideAnimator(0, SLL.MeasuredHeight, SLL);
                            mAnimator.Start();

                        }
                        else
                        {
                            //collapse();
                            int finalHeight = SLL.Height;

                            ValueAnimator mAnimator = SlideAnimator(finalHeight, 0, SLL);
                            mAnimator.Start();
                            mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) =>
                            {
                                SLL.Visibility = ViewStates.Gone;
                            };

                        }
                    };
                    Fast.Click += delegate
                    {
                        if (FLL.Visibility.Equals(ViewStates.Gone))
                        {
                            //set Visible
                            FLL.Visibility = ViewStates.Visible;
                            int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            FLL.Measure(widthSpec, heightSpec);

                            ValueAnimator mAnimator = SlideAnimator(0, FLL.MeasuredHeight, FLL);
                            mAnimator.Start();

                        }
                        else
                        {
                            //collapse();
                            int finalHeight = FLL.Height;

                            ValueAnimator mAnimator = SlideAnimator(finalHeight, 0, FLL);
                            mAnimator.Start();
                            mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) =>
                            {
                                FLL.Visibility = ViewStates.Gone;
                            };

                        }
                    };
                    Pray.Click += delegate
                    {
                        if (PLL.Visibility.Equals(ViewStates.Gone))
                        {
                            //set Visible
                            PLL.Visibility = ViewStates.Visible;
                            int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            PLL.Measure(widthSpec, heightSpec);

                            ValueAnimator mAnimator = SlideAnimator(0, PLL.MeasuredHeight, PLL);
                            mAnimator.Start();

                        }
                        else
                        {
                            //collapse();
                            int finalHeight = PLL.Height;

                            ValueAnimator mAnimator = SlideAnimator(finalHeight, 0, PLL);
                            mAnimator.Start();
                            mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) =>
                            {
                                PLL.Visibility = ViewStates.Gone;
                            };

                        }
                    };
                    L3.Click += delegate
                    {
                        if (SLL2.Visibility.Equals(ViewStates.Gone))
                        {
                            //set Visible
                            SLL2.Visibility = ViewStates.Visible;
                            int widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            int heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                            SLL2.Measure(widthSpec, heightSpec);

                            ValueAnimator mAnimator = SlideAnimator(0, SLL2.MeasuredHeight, SLL2);
                            mAnimator.Start();

                        }
                        else
                        {
                            //collapse();
                            int finalHeight = SLL2.Height;

                            ValueAnimator mAnimator = SlideAnimator(finalHeight, 0, SLL2);
                            mAnimator.Start();
                            mAnimator.AnimationEnd += (object IntentSender, EventArgs arg) =>
                            {
                                SLL2.Visibility = ViewStates.Gone;
                            };

                        }
                    };

                    SLCancel.Click += delegate { OnBackPressed(); };
                    SaveP.Click += delegate
                    {
                        //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                        if (DB.GetRecordCursor("Type", "SL", 1).Count != 0)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            id = DB.DBCursor.GetInt(0);
                        }
                        //string Fasts = "";
                        for (int i = 0; i < SLL.ChildCount; i++)
                        {
                            CheckBox child = (CheckBox)SLL.GetChildAt(i);
                            if (child.Checked)
                            {
                                Prayers += child.Text + "-";
                            }
                        }
                        if (Prayers != "")
                        {
                            Prayers = Prayers.Remove(Prayers.Length - 1);

                            Intent PIntent = new Intent(this, typeof(Prayers));
                            PendingIntent Ppending = PendingIntent.GetService(this, 23, PIntent, PendingIntentFlags.UpdateCurrent);

                            AlarmManager PalarmMgr = (AlarmManager)GetSystemService(AlarmService);

                            Calendar Pcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                            Pcal.Set(CalendarField.Year, Now.Year);
                            Pcal.Set(CalendarField.Month, Now.Month - 1);
                            Pcal.Set(CalendarField.DayOfMonth, Now.Day);
                            Pcal.Set(CalendarField.HourOfDay, 0);
                            Pcal.Set(CalendarField.Minute, 0);
                            Pcal.Set(CalendarField.Second, 0);
                            Pcal.Set(CalendarField.Millisecond, 0);

                            PalarmMgr.Cancel(Ppending);
                            PalarmMgr.SetRepeating(AlarmType.RtcWakeup, Pcal.TimeInMillis, 60 * 60 * 1000, Ppending);
                            this.StartService(PIntent);
                        }

                        Intent JPIntent = new Intent(this, typeof(JesusPrayS));
                        PendingIntent JPpending = PendingIntent.GetService(this, 34, JPIntent, PendingIntentFlags.UpdateCurrent);

                        AlarmManager JPalarmMgr = (AlarmManager)GetSystemService(AlarmService);

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
                        //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                        if (id != -1)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            DB.UpdateRecord(id, "SL", Prayers, DB.DBCursor.GetString(3), DB.DBCursor.GetString(4), JPNum.Text + "-" + JPSpin.SelectedItemPosition, PalNum.Text);
                        }
                        else
                        {
                            DB.AddRecord("SL", Prayers, "", "", JPNum.Text + "-" + JPSpin.SelectedItemPosition, PalNum.Text);
                        }
                    };
                    SaveF.Click += delegate
                    {
                        if (DB.GetRecordCursor("Type", "SL", 1).Count != 0)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            id = DB.DBCursor.GetInt(0);
                        }

                        for (int i = 0; i < SLL2.ChildCount; i++)
                        {
                            CheckBox child = (CheckBox)SLL2.GetChildAt(i);
                            if (child.Checked)
                            {
                                Fasts += child.Text + "-";
                            }
                        }
                        //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                        if (id != -1)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            DB.UpdateRecord(id, "SL", DB.DBCursor.GetString(2), Metaniat.Checked.ToString(), Fasts, DB.DBCursor.GetString(5), DB.DBCursor.GetString(6));
                        }
                        else
                        {
                            DB.AddRecord("SL", "", Metaniat.Checked.ToString(), Fasts, "", "");
                        }
                        if (Fasts != "")
                        {
                            Fasts = Fasts.Remove(Fasts.Length - 1);

                            Intent FIntent = new Intent(this, typeof(FastsS));
                            PendingIntent Fpending = PendingIntent.GetService(this, 40, FIntent, PendingIntentFlags.UpdateCurrent);

                            AlarmManager FalarmMgr = (AlarmManager)GetSystemService(AlarmService);

                            Calendar Fcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                            Fcal.Set(CalendarField.Year, Now.Year);
                            Fcal.Set(CalendarField.Month, Now.Month - 1);
                            Fcal.Set(CalendarField.DayOfMonth, Now.Day);
                            Fcal.Set(CalendarField.HourOfDay, 0);
                            Fcal.Set(CalendarField.Minute, 0);
                            Fcal.Set(CalendarField.Second, 0);
                            Fcal.Set(CalendarField.Millisecond, 0);

                            FalarmMgr.Cancel(Fpending);
                            FalarmMgr.SetRepeating(AlarmType.RtcWakeup, Fcal.TimeInMillis, 24 * 60 * 60 * 1000, Fpending);
                            this.StartService(new Intent(this, typeof(FastsS)));
                        }
                    };
                    SaveA.Click += delegate 
                    {
                        //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                        if (DB.GetRecordCursor("Type", "SL", 1).Count != 0)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            id = DB.DBCursor.GetInt(0);
                        }
                        //string Fasts = "";
                        for (int i = 0; i < SLL.ChildCount; i++)
                        {
                            CheckBox child = (CheckBox)SLL.GetChildAt(i);
                            if (child.Checked)
                            {
                                Prayers += child.Text + "-";
                            }
                        }
                        if (Prayers != "")
                        {
                            Prayers = Prayers.Remove(Prayers.Length - 1);

                            Intent PIntent = new Intent(this, typeof(Prayers));
                            PendingIntent Ppending = PendingIntent.GetService(this, 23, PIntent, PendingIntentFlags.UpdateCurrent);

                            AlarmManager PalarmMgr = (AlarmManager)GetSystemService(AlarmService);

                            Calendar Pcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                            Pcal.Set(CalendarField.Year, Now.Year);
                            Pcal.Set(CalendarField.Month, Now.Month - 1);
                            Pcal.Set(CalendarField.DayOfMonth, Now.Day);
                            Pcal.Set(CalendarField.HourOfDay, 0);
                            Pcal.Set(CalendarField.Minute, 0);
                            Pcal.Set(CalendarField.Second, 0);
                            Pcal.Set(CalendarField.Millisecond, 0);

                            PalarmMgr.Cancel(Ppending);
                            PalarmMgr.SetRepeating(AlarmType.RtcWakeup, Pcal.TimeInMillis, 60 * 60 * 1000, Ppending);

                        }

                        Intent JPIntent = new Intent(this, typeof(JesusPrayS));
                        PendingIntent JPpending = PendingIntent.GetService(this, 34, JPIntent, PendingIntentFlags.UpdateCurrent);

                        AlarmManager JPalarmMgr = (AlarmManager)GetSystemService(AlarmService);

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
                        //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                        if (id != -1)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            DB.UpdateRecord(id, "SL", Prayers, DB.DBCursor.GetString(3), DB.DBCursor.GetString(4), JPNum.Text + "-" + JPSpin.SelectedItemPosition, PalNum.Text);
                        }
                        else
                        {
                            DB.AddRecord("SL", Prayers, "", "", JPNum.Text + "-" + JPSpin.SelectedItemPosition, PalNum.Text);
                        }
                        if (DB.GetRecordCursor("Type", "SL", 1).Count != 0)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            id = DB.DBCursor.GetInt(0);
                        }

                        for (int i = 0; i < SLL2.ChildCount; i++)
                        {
                            CheckBox child = (CheckBox)SLL2.GetChildAt(i);
                            if (child.Checked)
                            {
                                Fasts += child.Text + "-";
                            }
                        }
                        //DB.AddRecord("SL", "Baker-", "عدد مزامير كل ساعة", "صلاة يسوع", "الأصوام", "الميطانيات");
                        if (id != -1)
                        {
                            DB.DBCursor = DB.GetRecordCursor("Type", "SL", 1);
                            DB.DBCursor.MoveToFirst();
                            DB.UpdateRecord(id, "SL", DB.DBCursor.GetString(2), Metaniat.Checked.ToString(), Fasts, DB.DBCursor.GetString(5), DB.DBCursor.GetString(6));
                        }
                        else
                        {
                            DB.AddRecord("SL", "", Metaniat.Checked.ToString(), Fasts, "", "");
                        }
                        if (Fasts != "")
                        {
                            Fasts = Fasts.Remove(Fasts.Length - 1);

                            Intent FIntent = new Intent(this, typeof(FastsS));
                            PendingIntent Fpending = PendingIntent.GetService(this, 40, FIntent, PendingIntentFlags.UpdateCurrent);

                            AlarmManager FalarmMgr = (AlarmManager)GetSystemService(AlarmService);

                            Calendar Fcal = Calendar.GetInstance(Java.Util.TimeZone.Default);
                            Fcal.Set(CalendarField.Year, Now.Year);
                            Fcal.Set(CalendarField.Month, Now.Month - 1);
                            Fcal.Set(CalendarField.DayOfMonth, Now.Day);
                            Fcal.Set(CalendarField.HourOfDay, 0);
                            Fcal.Set(CalendarField.Minute, 0);
                            Fcal.Set(CalendarField.Second, 0);
                            Fcal.Set(CalendarField.Millisecond, 0);

                            FalarmMgr.Cancel(Fpending);
                            FalarmMgr.SetRepeating(AlarmType.RtcWakeup, Fcal.TimeInMillis, 24 * 60 * 60 * 1000, Fpending);
                            this.StartService(new Intent(this, typeof(FastsS)));
                        }
                    };
                }
            }
            catch (Exception e)
            {
                Msgbox(e.Message, ToastLength.Long);
                Msgbox(e.Message, ToastLength.Long);
                Msgbox(e.Message, ToastLength.Long);
                Msgbox(e.StackTrace, ToastLength.Long);
                Msgbox(e.StackTrace, ToastLength.Long);
                Msgbox(e.StackTrace, ToastLength.Long);
            }
        }

        private void HBSpin_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            LinearLayout HBLL = FindViewById<LinearLayout>(Id.HBLL);
            TextView HWDD = FindViewById<TextView>(Id.HWDD);
            Spinner HBSpin = (Spinner)sender;
            if(HBSpin.SelectedItemPosition == 2)
            {
                HWDD.Visibility = ViewStates.Visible;
            }
            else
            {
                HWDD.Visibility = ViewStates.Gone;
                HBLL.Visibility = ViewStates.Gone;
            }
        }

        private ValueAnimator SlideAnimator(int start, int end, LinearLayout SLL)
        {

            ValueAnimator animator = ValueAnimator.OfInt(start, end);
            //ValueAnimator animator2 = ValueAnimator.OfInt(start, end);
            //  animator.AddUpdateListener (new ValueAnimator.IAnimatorUpdateListener{
            animator.Update +=
                (object sender, ValueAnimator.AnimatorUpdateEventArgs e) => {
                        //  int newValue = (int)
                        //e.Animation.AnimatedValue; // Apply this new value to the object being animated.
                        //  myObj.SomeIntegerValue = newValue; 
                        var value = (int)animator.AnimatedValue;
                    ViewGroup.LayoutParams layoutParams = SLL.LayoutParameters;
                    layoutParams.Height = value;
                    SLL.LayoutParameters = layoutParams;

                };


            //      });
            return animator;
        }
    }
}


#pragma warning restore CS0618 // Type or member is obsolete