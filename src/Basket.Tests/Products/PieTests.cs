namespace FishAndChipShop.Basket.Tests.Products
{
    using FishAndChipShop.Basket.Products;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using NodaTime;
    using Xunit;

    public class PieTests
    {
        public class ConstructorTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldInitialisePriceAndExpiryDate(
                Price price,
                LocalDate expiryDate)
            {
                Pie sut = new Pie(price, expiryDate);

                using (new AssertionScope())
                {
                    sut.Price.Should().Be(price);
                    sut.ExpiryDate.Should().Be(expiryDate);
                }
            }
        }
    }
}
