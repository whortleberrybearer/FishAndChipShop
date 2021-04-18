namespace FishAndChipShop.Basket.Tests.Products
{
    using FishAndChipShop.Basket.Products;
    using FluentAssertions;
    using Xunit;

    public class PortionOfChipsTests
    {
        public class ConstructorTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldInitialisePrice(
                Price price)
            {
                PortionOfChips sut = new PortionOfChips(price);

                sut.Price.Should().Be(price);
            }
        }
    }
}
