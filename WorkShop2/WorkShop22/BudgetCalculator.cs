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

            return _budgetRepository.GetBudgets().Sum(x => x.EffectAmount(period));
        }
    }
}