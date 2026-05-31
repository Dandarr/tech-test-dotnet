using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests.Builders
{
    public class AccountBuilder
    {
        private string _accountNumber = "testAccount1";
        private decimal _balance = 1000m;
        private AccountStatus _status = AccountStatus.Live;
        private AllowedPaymentSchemes _allowedPaymentSchemes =
            AllowedPaymentSchemes.Bacs |
            AllowedPaymentSchemes.FasterPayments |
            AllowedPaymentSchemes.Chaps;

        public AccountBuilder WithAccountNumber(string accountNumber)
        {
            _accountNumber = accountNumber;
            return this;
        }

        public AccountBuilder WithBalance(decimal balance)
        {
            _balance = balance;
            return this;
        }

        public AccountBuilder WithStatus(AccountStatus status)
        {
            _status = status;
            return this;
        }

        public AccountBuilder WithAllowedPaymentSchemes(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            _allowedPaymentSchemes = allowedPaymentSchemes;
            return this;
        }

        public Account Build()
        {
            var account = new Account
            {
                AccountNumber = _accountNumber,
                Status = _status,
                AllowedPaymentSchemes = _allowedPaymentSchemes
            };

            // Use Reflection to safely inject the balance into the private setter
            var propertyInfo = typeof(Account).GetProperty("Balance");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(account, _balance);
            }

            return account;
        }
    }
}