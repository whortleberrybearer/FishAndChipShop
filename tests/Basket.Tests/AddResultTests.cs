namespace FishAndChipShop.Basket.Tests
{
    using FluentAssertions;
    using Xunit;

    public class AddResultTests
    {
        public class ConstrutorTests
        {
            [Theory]
            [InlineAutoMoqData(true)]
            [InlineAutoMoqData(false)]
            public void ShouldInitialiseSuccess(
                bool succedd)
            {
                AddResult sut = new AddResult(succedd);

                sut.Success.Should().Be(succedd);
            }
        }
    }
}
