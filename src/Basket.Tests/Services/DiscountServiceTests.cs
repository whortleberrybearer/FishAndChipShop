namespace FishAndChipShop.Basket.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FishAndChipShop.Basket.Discounts;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Services;
    using FishAndChipShop.Basket.Tests.Fakes;
    using FluentAssertions;
    using Xunit;

    public class DiscountServiceTests
    {
        public class ApplyDiscountTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldThrownArguementNullExceptionForNullProducts(
                DiscountService sut)
            {
                Action a = () => sut.ApplyDiscounts(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldBe0WhenNoProductsToDiscount(
                DiscountService sut)
            {
                Price result = sut.ApplyDiscounts(Enumerable.Empty<IProduct>());

                result.Should().Be(0.0m);
            }

            [Theory]
            [AutoMoqData]
            public void ShouldEqualSumOfProductsWhenNoDiscounts(
                DiscountService sut)
            {
                IEnumerable<IProduct> products = new ProductFaker().Generate(3);

                Price result = sut.ApplyDiscounts(products);

                result.Should().Be(products.Sum(p => p.Price));
            }

            [Theory]
            [AutoMoqData]
            public void ShouldApplyDiscountToValidProducts(
                DiscountService sut)
            {
                // product1 is valid for discount, product2 is not.
                ProductFaker productFaker = new ProductFaker();
                IProduct product1 = productFaker.Generate();
                IProduct product2 = productFaker.Generate();

                Price discountPrice = new Price(product2.Price * 0.5m);
                IProductDiscount discount = new FakeProductDiscount(discountPrice, product2);

                sut.AddProductDiscount(discount);

                Price result = sut.ApplyDiscounts(new[] { product1, product2 });

                result.Should().Be(new Price(product1.Price + discountPrice));
            }

            [Theory]
            [AutoMoqData]
            public void ProductsShouldOnlyBeValidForOneDiscount(
                DiscountService sut)
            {
                IProduct product = new ProductFaker().Generate();

                Price discountPrice = new Price(product.Price * 0.1m);
                IProductDiscount discount = new FakeProductDiscount(discountPrice, product);

                // Adding the same discount twice, to it would apply to the product multiple times,
                sut.AddProductDiscount(discount);
                sut.AddProductDiscount(discount);

                Price result = sut.ApplyDiscounts(new[] { product });

                result.Should().Be(new Price(product.Price - discountPrice));
            }

            [Theory]
            [AutoMoqData]
            public void LowestPriceFromAllDiscountsShouldBeApplied(
                DiscountService sut)
            {
                IProduct product = new ProductFaker().Generate();

                Price discountPrice1 = new Price(product.Price * 0.1m);
                IProductDiscount discount1 = new FakeProductDiscount(discountPrice1, product);
                Price discountPrice2 = new Price(product.Price * 0.2m);
                IProductDiscount discount2 = new FakeProductDiscount(discountPrice2, product);

                // discount2 provides more saving that discount1, so that should be applied.
                sut.AddProductDiscount(discount1);
                sut.AddProductDiscount(discount2);

                Price result = sut.ApplyDiscounts(new[] { product });

                result.Should().Be(new Price(product.Price - discountPrice2));
            }
        }

        public class AddProductDiscountTests
        {
            [Theory]
            [AutoMoqData]
            public void ShouldThrownArgumentNullExceptionForNullDiscount(
                DiscountService sut)
            {
                Action a = () => sut.AddProductDiscount(null!);

                a.Should().Throw<ArgumentNullException>();
            }

            [Theory]
            [AutoMoqData]
            public void ShouldAddDiscountToDiscounts(
                DiscountService sut)
            {
                IProductDiscount discount = new FakeProductDiscount();

                sut.AddProductDiscount(discount);

                sut.ProductDiscounts.Should().Contain(discount);
            }
        }
    }
}
