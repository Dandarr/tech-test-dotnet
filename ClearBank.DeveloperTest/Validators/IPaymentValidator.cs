using ClearBank.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Validators
{
    public interface IPaymentValidator
    {
        bool IsValid(Account account, MakePaymentRequest request);
    }
}
