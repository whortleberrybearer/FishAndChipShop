namespace FishAndChipShop.Basket.Tests.Discounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FishAndChipShop.Basket.Discounts;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Tests.Fakes;
    using FluentAssertions;
    using NodaTime;
    using Xunit;

    public class PieAndChipsMealDealDiscountTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldInitialiseDiscountPercent()
            {
                DiscountPercent discountPercent = new DiscountPercentFaker().Generate();

                PieAndChipsMealDealDiscount sut = new PieAndChipsMealDealDiscount(discountPercent);

                sut.DiscountPercent.Should().Be(discountPercent);
            }
        }

        public class CalculateDiscountsTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldThrowArgumentNullExceptionWhenProductsNull(
                PieAndChipsMealDealDiscount sut)
            {
                Action a = () => sut.CalculateDiscounts(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqData]
            public void NoDiscountShouldBeAppliedToEmptyProducts(
                PieAndChipsMealDealDiscount sut)
            {
                IEnumerable<Discount> results = sut.CalculateDiscounts(Enumerable.Empty<IProduct>());

                results.Should().BeEmpty();
            }

            [Theory]
            [AutoMoqData]
            public void PieAndPortionOfChipsShouldBeDiscounted(
                PieAndChipsMealDealDiscount sut)
            {
                PriceFaker priceFaker = new PriceFaker();

                Pie pie = new Pie(priceFaker.Generate(), LocalDate.MaxIsoValue);
                PortionOfChips portionOfChips = new PortionOfChips(priceFaker.Generate());

                IEnumerable<Discount> results = sut.CalculateDiscounts(new IProduct[] { pie, portionOfChips });

                Price expectedDicountPrice = new Price((pie.Price + portionOfChips.Price) * (sut.DiscountPercent / 100.0m));

                results.Should().Contain(new Discount(expectedDicountPrice, pie, portionOfChips));
            }

            [Theory]
            [AutoMoqData]
            public void DiscountShouldOnlyApplyToPieAndChips(
                PieAndChipsMealDealDiscount sut)
            {
                IEnumerable<Discount> results = sut.CalculateDiscounts(new ProductFaker().Generate(3));

                results.Should().BeEmpty();
            }

            [Theory]
            [AutoMoqData]
            public void NoChipsShouldNotBeIncludedInDiscount(
                PieAndChipsMealDealDiscount sut)
            {
                Pie pie = new Pie(new PriceFaker().Generate(), LocalDate.MaxIsoValue);

                IEnumerable<Discount> results = sut.CalculateDiscounts(new[] { pie });

                results.Should().BeEmpty();
            }

            [Theory]
            [AutoMoqData]
            public void NoPieShouldNotBeIncludedInDiscount(
                PieAndChipsMealDealDiscount sut)
            {
                PortionOfChips portionOfChips = new PortionOfChips(new PriceFaker().Generate());

                IEnumerable<Discount> results = sut.CalculateDiscounts(new[] { portionOfChips });

                results.Should().BeEmpty();
            }

            [Theory]
            [AutoMoqData]
            public void AdditionChipsShouldNotBeIncluded(
                PieAndChipsMealDealDiscount sut)
            {
                PriceFaker priceFaker = new PriceFaker();

                PortionOfChips portionOfChips = new PortionOfChips(priceFaker.Generate());
                Pie pie = new Pie(priceFaker.Generate(), LocalDate.MaxIsoValue);

                IEnumerable<Discount> results = sut.CalculateDiscounts(new IProduct[] { portionOfChips, portionOfChips, pie });

                Price expectedDicountPrice = new Price((pie.Price + portionOfChips.Price) * (sut.DiscountPercent / 100.0m));

                results.Should().Contain(new Discount(expectedDicountPrice, pie, portionOfChips));
            }

            [Theory]
            [AutoMoqData]
            public void AdditionPieShouldNotBeIncluded(
                PieAndChipsMealDealDiscount sut)
            {
                PriceFaker priceFaker = new PriceFaker();

                PortionOfChips portionOfChips = new PortionOfChips(priceFaker.Generate());
                Pie pie = new Pie(priceFaker.Generate(), LocalDate.MaxIsoValue);

                IEnumerable<Discount> results = sut.CalculateDiscounts(new IProduct[] { portionOfChips, pie, pie });

                Price expectedDicountPrice = new Price((pie.Price + portionOfChips.Price) * (sut.DiscountPercent / 100.0m));

                results.Should().Contain(new Discount(expectedDicountPrice, pie, portionOfChips));
            }
        }
    }
}
