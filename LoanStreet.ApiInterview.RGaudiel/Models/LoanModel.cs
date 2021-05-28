using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace LoanStreet.ApiInterview.RGaudiel.Models
{
    /// <summary>
    /// Standard Loan Attributes
    /// </summary>
    public class LoanModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Loans must have an outstanding amount.")]
        public decimal OutstandingAmtCurrent { get; set; } /* ToDo: Add validation, > 0 */
        public decimal IntRateCurrent { get; set; } 
        public int RemainingTerm { get; set; } /* ToDo: Add validation, > 0 */
        public decimal MonthlyPrincipalPayment { get; set; } /* ToDo: Add validation, > 0 */
        [Required]
        public int IsActive { get; set; }
        [Required]
        public DateTime CreatedDt { get; set; } 
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime UpdateDt { get; set; }
        [Required]
        public string UpdatedBy { get; set; }

    }
}
