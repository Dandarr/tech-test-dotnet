using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly IValidatorFactory _validatorFactory;


        //gets config for dataStoreType since this doesnt have a program cs and i don't want to assume it will.
        public PaymentService()
            : this(new AccountDataStoreFactory().Create(), new PaymentValidatorFactory())
        {
        }

        public PaymentService(IAccountDataStore accountDataStore, IValidatorFactory validatorFactory)
        {
            _accountDataStore = accountDataStore;
            _validatorFactory = validatorFactory;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
            var result = new MakePaymentResult { Success = false };

            //Ask factory for validator
            var validator = _validatorFactory.GetValidator(request.PaymentScheme);

            //If validator exists, run the rules
            if (validator != null)
            {
                result.Success = validator.IsValid(account, request);
            }

            //If valid process the payment
            if (result.Success)
            {
                account.DeductBalance(request.Amount);

                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}