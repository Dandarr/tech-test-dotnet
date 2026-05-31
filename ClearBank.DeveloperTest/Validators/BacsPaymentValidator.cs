using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators
{
    public class BacsPaymentValidator : IPaymentValidator
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            //if exists and not Bacs or account missing return false else true
            return account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }
    }
}