using System;

namespace WorkShop2.Tests
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public DateTime Firstday
        {
            get { return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null); }
        }

        public int DaysInMonth()
        {
            return DateTime.DaysInMonth(Firstday.Year,Firstday.Month);
        }
    }
}
