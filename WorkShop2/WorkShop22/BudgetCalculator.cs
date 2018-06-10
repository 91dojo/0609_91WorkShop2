using System;
using System.Collections.Generic;
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
            var budgets = _budgetRepository.GetBudgets();
            if (period.IsSameMonth())
            {
                var budget = budgets.SingleOrDefault(x => x.YearMonth == period.StartTime.ToString("yyyyMM"));
                if (budget == null)
                {
                    return 0;
                }
                return period.Days() * budget.DailyAmount();
            }

            var currentTime = period.StartTime;
            return TotalAmountWhenMultiMonth(currentTime, period, budgets);
        }

        private static decimal TotalAmountWhenMultiMonth(DateTime currentTime, Period period, List<Budget> budgets)
        {
            var total = 0;
            while (currentTime <= period.EndTime.AddMonths(1))
            {
                var budget = budgets.SingleOrDefault(x => x.YearMonth == currentTime.ToString("yyyyMM"));
                if (budget != null)
                {
                    var effectAmount = EffectAmount(period, budget);
                    total += effectAmount;
                }
                currentTime = currentTime.AddMonths(1);
            }
            return total;
        }

        private static int EffectAmount(Period period, Budget budget)
        {
            return period.OverlapDay(budget) * budget.DailyAmount();
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

        private static int GetMonthlyTotalBudget(DateTime time)
        {
            var budgets = _budgetRepository.GetBudgets();
            return budgets.SingleOrDefault(x => x.YearMonth == time.ToString("yyyyMM"))?.Amount ?? 0;
        }
    }
}