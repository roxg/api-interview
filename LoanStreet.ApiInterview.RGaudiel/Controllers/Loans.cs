using Microsoft.AspNetCore.Mvc;
using LoanStreet.ApiInterview.RGaudiel.LocalData;
using LoanStreet.ApiInterview.RGaudiel.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanStreet.ApiInterview.RGaudiel.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class Loans : ControllerBase
    {
        private readonly ILogger<Loans> _logger;

        public Loans(ILogger<Loans> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult GetLoans()
        {
            return Ok(LoanDataStore.Current.Loans);
        }

        [HttpGet("{id}")]
        public IActionResult GetLoans(int id)
        {
            // find loan
            var loanToReturn = LoanDataStore.Current.Loans
                .FirstOrDefault(c => c.Id == id);

            if (loanToReturn == null)
            {
                _logger.LogInformation($"GetLoans Action | ID NOT FOUND: {loanToReturn} - Loan ID was not found.");
                return NotFound();
            }

            _logger.LogInformation($"GetLoans Action | ID: {loanToReturn} returned successfully.");
            return Ok(loanToReturn);
        }

        // RG: Post to create new loan
        [HttpPost]
        public IActionResult PostNewLoan([FromBody] LoanModel loan)
        {
            var now = DateTime.Now;

            //RG: Increment this in the local datastore. When data is persisted to a database, the identity column will be defaulted
            var Id = LoanDataStore.Current.Loans.Any() ?
             LoanDataStore.Current.Loans.Max(p => p.Id) + 1 : 1;

            var newLoan = new LoanModel()
            {
                Id                      = Id,
                OutstandingAmtCurrent   = loan.OutstandingAmtCurrent,
                IntRateCurrent          = loan.IntRateCurrent,
                RemainingTerm           = loan.RemainingTerm,
                MonthlyPrincipalPayment = loan.MonthlyPrincipalPayment,
                IsActive                = 1,
                CreatedBy               = loan.CreatedBy,
                CreatedDt               = now,
                UpdatedBy               = loan.UpdatedBy,
                UpdateDt                = now

            };

            LoanDataStore.Current.Loans.Add(newLoan);

            //ToDo: Fix output. It returns 0 rather than the new ID
            return CreatedAtAction("PostNewLoan", Id, loan);

        }

        // RG: Update loan with Patch
        [HttpPatch("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: Will not include a delete.  Loans should only be soft deleted through patch
        
    }
}
