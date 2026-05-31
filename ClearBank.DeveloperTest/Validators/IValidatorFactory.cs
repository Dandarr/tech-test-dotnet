using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators
{
    public interface IValidatorFactory
    {
        IPaymentValidator GetValidator(PaymentScheme scheme);
    }
}