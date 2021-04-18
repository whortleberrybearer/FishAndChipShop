namespace FishAndChipShop.Basket.Tests.Fakes
{
    using System.Collections.Generic;
    using FishAndChipShop.Basket.Discounts;
    using FishAndChipShop.Basket.Products;

    public class FakeProductDiscount : IProductDiscount
    {
        private readonly Price discountPrice;
        private readonly IProduct? productToDiscount;

        public FakeProductDiscount()
        {
        }

        public FakeProductDiscount(Price discountPrice, IProduct productToDiscount)
        {
            this.discountPrice = discountPrice;
            this.productToDiscount = productToDiscount;
        }

        public IEnumerable<Discount> CalculateDiscounts(IEnumerable<IProduct> products)
        {
            List<Discount> discounts = new List<Discount>();

            foreach (IProduct product in products)
            {
                if (product == this.productToDiscount)
                {
                    discounts.Add(new Discount(this.discountPrice, product));
                }
            }

            return discounts;
        }
    }
}
