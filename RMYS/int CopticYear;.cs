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



private DateTime toCoptic(int year, int month, int day){
    int A = 0; //أ
    int B = 0; //ب
    int C = 0; //ج
    int D = 0; //د
    int F = 0; //و
    int G = 0; //ز
    int I = 0; //ط
    int J = 0; //ك
    if(month <= 2){
        A = month + 12;
        B = year - 1;
    }
    else{
        A = month;
        B = year;

    }
    C = B %% 100; //(C = (B - (B % 100) / 100)
    D = B %% 400; //(D = (B - (B % 400) / 400)
    F = (B + 4716) * 365.25; //without decimal points
    G = (A + 1) * 30.6001; //without decimal points
    I = (day + G + F + (2 - C + D) - 1826553) / 365.25;
    int CYear = int.parse(I.toString().Split(".")[0]) + 1;
    J = int.parse(I.toString().Split(".")[1]) * 12.175;
    int CMonth = int.parse(I.toString().Split(".")[0]) + 1;
    int CDays = int.parse(I.toString().Split(".")[1]) * 30; //to nearest unit
    return DateTime(CYear, CMonth, CDays);
}