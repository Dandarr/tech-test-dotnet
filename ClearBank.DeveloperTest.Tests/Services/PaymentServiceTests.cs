using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using ClearBank.DeveloperTest.Tests.Builders;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IAccountDataStore> _mockAccountDataStore;
        private readonly Mock<IValidatorFactory> _mockValidatorFactory;
        private readonly PaymentService _sut; // SUT = System Under Test

        public PaymentServiceTests()
        {
            //Initialize mocks
            _mockAccountDataStore = new Mock<IAccountDataStore>();
            _mockValidatorFactory = new Mock<IValidatorFactory>();

            //Inject into refactored service
            _sut = new PaymentService(_mockAccountDataStore.Object, _mockValidatorFactory.Object);
        }

        [Fact]
        public void MakePayment_WhenValidatorFails_ReturnsSuccessFalse_AndDoesNotUpdateAccount()
        {
            var request = new MakePaymentRequestBuilder()
                .WithPaymentScheme(PaymentScheme.Bacs)
                .WithAmount(50m)
                .Build();

            var account = new AccountBuilder().WithBalance(100m).Build();

            var mockValidator = new Mock<IPaymentValidator>();
            //Force the validator to return FALSE
            mockValidator.Setup(v => v.IsValid(account, request)).Returns(false);

            _mockAccountDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber)).Returns(account);
            _mockValidatorFactory.Setup(vf => vf.GetValidator(request.PaymentScheme)).Returns(mockValidator.Object);

            var result = _sut.MakePayment(request);
            Assert.False(result.Success);

            //Verify that we NEVER called the database to save an update
            _mockAccountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void MakePayment_WhenValidatorIsNull_ReturnsSuccessFalse_AndDoesNotUpdateAccount()
        {

            var request = new MakePaymentRequestBuilder().Build();
            var account = new AccountBuilder().WithBalance(100m).Build();

            _mockAccountDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber)).Returns(account);

            //Force factory to return NULL (simulating an unsupported/missing payment scheme)
            _mockValidatorFactory.Setup(vf => vf.GetValidator(request.PaymentScheme)).Returns((IPaymentValidator)null);
            var result = _sut.MakePayment(request);
            Assert.False(result.Success);
            _mockAccountDataStore.Verify(ds => ds.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Fact]
        public void MakePayment_WhenValidatorSucceeds_UpdatesAccountBalance_AndReturnsSuccessTrue()
        {
            var request = new MakePaymentRequestBuilder()
                .WithPaymentScheme(PaymentScheme.FasterPayments)
                .WithAmount(50m)
                .Build();

            var account = new AccountBuilder().WithBalance(100m).Build();

            var mockValidator = new Mock<IPaymentValidator>();
            //Force validator to return TRUE
            mockValidator.Setup(v => v.IsValid(account, request)).Returns(true);

            _mockAccountDataStore.Setup(ds => ds.GetAccount(request.DebtorAccountNumber)).Returns(account);
            _mockValidatorFactory.Setup(vf => vf.GetValidator(request.PaymentScheme)).Returns(mockValidator.Object);
            var result = _sut.MakePayment(request);
            Assert.True(result.Success);
            Assert.Equal(50m, account.Balance);

            //Verify that we DID call database EXACTLY ONCE to save updated account
            _mockAccountDataStore.Verify(ds => ds.UpdateAccount(account), Times.Once);
        }
    }
}