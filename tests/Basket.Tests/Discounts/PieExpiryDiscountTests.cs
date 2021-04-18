namespace FishAndChipShop.Basket.Tests.Discounts
{
    using System;
    using System.Collections.Generic;
    using AutoFixture.Xunit2;
    using FishAndChipShop.Basket.Discounts;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Tests.Fakes;
    using FluentAssertions;
    using NodaTime.Testing;
    using Xunit;

    public class PieExpiryDiscountTests
    {
        public class ConstructorTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldInitialiseDiscountPercent(
                FakeClock fakeClock)
            {
                DiscountPercent discountPercent = new DiscountPercentFaker().Generate();

                PieExpiryDiscount sut = new PieExpiryDiscount(fakeClock, discountPercent);

                sut.DiscountPercent.Should().Be(discountPercent);
            }
        }

        public class CalculateDiscountsTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldThrowArgumentNullExceptionWhenProductsNull(
                PieExpiryDiscount sut)
            {
                Action a = () => sut.CalculateDiscounts(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldOnlyApplyToPues(
                PieExpiryDiscount sut)
            {
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);

                IEnumerable<Discount> results = sut.CalculateDiscounts(products);

                results.Should().BeEmpty();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldNotReducePriceWhenNotExpired(
                [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
                PieExpiryDiscount sut)
            {
                Pie pie = new Pie(default, fakeClock.GetCurrentInstant().InUtc().Date.PlusDays(1));

                IEnumerable<Discount> results = sut.CalculateDiscounts(new[] { pie });

                results.Should().BeEmpty();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldReducePriceOnExpiryDay(
                [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
                PieExpiryDiscount sut)
            {
                PriceFaker priceFaker = new PriceFaker();
                Pie pie1 = new Pie(priceFaker.Generate(), fakeClock.GetCurrentInstant().InUtc().Date);
                Pie pie2 = new Pie(priceFaker.Generate(), fakeClock.GetCurrentInstant().InUtc().Date);

                IEnumerable<Discount> results = sut.CalculateDiscounts(new[] { pie1, pie2 });

                Price expectedPie1DicountPrice = new Price(pie1.Price * (sut.DiscountPercent / 100.0m));
                Price expectedPie2DicountPrice = new Price(pie2.Price * (sut.DiscountPercent / 100.0m));

                results.Should().BeEquivalentTo(new[] { new Discount(expectedPie1DicountPrice, pie1), new Discount(expectedPie2DicountPrice, pie2) });
            }
        }
    }
}
