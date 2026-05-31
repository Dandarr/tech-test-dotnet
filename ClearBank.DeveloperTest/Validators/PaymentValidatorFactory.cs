using ClearBank.DeveloperTest.Types;
using System.Collections.Generic;

namespace ClearBank.DeveloperTest.Validators
{
    public class PaymentValidatorFactory : IValidatorFactory
    {
        private readonly Dictionary<PaymentScheme, IPaymentValidator> _validators;

        public PaymentValidatorFactory()
        {
            _validators = new Dictionary<PaymentScheme, IPaymentValidator>
            {
                { PaymentScheme.Bacs, new BacsPaymentValidator() },
                { PaymentScheme.FasterPayments, new FasterPaymentsValidator() },
                { PaymentScheme.Chaps, new ChapsPaymentValidator() }
            };
        }

        public IPaymentValidator GetValidator(PaymentScheme scheme)
        {
            if (_validators.TryGetValue(scheme, out var validator))
            {
                return validator;
            }

            //If an unsupported scheme passed return null
            return null;
        }
    }
}