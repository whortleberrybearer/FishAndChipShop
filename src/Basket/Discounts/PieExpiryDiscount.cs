namespace FishAndChipShop.Basket.Discounts
{
    using System;
    using System.Collections.Generic;
    using FishAndChipShop.Basket.Products;
    using NodaTime;

    public class PieExpiryDiscount : IProductDiscount
    {
        private readonly IClock clock;

        public PieExpiryDiscount(IClock clock, DiscountPercent discountPercent)
        {
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
            this.DiscountPercent = discountPercent;
        }

        public DiscountPercent DiscountPercent { get; }

        public IEnumerable<Discount> CalculateDiscounts(IEnumerable<IProduct> products)
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            List<Discount> discounts = new List<Discount>();

            foreach (IProduct product in products)
            {
                // If we are a pie and on the expiry date, then the discount can be applied.
                if (product is Pie pie && (pie.ExpiryDate == this.clock.GetCurrentInstant().InUtc().Date))
                {
                    decimal discountValue = pie.Price * (this.DiscountPercent / 100.0m);

                    discounts.Add(new Discount(new Price(discountValue), pie));
                }
            }

            return discounts;
        }
    }
}
