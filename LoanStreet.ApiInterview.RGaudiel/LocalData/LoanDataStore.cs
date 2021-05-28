using System;
using LoanStreet.ApiInterview.RGaudiel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanStreet.ApiInterview.RGaudiel.LocalData
{

    public class LoanDataStore
    {
        public static LoanDataStore Current { get; } = new LoanDataStore();

        public List<LoanModel> Loans { get; set; }

        public LoanDataStore()
        {
            // init dummy data
            Loans = new List<LoanModel>()
            {
                new LoanModel()
                {
                     Id = 1,
                     OutstandingAmtCurrent = 150000.00m,
                     IntRateCurrent = 3.2585m,
                     RemainingTerm = 123,
                     MonthlyPrincipalPayment = 1000.0m
                },
                new LoanModel()
                {
                     Id = 2,
                     OutstandingAmtCurrent = 250000.00m,
                     IntRateCurrent = 3.12345678m,
                     RemainingTerm = 300,
                     MonthlyPrincipalPayment = 2222.0m
                },
                new LoanModel()
                {
                     Id = 3,
                     OutstandingAmtCurrent = 350000.00m,
                     IntRateCurrent = 3.2585m,
                     RemainingTerm = 123,
                     MonthlyPrincipalPayment = 3333.0m
                }
            };
        }

    }

}
