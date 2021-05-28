using Microsoft.AspNetCore.Mvc;
using LoanStreet.ApiInterview.RGaudiel.LocalData;
using LoanStreet.ApiInterview.RGaudiel.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;

namespace LoanStreet.ApiInterview.RGaudiel.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class Loans : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Loans));
        //ToDo: Add authentication, add throttling

        [HttpGet]
        public IActionResult GetLoans()
        {
            //ToDo: Add pagination, check timeout settings when DB is implemented. In memory will only be as slow as garbage collection allows
            //ToDo: Depending on usage, make query language for API, depending on entity mapping, use GraphQL
            try
            {
                log.Info($"GetLoans Action | Start retrieving all loans.");
                return Ok(LoanDataStore.Current.Loans);
            }
            catch (Exception ex)
            {
                log.Error($"GetLoans Action | ERROR PROCESSING get operation to retrieve all Loans. {ex}");
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetLoans(int id)
        {
            try
            {
                // find loan
                log.Info($"GetLoans by ID Action | Loan ID: {id} Retrieving info for loan.");

                var loanToReturn = LoanDataStore.Current.Loans
                    .FirstOrDefault(c => c.Id == id);

                if (loanToReturn == null)
                {
                    log.Error($"GetLoans Action | ID NOT FOUND: {loanToReturn} - Loan ID was not found.");
                    return NotFound();
                }

                log.Debug($"GetLoans Action | ID: {loanToReturn} returned successfully.");
                return Ok(loanToReturn);
            }
            catch (Exception ex)
            {
                log.Error($"GetLoans Action | ERROR PROCESSING get operation. {ex}");
                return NoContent();
            }

        }

        // RG: Post to create new loan
        [HttpPost]
        public IActionResult PostNewLoan([FromBody] LoanModel loan)
        {
            try
            {
                var now = DateTime.Now;
                log.Info($"PostNewLoan Action | Start posting new loan.");

                //RG: Increment this in the local datastore. When data is persisted to a database, the identity column will be defaulted
                var id = LoanDataStore.Current.Loans.Any() ?
                 LoanDataStore.Current.Loans.Max(p => p.Id) + 1 : 1;

                var newLoan = new LoanModel()
                {
                    Id                          = id,
                    OutstandingAmtCurrent       = loan.OutstandingAmtCurrent,
                    IntRateCurrent              = loan.IntRateCurrent,
                    RemainingTerm               = loan.RemainingTerm,
                    MonthlyPrincipalPayment     = loan.MonthlyPrincipalPayment,
                    IsActive                    = 1,
                    CreatedBy                   = loan.CreatedBy,
                    CreatedDt                   = now,
                    UpdatedBy                   = loan.UpdatedBy,
                    UpdateDt                    = now

                };

                if (newLoan == null)
                {
                    return NotFound();
                }

                //ToDo: create database connection rather than in memory data store
                LoanDataStore.Current.Loans.Add(newLoan);

                log.Debug($"PostNewLoan Action | Loan ID: {id} created successfully.");

                //ToDo: Fix output. It returns 0 rather than the new ID
                return CreatedAtAction("PostNewLoan", id, loan);
            }
            catch (Exception ex)
            {
                log.Error($"PatchLoan Action | ERROR PROCESSING Patch operation. {ex}");
                return NoContent();
            }

        }

        [HttpPut("{id}")]
        public IActionResult PutLoan(int id, [FromBody] LoanModel putLoan)
        {
            try
            {
                log.Info($"PutLoan Action | Loan ID: {id} start patch process.");

                var now = DateTime.Now;

                //ToDo: Add validation to check if loan id exists in data store

                var loan = LoanDataStore.Current.Loans
                    .FirstOrDefault(c => c.Id == id);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                loan.OutstandingAmtCurrent      = putLoan.OutstandingAmtCurrent;
                loan.IntRateCurrent             = putLoan.IntRateCurrent;
                loan.RemainingTerm              = putLoan.RemainingTerm;
                loan.MonthlyPrincipalPayment    = putLoan.MonthlyPrincipalPayment;
                loan.IsActive                   = putLoan.IsActive;
                loan.UpdatedBy                  = putLoan.UpdatedBy;
                loan.UpdateDt                   = now;

                log.Debug($"PutLoan Action | Loan ID: {id} updated successfully.");

                return NoContent();

            }
            catch (Exception ex)
            {
                log.Error($"PutLoan Action | Loan ID: {id} ERROR PROCESSING Patch operation. {ex}");
                return NoContent();
            }
            
        }

        // DELETE: Will not include a delete.  Loans should only be soft deleted through put

    }
}
