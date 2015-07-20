using EventSourcingAndCQRS.Models;
using EventSourcingAndCQRS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingAndCQRS.Tests.Fakes
{
    public class TotalNewAccountsPerDaySummarizer : EventSourcingAndCQRS.Services.Event.EventHandler<UserCreated>
    {
        private DailySummaryAccountsRepository DailySummaryAccountsRepository;

        public TotalNewAccountsPerDaySummarizer(DailySummaryAccountsRepository totalNewAccountsPerDayRepository) 
            : base(SubscriptionId.FromString("TotalNewAccountsPerDaySummarizer"))
        {
            DailySummaryAccountsRepository = totalNewAccountsPerDayRepository;
        }

        protected override void Handle(UserCreated evnt)
        {
            DailySummaryAccounts totalNewAccountsOfToday;
            if (DailySummaryAccountsRepository.HasDailySumary(DateTime.Today))
            {
                totalNewAccountsOfToday = DailySummaryAccountsRepository.FindByDate(DateTime.Today);
            }
            else
            {
                totalNewAccountsOfToday = new DailySummaryAccounts(DateTime.Today);
            }
            totalNewAccountsOfToday.IncrementTotalNewAccountsPerDay();
            DailySummaryAccountsRepository.Save(totalNewAccountsOfToday);
        }
    }
}
