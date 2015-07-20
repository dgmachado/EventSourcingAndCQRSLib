using EventSourcingAndCQRS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class DailySummaryAccountsRepository
    {
        private List<DailySummaryAccounts> DailySummary;

        public DailySummaryAccountsRepository()
        {
            DailySummary = new List<DailySummaryAccounts>();
        }
        
        internal DailySummaryAccounts FindByDate(DateTime dateTime)
        {
            return DailySummary.Single(s => s.Date == dateTime);
        }

        internal bool HasDailySumary(DateTime dateTime)
        {
            return DailySummary.SingleOrDefault(s => s.Date == dateTime) != null;
        }

        internal void RemoveDailySummary(DateTime dateTime)
        {
            DailySummary.Remove(DailySummary.Single(s => s.Date == dateTime));
        }

        internal void Save(DailySummaryAccounts dailySummaryAccounts)
        {
            if (DailySummary.SingleOrDefault(s => s.Date == dailySummaryAccounts.Date) != null)
            {
                RemoveDailySummary(dailySummaryAccounts.Date);
            }
            DailySummary.Add(dailySummaryAccounts);
        }

        private void AddDailySumary(DailySummaryAccounts dailySummaryAccounts)
        {
            DailySummary.Add(dailySummaryAccounts);
        }
    }
}
