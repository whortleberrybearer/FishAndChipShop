namespace FishAndChipShop.Basket.Tests
{
    using AutoFixture.Xunit2;
    using FishAndChipShop.Basket;
    using FishAndChipShop.Basket.Discounts;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Services;
    using FishAndChipShop.Basket.Validators;
    using FluentAssertions;
    using NodaTime;
    using NodaTime.Testing;
    using NodaTime.Text;
    using Xunit;

    public class Scenarios
    {
        /// <summary>
        /// Given the basket is empty
        /// Then the total cost should be £0.0
        /// </summary>
        [Trait("part", "1")]
        [Theory]
        [AutoMoqData]
        public void EmptyBasketShouldBe0(
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            basket.TotalCost.Should().Be(0.0m);
        }

        /// <summary>
        /// Given a portion of chips costs £1.80
        /// When adding {numberOfBags} bag of chips to the basket
        /// The total cost of the basket should be {totalCost}
        ///
        /// | numberOfBags | totalCost |
        /// |--------------|-----------|
        /// | 1            | 1.80      |
        /// | 2            | 3.60      |
        /// | 10           | 18.0      |
        /// </summary>
        [Trait("part", "1")]
        [Theory]
        [InlineAutoMoqData(1, 1.80)]
        [InlineAutoMoqData(2, 3.60)]
        [InlineAutoMoqData(10, 18.0)]
        public void BasketShouldTotalNumberOfPortionsOfChips(
            int numberOfBags,
            decimal totalCost,
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            for (int i = 0; i < numberOfBags; i++)
            {
                PortionOfChips portionOfChips = new PortionOfChips(new Price(1.80m));

                basket.Add(portionOfChips);
            }

            basket.TotalCost.Should().Be(totalCost);
        }

        /// <summary>
        /// Given a pie costs £3.20
        /// When adding a pie to the basket
        /// The total cost of the basket should be £3.20
        /// </summary>
        [Trait("part", "2")]
        [Theory]
        [AutoMoqData]
        public void PieShouldCost320(
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            Pie pie = new Pie(new Price(3.20m), LocalDate.MaxIsoValue);

            basket.Add(pie);

            basket.TotalCost.Should().Be(3.20m);
        }

        /// <summary>
        /// Given a pie has an expiry date of {expiryDate}
        /// And the date is {currentDate}
        /// When adding a pie to the basket
        /// Then an error should be returned stating the pie has expired.
        ///
        /// | expiryDate | currentDate |
        /// |------------|-------------|
        /// | 1/1/2020   | 2/1/2020    |
        /// | 2/5/2020   | 2/6/2020    |
        /// </summary>
        [Trait("part", "2")]
        [Theory]
        [InlineAutoMoqData("01/01/2020", "02/01/2020")]
        [InlineAutoMoqData("02/05/2020", "02/06/2020")]
        public void CanNotSellAnExpiredPie(
            string expiryDateString,
            string currentDateString,
            [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
            [Frozen(Matching.ImplementedInterfaces)] ProductValidationService productValidationService,
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            productValidationService.AddValidator(new ExpiredPieValidator(fakeClock));

            LocalDate expiryDate = LocalDatePattern.CreateWithCurrentCulture("dd/MM/yyyy").Parse(expiryDateString).Value;
            Instant currentDate = InstantPattern.CreateWithCurrentCulture("dd/MM/yyyy").Parse(currentDateString).Value;

            fakeClock.Reset(currentDate);

            Pie pie = new Pie(default(Price), expiryDate);

            AddResult result = basket.Add(pie);

            result.Success.Should().BeFalse();
        }

        /// <summary>
        /// Given a pie costs £3.20
        /// And the pie expires today
        /// And a discount of {discountPercent}% is applied on the date of expiry
        /// When adding a pie to the basket
        /// The total cost of the basket should be {totalCost}
        ///
        /// | discountPercent | totalCost |
        /// |-----------------|-----------|
        /// | 50             | 1.60       |
        /// | 25             | 2.40       |
        /// </summary>
        [Trait("part", "2")]
        [Theory]
        [InlineAutoMoqData(50, 1.60)]
        [InlineAutoMoqData(25, 2.40)]
        public void PiesShouldBeDiscountedOnExpiryDate(
            int discountPercent,
            decimal totalCost,
            [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            discountService.AddProductDiscount(new PieExpiryDiscount(fakeClock, new DiscountPercent(discountPercent)));

            Pie pie = new Pie(new Price(3.20m), fakeClock.GetCurrentInstant().InUtc().Date);

            basket.Add(pie);

            basket.TotalCost.Should().Be(totalCost);
        }

        ///// <summary>
        ///// Given a pie costs £3.20
        ///// And 1 pie expires today
        ///// And another pie expires tomorrow
        ///// And a discount of 50% is applied on the date of expiry
        ///// When adding both pie to the basket
        ///// The total cost of the basket should be £4.80
        ///// </summary>
        [Trait("part", "2")]
        [Theory]
        [AutoMoqData]
        public void PieDiscountShouldOnlyAllyToExpiringPies(
            [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            discountService.AddProductDiscount(new PieExpiryDiscount(fakeClock, new DiscountPercent(50)));

            Pie pie1 = new Pie(new Price(3.20m), fakeClock.GetCurrentInstant().InUtc().Date);
            Pie pie2 = new Pie(new Price(3.20m), fakeClock.GetCurrentInstant().InUtc().Date.PlusDays(1));

            basket.Add(pie1);
            basket.Add(pie2);

            basket.TotalCost.Should().Be(4.80m);
        }

        /// <summary>
        /// Given a portion of chips costs £1.80
        /// And a pie costs £3.20
        /// And buying a portion of chips and a pie gets a meal deal discount of 20%
        /// When adding { numberOfPortionsOfChips }
        /// portions of chips to the basket
        /// And adding { numberOfPies } pies to the basket
        /// Then the total cost should be { totalCost }
        ///
        /// | numberOfPortionsOfChips | numberOfPies | totalCost |
        /// |-------------------------|--------------|-----------|
        /// | 1                       | 1            | 4         |
        /// | 2                       | 2            | 8         |
        /// | 3                       | 2            | 9.8       |
        /// </summary>
        [Trait("part", "3")]
        [Theory]
        [InlineAutoMoqData(1, 1, 4)]
        [InlineAutoMoqData(2, 2, 8)]
        [InlineAutoMoqData(3, 2, 9.8)]
        public void MealDealShouldBeAppliedWhenBuyingPieAndPortionOfChips(
            int numberOfPortionsOfChips,
            int numberOfPies,
            decimal totalCost,
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            discountService.AddProductDiscount(new PieAndChipsMealDealDiscount(new DiscountPercent(20)));

            for (int i = 0; i < numberOfPortionsOfChips; i++)
            {
                basket.Add(new PortionOfChips(new Price(1.80m)));
            }

            for (int i = 0; i < numberOfPies; i++)
            {
                basket.Add(new Pie(new Price(3.20m), LocalDate.MaxIsoValue));
            }

            basket.TotalCost.Should().Be(totalCost);
        }

        /// <summary>
        /// Given a portion of chips costs £1.80
        /// And a pie costs £3.20
        /// And buying a portion of chips and a pie gets a meal deal discount of 20%
        /// And a discount of 50% is applied on the date of expiry for pies
        /// When adding an expiring pie to the basket
        /// And adding a portion of chips to the basket
        /// Then the total cost should be £3.40
        /// </summary>
        [Trait("part", "3")]
        [Theory]
        [AutoMoqData]
        public void ProductsShouldNotBeIncludedInMultipleDiscounts(
            [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            discountService.AddProductDiscount(new PieAndChipsMealDealDiscount(new DiscountPercent(20)));
            discountService.AddProductDiscount(new PieExpiryDiscount(fakeClock, new DiscountPercent(50)));

            basket.Add(new Pie(new Price(3.20m), fakeClock.GetCurrentInstant().InUtc().Date));
            basket.Add(new PortionOfChips(new Price(1.80m)));

            basket.TotalCost.Should().Be(3.40m);
        }

        /// <summary>
        /// Given a portion of chips costs £1.80
        /// And a pie costs £3.20
        /// And buying a portion of chips and a pie gets a meal deal discount of {mealDealDiscountPercent
        /// And a discount of { expiringPieDiscountPercent } is applied on the date of expiry for pies
        /// When adding an expiring pie to the basket
        /// And adding 1 pie that is not expiring to the basket
        /// And adding 2 portions of chips to the basket
        /// Then the total cost should be { totalCost }
        ///
        /// | mealDealDiscountPercent | expiringPieDiscountPercent | totalCost |
        /// |-------------------------|----------------------------|-----------|
        /// | 20                      | 50                         | 7.4       |
        /// | 50                      | 10                         | 5         |
        /// </summary>
        [Trait("part", "3")]
        [Theory]
        [InlineAutoMoqData(20, 50, 7.4)]
        [InlineAutoMoqData(50, 10, 5)]
        public void DiscountsShouldCalculateTheLowestPrice(
            int mealDealDiscountPercent,
            int expiringPieDiscountPercent,
            decimal totalCost,
            [Frozen(Matching.ImplementedInterfaces)] FakeClock fakeClock,
            [Frozen(Matching.ImplementedInterfaces)] DiscountService discountService,
            Basket basket)
        {
            discountService.AddProductDiscount(new PieAndChipsMealDealDiscount(new DiscountPercent(mealDealDiscountPercent)));
            discountService.AddProductDiscount(new PieExpiryDiscount(fakeClock, new DiscountPercent(expiringPieDiscountPercent)));

            basket.Add(new Pie(new Price(3.20m), fakeClock.GetCurrentInstant().InUtc().Date));
            basket.Add(new Pie(new Price(3.20m), LocalDate.MaxIsoValue));

            for (int i = 0; i < 2; i++)
            {
                basket.Add(new PortionOfChips(new Price(1.80m)));
            }

            basket.TotalCost.Should().Be(totalCost);
        }
    }
}
