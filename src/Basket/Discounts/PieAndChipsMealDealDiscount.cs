namespace FishAndChipShop.Basket.Discounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FishAndChipShop.Basket.Products;

    public class PieAndChipsMealDealDiscount : IProductDiscount
    {
        public PieAndChipsMealDealDiscount(DiscountPercent discountPercent)
        {
            this.DiscountPercent = discountPercent;
        }

        public DiscountPercent DiscountPercent { get; }

        public IEnumerable<Discount> CalculateDiscounts(IEnumerable<IProduct> products)
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            IEnumerable<Pie> pies = products.OfType<Pie>();
            IEnumerable<PortionOfChips> portionsOfChips = products.OfType<PortionOfChips>();

            // A discount is only applied if a pie and a portion of chips exist, so find the minimum amount of each and batch them up.
            int totalDiscounts = Math.Min(pies.Count(), portionsOfChips.Count());

            List<Discount> discounts = new List<Discount>();

            for (int i = 0; i < totalDiscounts; i++)
            {
                Pie pie = pies.ElementAt(i);
                PortionOfChips portionOfChips = portionsOfChips.ElementAt(i);

                decimal discountValue = (pie.Price + portionOfChips.Price) * (this.DiscountPercent / 100.0m);

                discounts.Add(new Discount(new Price(discountValue), pie, portionOfChips));
            }

            return discounts;
        }
    }
}
