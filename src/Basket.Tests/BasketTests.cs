namespace FishAndChipShop.Basket.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoFixture.Xunit2;
    using Bogus;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Services;
    using FishAndChipShop.Basket.Tests.Fakes;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class BasketTests
    {
        public class AddTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldThrowArguementNullExceptionWhenNullValue(
                Basket sut)
            {
                Action a = () => sut.Add(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldAddProductToListOfProducts(
                Basket sut)
            {
                IProduct product = new ProductFaker().Generate();

                sut.Add(product);

                sut.Products.Should().Contain(product);
            }

            [Theory]
            [AutoMoqData]
            public void CanAddMultipleOfSameProduct(
                Basket sut)
            {
                IProduct product = new ProductFaker().Generate();

                sut.Add(product);
                sut.Add(product);

                sut.Products.Should().BeEquivalentTo(product, product);
            }

            [Theory]
            [AutoMoqData]
            public void ShouldReturnSuccessWhenAdded(
                Basket sut)
            {
                IProduct product = new ProductFaker().Generate();

                AddResult result = sut.Add(product);

                result.Success.Should().BeTrue();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldReturnFailedWhenProductNotValid(
                [Frozen]Mock<IProductValidationService> productValidationService,
                Basket sut)
            {
                IProduct product = new ProductFaker().Generate();

                productValidationService
                    .Setup(service => service.Validate(product))
                    .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Test", "Test failure") }));

                AddResult result = sut.Add(product);

                result.Success.Should().BeFalse();
            }
        }

        public class TotalCostTests
        {
            [Theory]
            [AutoMoqData]
            public void DiscountsShouldBeApplied(
                [Frozen]Mock<IDiscountService> discountServiceMock,
                Basket sut)
            {
                IEnumerable<IProduct> products = new ProductFaker().Generate(1);
                Price discountedPrice = new Price(new Faker().Finance.Amount());

                discountServiceMock
                    .Setup(service => service.ApplyDiscounts(products))
                    .Returns(discountedPrice);

                foreach (IProduct product in products)
                {
                    sut.Add(product);
                }

                sut.TotalCost.Should().Be(discountedPrice);
            }
        }
    }
}
