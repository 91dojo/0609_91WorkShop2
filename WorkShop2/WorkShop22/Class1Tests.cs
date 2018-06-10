using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkShop2.Tests
{
    [TestClass()]
    public class Class1Tests
    {
        private WorkShop22.BudgetCalculator _budgetCalculator;
        private IRepository<Budget> _budgetRepository = Substitute.For<IRepository<Budget>>();
        [TestMethod()]
        public void AllYearBudget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 },
                new Budget() { YearMonth = "201807", Amount = 310 }
            );
            BudgetResultShouldBe(new DateTime(2018, 01, 1), new DateTime(2018, 12, 31), 890);
        }

        [TestMethod()]
        public void OneMonthFullBudget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2018, 6, 1), new DateTime(2018, 6, 30), 300m);
        }

        [TestMethod()]
        public void OneMonthPartialBudget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2018, 6, 1), new DateTime(2018, 6, 15), 150m);
        }

        [TestMethod()]
        public void OverThreeYearBudget_EndYearHas2Budget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 },
                new Budget() { YearMonth = "201807", Amount = 310 }
            );
            BudgetResultShouldBe(new DateTime(2017, 11, 1), new DateTime(2019, 1, 30), 890m);
        }

        [TestMethod()]
        public void OverYearBudget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 }
            );
            BudgetResultShouldBe(new DateTime(2017, 12, 1), new DateTime(2018, 2, 1), 10m);
        }

        [TestMethod()]
        public void OverYearBudget_EndYearHas2Budget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2017, 11, 1), new DateTime(2018, 6, 30), 580m);
        }

        [TestMethod()]
        public void OverYearBudget_StartDayBiggerThanEndDay()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201802", Amount = 280 },
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2017, 10, 10), new DateTime(2018, 6, 9), 370m);
        }

        [TestInitialize]
        public void TestInit()
        {
            _budgetCalculator = new WorkShop22.BudgetCalculator(_budgetRepository);
        }
     

        [TestMethod()]
        public void TwoMonth_StartMonthHasBudget_EndMonthNoBudget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201807", Amount = 310 }
            );
            BudgetResultShouldBe(new DateTime(2018, 7, 30), new DateTime(2018, 8, 22), 20m);
        }

        [TestMethod()]
        public void TwoMonth_StartMonthNoBudget_EndMonthHasBudget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201806", Amount = 300 }
            );
            BudgetResultShouldBe(new DateTime(2018, 5, 20), new DateTime(2018, 6, 10), 100m);
        }

        [TestMethod()]
        public void TwoMonthBothBudget()
        {
            GiveBudgets(
                new Budget() { YearMonth = "201806", Amount = 300 },
                new Budget() { YearMonth = "201807", Amount = 310 }
            );
            BudgetResultShouldBe(new DateTime(2018, 6, 15), new DateTime(2018, 7, 14), 300m);
        }

        [TestMethod()]
        public void TwoMonthBothNotBudget()
        {
            GiveBudgets(
                new Budget() {YearMonth = "201802", Amount = 280}
            );
            BudgetResultShouldBe(new DateTime(2018, 3, 1), new DateTime(2018, 4, 2), 0m);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void ThrowExpection()
        {

            var result = _budgetCalculator.Result(new DateTime(2018, 5, 1), new DateTime(2018, 4, 30));
        }
        private void BudgetResultShouldBe(DateTime startTime, DateTime endTime, decimal expected)
        {
            var actual = _budgetCalculator.Result(startTime, endTime);
            Assert.AreEqual(expected, actual);
        }

        private void GiveBudgets(params Budget[] budgets)
        {
            _budgetRepository.GetBudgets().Returns(budgets.ToList());
        }
    }
}