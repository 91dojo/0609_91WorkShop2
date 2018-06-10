using System;

namespace WorkShop22
{
    public class Period
    {
        public Period(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public bool IsInvaildPeriod()
        {
            return StartTime > EndTime;
        }
    }
}