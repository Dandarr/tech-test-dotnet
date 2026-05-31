using System;

namespace ClearBank.DeveloperTest.Types
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; private set; }
        public AccountStatus Status { get; set; }
        public AllowedPaymentSchemes AllowedPaymentSchemes { get; set; }
        public void DeductBalance(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Amount must be positive");

            Balance -= amount;
        }
    }
}
