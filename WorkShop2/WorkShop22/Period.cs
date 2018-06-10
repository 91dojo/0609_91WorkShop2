using System;

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

        public int OverlapDay(Period period1)
        {
            var overlapStart = StartTime > period1.StartTime
                ? StartTime
                : period1.StartTime;

            var overlapEnd = EndTime < period1.EndTime
                ? EndTime
                : period1.EndTime;


            return (overlapEnd.AddDays(1) - overlapStart).Days;
        }
    }
}