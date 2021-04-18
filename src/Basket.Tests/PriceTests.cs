namespace FishAndChipShop.Basket.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class PriceTests
    {
        public class ConstructorTests
        {
            [Theory]
            [InlineData(-3)]
            [InlineData(-0.01)]
            public void PriceMustBe0OrAbove(decimal value)
            {
                Action a = () => new Price(value);

                a.Should().Throw<ArgumentOutOfRangeException>();
            }
        }

        public class EqualsOperatorTests
        {
            [Theory]
            [InlineData(1)]
            [InlineData(0.5)]
            [InlineData(7.65)]
            public void PricesShouldBeEqual(decimal value)
            {
                Price price1 = new Price(value);
                Price price2 = new Price(value);

                bool result = price1 == price2;

                result.Should().BeTrue();
            }

            [Theory]
            [InlineData(0, 1)]
            [InlineData(0.5, 0.51)]
            [InlineData(7.65, 76.5)]
            public void PricesShouldNotBeEqual(decimal value1, decimal value2)
            {
                Price price1 = new Price(value1);
                Price price2 = new Price(value2);

                bool result = price1 == price2;

                result.Should().BeFalse();
            }
        }

        public class NotEqualsOperatorTests
        {
            [Theory]
            [InlineData(1)]
            [InlineData(0.5)]
            [InlineData(7.65)]
            public void PricesShouldBeNotEqual(decimal value)
            {
                Price price1 = new Price(value);
                Price price2 = new Price(value);

                bool result = price1 != price2;

                result.Should().BeFalse();
            }

            [Theory]
            [InlineData(0, 1)]
            [InlineData(0.5, 0.51)]
            [InlineData(7.65, 76.5)]
            public void PricesShouldNotBeEqual(decimal value1, decimal value2)
            {
                Price price1 = new Price(value1);
                Price price2 = new Price(value2);

                bool result = price1 != price2;

                result.Should().BeTrue();
            }
        }

        public class EqualsObjectTests
        {
            [Theory]
            [InlineData(1)]
            [InlineData(0.5)]
            [InlineData(7.65)]
            public void PricesShouldBeObjectEqual(decimal value)
            {
                Price price1 = new Price(value);
                Price price2 = new Price(value);

                bool result = price1.Equals((object)price2);

                result.Should().BeTrue();
            }
        }

        public class DecimalOperatorTests
        {
            [Theory]
            [InlineData(1)]
            [InlineData(0.5)]
            public void PriceShouldActLikeDecimal(decimal value)
            {
                Price price = new Price(value);

                price.Should().Be(value);
            }
        }
    }
}
