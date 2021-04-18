namespace FishAndChipShop.Basket.Tests.Services
{
    using System;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Services;
    using FishAndChipShop.Basket.Tests.Fakes;
    using FishAndChipShop.Basket.Validators;
    using FluentAssertions;
    using FluentValidation.Results;
    using Moq;
    using Xunit;

    public class ProductValidationServiceTests
    {
        public class ValidateTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldThrownArguementNullExceptionForNullProduct(
                ProductValidationService sut)
            {
                Action a = () => sut.Validate(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldReturnValidWhenNoValidators(
                ProductValidationService sut)
            {
                IProduct product = new ProductFaker().Generate();

                ValidationResult result = sut.Validate(product);

                result.IsValid.Should().BeTrue();
            }

            [Theory]
            [InlineAutoMoqData(true)]
            [InlineAutoMoqData(false)]
            public void ShouldReturnResultOfValidator(
                bool valid,
                ProductValidationService sut)
            {
                Mock<IProductValidator> productValidatorMock = new Mock<IProductValidator>();
                productValidatorMock
                    .Setup(validator => validator.Validate(It.IsAny<IProduct>()))
                    .Returns(() =>
                    {
                        if (valid)
                        {
                            return new ValidationResult();
                        }
                        else
                        {
                            return new ValidationResult(new[] { new ValidationFailure("Test", "Test validator") });
                        }
                    });

                sut.AddValidator(productValidatorMock.Object);

                IProduct product = new ProductFaker().Generate();

                ValidationResult result = sut.Validate(product);

                result.IsValid.Should().Be(valid);
            }
        }

        public class AddProductValidatorTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldThrownArgumentNullExceptionForNullValidator(
                ProductValidationService sut)
            {
                Action a = () => sut.AddValidator(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldAddValidatorToValidators(
                ProductValidationService sut)
            {
                IProductValidator productValidator = new FakeProductValidator();

                sut.AddValidator(productValidator);

                sut.Validators.Should().Contain(productValidator);
            }
        }
    }
}
