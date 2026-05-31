using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators;
using Xunit;

namespace ClearBank.DeveloperTest.Tests.Validators
{
    public class PaymentValidatorFactoryTests
    {
        private readonly PaymentValidatorFactory _sut; // SUT = System Under Test

        public PaymentValidatorFactoryTests()
        {
            _sut = new PaymentValidatorFactory();
        }

        [Fact]
        public void GetValidator_WhenSchemeIsBacs_ReturnsBacsPaymentValidator()
        {
            var result = _sut.GetValidator(PaymentScheme.Bacs);
            Assert.IsType<BacsPaymentValidator>(result);
        }

        [Fact]
        public void GetValidator_WhenSchemeIsFasterPayments_ReturnsFasterPaymentsValidator()
        {
            var result = _sut.GetValidator(PaymentScheme.FasterPayments);
            Assert.IsType<FasterPaymentsValidator>(result);
        }

        [Fact]
        public void GetValidator_WhenSchemeIsChaps_ReturnsChapsPaymentValidator()
        {

            var result = _sut.GetValidator(PaymentScheme.Chaps);
            Assert.IsType<ChapsPaymentValidator>(result);
        }

        [Fact]
        public void GetValidator_WhenSchemeIsUnknown_ReturnsNull()
        {
            // cast an arbitrary number to the enum to simulate an unsupported or newly added scheme
            var unsupportedScheme = (PaymentScheme)999;
            var result = _sut.GetValidator(unsupportedScheme);
            Assert.Null(result);
        }
    }
}