using System;
using System.Linq;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class BudgetCalculator
    {
        private static IRepository<Budget> _budgetRepository;
        private decimal _result = 0m;

        public BudgetCalculator(IRepository<Budget> budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        internal decimal TotalAmount(DateTime startTime, DateTime endTime)
        {
            var period = new Period(startTime, endTime);

            if (period.IsSameMonth())
            {
                return CalculateBudget(period.StartTime, period.EndTime);
            }

            _result += FirstMonthBudget(period.StartTime);
            _result += LastMonthBudget(period.EndTime);
            if (IsOver2Months(period.StartTime, period.EndTime))
            {
                var targetStartTime = new DateTime(period.StartTime.Year, period.StartTime.Month, 1);
                var targetEndTime = new DateTime(period.EndTime.Year, period.EndTime.Month, 1).AddMonths(-1);
                while (targetStartTime < targetEndTime)
                {
                    targetStartTime = targetStartTime.AddMonths(1);
                    var currentStartTime = new DateTime(targetStartTime.Year, targetStartTime.Month, 1);
                    var currentEndTime = new DateTime(targetStartTime.Year, targetStartTime.Month, DateTime.DaysInMonth(targetStartTime.Year, targetStartTime.Month));
                    _result += CalculateBudget(currentStartTime, currentEndTime);

                }
            }
            return _result;

            DateTime Counter = new DateTime(period.StartTime.Year, period.StartTime.Month, 1);
            if (IsOver2Months(period.StartTime, period.EndTime))
            {
                do
                {
                    DateTime startTime2 = Counter.AddMonths(1);
                    _result += CalculateBudget(startTime2, Counter.AddMonths(2).AddDays(-1));
                    Counter = Counter.AddMonths(1);
                } while (Counter.Month != period.EndTime.AddMonths(-1).Month);
            }
            return _result;
        }

     

        private static int LastMonthBudget(DateTime endTime)
        {
            return CalculateBudget(startDayOfEndTimeMonth(endTime), endTime);
        }

        private static int FirstMonthBudget(DateTime startTime)
        {
            return CalculateBudget(startTime, new DateTime(startTime.Year, startTime.Month, DateTime.DaysInMonth(startTime.Year, startTime.Month)));
        }

        private static DateTime startDayOfEndTimeMonth(DateTime endTime)
        {
            return new DateTime(endTime.Year, endTime.Month, 1);
        }

        private static bool IsOver2Months(DateTime startTime, DateTime endTime)
        {
            var startTime1 = new DateTime(startTime.Year, startTime.Month, 1);
            var endTime1 = new DateTime(endTime.Year, endTime.Month, 1);
            return startTime1.AddMonths(2) < endTime1;
        }

        private static int CalculateBudget(DateTime startTime, DateTime endTime)
        {
            var budgets = _budgetRepository.GetBudgets();
            var singleOrDefault = budgets.SingleOrDefault(x => x.YearMonth == startTime.ToString("yyyyMM"));
            var budget= singleOrDefault?.Amount ?? 0;
            return new Period(startTime, endTime).Days() * budget / DateTime.DaysInMonth(startTime.Year, startTime.Month);
        }

        private static int GetMonthlyTotalBudget(DateTime time)
        {
            var budgets = _budgetRepository.GetBudgets();
            return budgets.SingleOrDefault(x => x.YearMonth == time.ToString("yyyyMM"))?.Amount ?? 0;
        }
    }
}