using System;
using System.Linq;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class BudgetCalculate
    {
        private static IRepository<Budget> _budRepository;
        private decimal _result = 0m;

        public BudgetCalculate(IRepository<Budget> budRepository)
        {
            _budRepository = budRepository;
        }

        internal decimal Result(DateTime startTime, DateTime endTime)
        {
            if (IsInputError(startTime, endTime))
            {
                throw new ArgumentException();
            }

            if (startTime.Month == endTime.Month)
            {
                return CaluateBudget(startTime, endTime, GetMonthlyTotalBudget(startTime));
            }
            _result += FirstMonthBudget(startTime);
            _result += LastMonthBudget(endTime);
            if (IsOver2Months(startTime, endTime))
            {
                var targetStartTime = new DateTime(startTime.Year, startTime.Month, 1);
                var targetEndTime = new DateTime(endTime.Year, endTime.Month, 1).AddMonths(-1);
                while (targetStartTime < targetEndTime)
                {
                    targetStartTime = targetStartTime.AddMonths(1);
                    var currentStartTime = new DateTime(targetStartTime.Year, targetStartTime.Month, 1);
                    var currentEndTime = new DateTime(targetStartTime.Year, targetStartTime.Month, DateTime.DaysInMonth(targetStartTime.Year, targetStartTime.Month));
                    _result += CaluateBudget(currentStartTime, currentEndTime, GetMonthlyTotalBudget(currentStartTime));

                }
            }
            return _result;

            DateTime Counter = new DateTime(startTime.Year, startTime.Month, 1);
            if (IsOver2Months(startTime, endTime))
            {
                do
                {
                    DateTime startTime2 = Counter.AddMonths(1);
                    _result += CaluateBudget(startTime2, Counter.AddMonths(2).AddDays(-1), GetMonthlyTotalBudget(startTime2));
                    Counter = Counter.AddMonths(1);
                } while (Counter.Month != endTime.AddMonths(-1).Month);
            }
            return _result;
        }

        private static int LastMonthBudget(DateTime endTime)
        {
            return CaluateBudget(startDayOfEndTimeMonth(endTime), endTime, GetMonthlyTotalBudget(startDayOfEndTimeMonth(endTime)));
        }

        private static int FirstMonthBudget(DateTime startTime)
        {
            return CaluateBudget(startTime, new DateTime(startTime.Year, startTime.Month, DateTime.DaysInMonth(startTime.Year, startTime.Month)), GetMonthlyTotalBudget(startTime));
        }

        private static DateTime startDayOfEndTimeMonth(DateTime endTime)
        {
            return new DateTime(endTime.Year, endTime.Month, 1);
        }

        private static bool IsInputError(DateTime startTime, DateTime endTime)
        {
            return startTime > endTime;
        }

        private static bool IsOver2Months(DateTime startTime, DateTime endTime)
        {
            var startTime1 = new DateTime(startTime.Year, startTime.Month, 1);
            var endTime1 = new DateTime(endTime.Year, endTime.Month, 1);
            return startTime1.AddMonths(2) < endTime1;
        }

        private static int CaluateBudget(DateTime startTime, DateTime endTime, int budget)
        {
            return (endTime.Subtract(startTime).Days + 1) * budget / DateTime.DaysInMonth(startTime.Year, startTime.Month);
        }

        private static int GetMonthlyTotalBudget(DateTime time)
        {
            var budgets = _budRepository.GetBudgets();
            return budgets.SingleOrDefault(x => x.YearMonth == time.ToString("yyyyMM"))?.Amount ?? 0;
        }
    }
}