using System;
using System.Collections.Generic;
using System.Linq;
using WorkShop2.Tests;

namespace WorkShop22
{
    public class BudgetCalculate
    {
        private readonly IRepository<Budget> _budRepository;

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
            var budgets = _budRepository.GetBudgets();
            var total = 0m;
            if (startTime.Month == endTime.Month)
            {
                return CaluateBudget(startTime, endTime,  GetThisMonthBudget(startTime, budgets));
            }
            total += CaluateBudget(startTime, new DateTime(startTime.Year, startTime.Month, DateTime.DaysInMonth(startTime.Year, startTime.Month)),GetThisMonthBudget(startTime, budgets));

            DateTime startTime1 = startDayOfEndTimeMonth(endTime);
            total += CaluateBudget(startTime1, endTime,  GetThisMonthBudget(startTime1, budgets));

            DateTime Counter = startTime;
            if (IsOver2Months(startTime, endTime))
            {
                do
                {
                    DateTime startTime2 = Counter.AddMonths(1);
                    total += CaluateBudget(startTime2, Counter.AddMonths(2).AddDays(-1), GetThisMonthBudget(startTime2, budgets));
                    Counter = Counter.AddMonths(1);
                } while (Counter.Month != endTime.AddMonths(-1).Month);
            }
            return total;
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

        private static int GetThisMonthBudget(DateTime startTime, List<Budget> budgets)
        {
            return budgets.SingleOrDefault(x => x.YearMonth == startTime.ToString("yyyyMM"))?.Amount ?? 0;
        }
    }
}