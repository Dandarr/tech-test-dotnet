using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Validators
{
    public class FasterPaymentsValidator : IPaymentValidator
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            //if account missing or not FasterPayments or insufficient funds return false else true
            return account != null &&
                   account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
                   account.Balance >= request.Amount;
        }
    }
}
