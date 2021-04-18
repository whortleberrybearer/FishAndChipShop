namespace FishAndChipShop.Basket.Tests.Validators
{
    using AutoFixture.Xunit2;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Tests.Fakes;
    using FishAndChipShop.Basket.Validators;
    using FluentAssertions;
    using FluentValidation.Results;
    using NodaTime.Testing;
    using Xunit;

    public class PieValidatorTests
    {
        public class ValidateTests
        {
            [Theory]
            [InlineAutoMoqData(1)]
            [InlineAutoMoqData(3)]
            public void ExpiredPieShouldNotBeValid(
                int advanceDays,
                [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
                ExpiredPieValidator sut)
            {
                Pie pie = new Pie(default(Price), fakeClock.GetCurrentInstant().InUtc().LocalDateTime.Date);

                // Advance the clock forward so the pie is now expired.
                fakeClock.AdvanceDays(advanceDays);

                ValidationResult result = sut.Validate(pie);

                result.IsValid.Should().BeFalse();
            }

            [Theory]
            [InlineAutoMoqData(0)]
            [InlineAutoMoqData(-1)]
            [InlineAutoMoqData(-4)]
            public void UnexpiredPieShouldBeValid(
                int advanceDays,
                [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
                ExpiredPieValidator sut)
            {
                Pie pie = new Pie(default(Price), fakeClock.GetCurrentInstant().InUtc().LocalDateTime.Date);

                // Roll the clock back so the pie will not have expired.
                fakeClock.AdvanceDays(advanceDays);

                ValidationResult result = sut.Validate(pie);

                result.IsValid.Should().BeTrue();
            }

            [Theory]
            [AutoMoqData]
            public void NonPiesShouldBeValid(
                ExpiredPieValidator sut)
            {
                IProduct product = new ProductFaker().Generate();

                ValidationResult result = sut.Validate(product);

                result.IsValid.Should().BeTrue();
            }
        }
    }
}
