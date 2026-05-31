using ClearBank.DeveloperTest.Types;
using System;

namespace ClearBank.DeveloperTest.Tests.Builders
{
    public class MakePaymentRequestBuilder
    {
        private string _debtorAccountNumber = "testDebtor01";
        private string _creditorAccountNumber = "testCreditor01";
        private decimal _amount = 100m;
        private PaymentScheme _paymentScheme = PaymentScheme.Bacs;
        private DateTime _paymentDate = DateTime.Now;

        public MakePaymentRequestBuilder WithDebtorAccountNumber(string accountNumber)
        {
            _debtorAccountNumber = accountNumber;
            return this;
        }

        public MakePaymentRequestBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public MakePaymentRequestBuilder WithPaymentScheme(PaymentScheme paymentScheme)
        {
            _paymentScheme = paymentScheme;
            return this;
        }

        public MakePaymentRequest Build()
        {
            return new MakePaymentRequest
            {
                DebtorAccountNumber = _debtorAccountNumber,
                CreditorAccountNumber = _creditorAccountNumber,
                Amount = _amount,
                PaymentScheme = _paymentScheme,
                PaymentDate = _paymentDate
            };
        }
    }
}