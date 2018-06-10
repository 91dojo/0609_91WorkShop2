using System;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class Period
    {
        public Period(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime)
            {
                throw new ArgumentException();
            };
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public bool IsSameMonth()
        {
            return StartTime.Month == EndTime.Month;
        }

        public int Days()
        {
            return (EndTime.Subtract(StartTime).Days + 1);
        }

        public int OverlapDay(Budget budget)
        {
            var overlapStart = StartTime.ToString("yyyyMM") == budget.YearMonth
                ? StartTime
                : budget.Firstday;
            var overlapEnd = EndTime.ToString("yyyyMM") == budget.YearMonth
                ? EndTime
                : budget.LastDay;


            var overlapDay = new Period(overlapStart, overlapEnd).Days();
            return overlapDay;
        }
    }
}