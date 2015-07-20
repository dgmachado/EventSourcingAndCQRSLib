using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    internal class DailySummaryAccounts
    {
        public DateTime Date { get; private set; }
        public int TotalNewAccountsPerDay { get; private set; }

        public DailySummaryAccounts(DateTime date)
        {
            Date = date;
            TotalNewAccountsPerDay = 0;
        }

        internal void IncrementTotalNewAccountsPerDay()
        {
            TotalNewAccountsPerDay++;
        }
    }
}
