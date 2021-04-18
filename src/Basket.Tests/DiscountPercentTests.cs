namespace FishAndChipShop.Basket.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class DiscountPercentTests
    {
        public class ConstructorTests
        {
            [Theory]
            [InlineData(-1)]
            [InlineData(-3)]
            [InlineData(101)]
            [InlineData(102)]
            public void MustNotBeBelow1OrAbove100(int value)
            {
                Action a = () => new DiscountPercent(value);

                a.Should().Throw<ArgumentOutOfRangeException>();
            }
        }

        public class EqualsOperatorTests
        {
            [Theory]
            [InlineData(27)]
            public void PricesShouldBeEqual(int value)
            {
                DiscountPercent discountPercent1 = new DiscountPercent(value);
                DiscountPercent discountPercent2 = new DiscountPercent(value);

                bool result = discountPercent1 == discountPercent2;

                result.Should().BeTrue();
            }

            [Theory]
            [InlineData(1, 2)]
            [InlineData(23, 45)]
            public void DiscountPercentsShouldNotBeEqual(int value1, int value2)
            {
                DiscountPercent discountPercent1 = new DiscountPercent(value1);
                DiscountPercent discountPercent2 = new DiscountPercent(value2);

                bool result = discountPercent1 == discountPercent2;

                result.Should().BeFalse();
            }
        }

        public class NotEqualsOperatorTests
        {
            [Theory]
            [InlineData(27)]
            public void PricesShouldBeEqual(int value)
            {
                DiscountPercent discountPercent1 = new DiscountPercent(value);
                DiscountPercent discountPercent2 = new DiscountPercent(value);

                bool result = discountPercent1 != discountPercent2;

                result.Should().BeFalse();
            }

            [Theory]
            [InlineData(1, 2)]
            [InlineData(23, 45)]
            public void DiscountPercentsShouldNotBeEqual(int value1, int value2)
            {
                DiscountPercent discountPercent1 = new DiscountPercent(value1);
                DiscountPercent discountPercent2 = new DiscountPercent(value2);

                bool result = discountPercent1 != discountPercent2;

                result.Should().BeTrue();
            }
        }

        public class EqualsObjectTests
        {
            [Theory]
            [InlineData(6)]
            public void DiscountPercentsShouldBeObjectEqual(int value)
            {
                DiscountPercent discountPercent1 = new DiscountPercent(value);
                DiscountPercent discountPercent2 = new DiscountPercent(value);

                bool result = discountPercent1.Equals((object)discountPercent2);

                result.Should().BeTrue();
            }
        }

        public class IntOperatorTests
        {
            [Theory]
            [InlineData(18)]
            public void DiscountPercentShouldActLikeInt(int value)
            {
                DiscountPercent discountPercent = new DiscountPercent(value);

                discountPercent.Should().Be(value);
            }
        }
    }
}
