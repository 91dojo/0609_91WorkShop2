using System;
using WorkShop22;

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
        public DateTime LastDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + DaysInMonth().ToString(), "yyyyMMdd", null);
            }

        }

        public int DaysInMonth()
        {
            return DateTime.DaysInMonth(Firstday.Year,Firstday.Month);
        }

        public int DailyAmount()
        {
            var dailyAmount = Amount / DaysInMonth();
            return dailyAmount;
        }

        public int EffectAmount(Period period)
        {
            return period.OverlapDay(new Period(Firstday, LastDay)) * DailyAmount();
        }
    }
}
