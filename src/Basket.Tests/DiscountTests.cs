namespace FishAndChipShop.Basket.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Tests.Fakes;
    using FluentAssertions;
    using Xunit;

    public class DiscountTests
    {
        public class ConstructorTests
        {
            [Fact]
            public void ShouldThrowArguementNullExceptionWhenProductsIsNull()
            {
                Action a = () => new Discount(default, null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ShouldSetProducts()
            {
                ProductFaker productFaker = new ProductFaker();
                IEnumerable<IProduct> products = productFaker.Generate(3);

                Discount sut = new Discount(default, products.ToArray());

                sut.Products.Should().BeEquivalentTo(products);
            }

            [Fact]
            public void ShouldSetPrice()
            {
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);
                Price discountPrice = new PriceFaker().Generate();

                Discount sut = new Discount(discountPrice, products.ToArray());

                sut.DiscountPrice.Should().Be(discountPrice);
            }
        }

        public class EqualsOperatorTests
        {
            [Fact]
            public void DiscountsShouldBeEqual()
            {
                Price price = new PriceFaker().Generate();
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);

                Discount discount1 = new Discount(price, products.ToArray());
                Discount discount2 = new Discount(price, products.ToArray());

                bool result = discount1 == discount2;

                result.Should().BeTrue();
            }

            [Fact]
            public void DiscountsShouldNotBeEqualForDifferentPrice()
            {
                PriceFaker priceFaker = new PriceFaker();

                Price price1 = priceFaker.Generate();
                Price price2 = priceFaker.Generate();
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);

                Discount discount1 = new Discount(price1, products.ToArray());
                Discount discount2 = new Discount(price2, products.ToArray());

                bool result = discount1 == discount2;

                result.Should().BeFalse();
            }

            [Fact]
            public void DiscountsShouldNotBeEqualForDifferentProducts()
            {
                ProductFaker productsFaker = new ProductFaker();

                Price price = new PriceFaker().Generate();
                IEnumerable<IProduct> products1 = productsFaker.Generate(3);
                IEnumerable<IProduct> products2 = productsFaker.Generate(3);

                Discount discount1 = new Discount(price, products1.ToArray());
                Discount discount2 = new Discount(price, products2.ToArray());

                bool result = discount1 == discount2;

                result.Should().BeFalse();
            }
        }

        public class NotEqualsOperatorTests
        {
            [Fact]
            public void DiscountsShouldBeNotEqual()
            {
                Price price = new PriceFaker().Generate();
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);

                Discount discount1 = new Discount(price, products.ToArray());
                Discount discount2 = new Discount(price, products.ToArray());

                bool result = discount1 != discount2;

                result.Should().BeFalse();
            }

            [Fact]
            public void DiscountsShouldNotBeEqualForDifferentPrice()
            {
                PriceFaker priceFaker = new PriceFaker();

                Price price1 = priceFaker.Generate();
                Price price2 = priceFaker.Generate();
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);

                Discount discount1 = new Discount(price1, products.ToArray());
                Discount discount2 = new Discount(price2, products.ToArray());

                bool result = discount1 != discount2;

                result.Should().BeTrue();
            }

            [Fact]
            public void DiscountsShouldNotBeEqualForDifferentProducts()
            {
                ProductFaker productsFaker = new ProductFaker();

                Price price = new PriceFaker().Generate();
                IEnumerable<IProduct> products1 = productsFaker.Generate(3);
                IEnumerable<IProduct> products2 = productsFaker.Generate(3);

                Discount discount1 = new Discount(price, products1.ToArray());
                Discount discount2 = new Discount(price, products2.ToArray());

                bool result = discount1 != discount2;

                result.Should().BeTrue();
            }
        }

        public class EqualsObjectTests
        {
            [Fact]
            public void DiscountsShouldBeObjectEqual()
            {
                Price price = new PriceFaker().Generate();
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);

                Discount discount1 = new Discount(price, products.ToArray());
                Discount discount2 = new Discount(price, products.ToArray());

                bool result = discount1.Equals((object)discount2);

                result.Should().BeTrue();
            }
        }
    }
}
