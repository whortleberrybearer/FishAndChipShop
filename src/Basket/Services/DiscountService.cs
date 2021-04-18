namespace FishAndChipShop.Basket.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FishAndChipShop.Basket.Discounts;
    using FishAndChipShop.Basket.Products;

    public class DiscountService : IDiscountService
    {
        private readonly List<IProductDiscount> productDiscounts = new List<IProductDiscount>();

        public IReadOnlyList<IProductDiscount> ProductDiscounts => this.productDiscounts.AsReadOnly();

        public Price ApplyDiscounts(IEnumerable<IProduct> products)
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            IEnumerable<Discount> discounts = this.CalculateBestDiscount(products, this.productDiscounts);

            decimal totalPrice = products.Sum(product => product.Price);
            decimal totalDiscounts = discounts.Sum(discount => discount.DiscountPrice);

            return new Price(totalPrice - totalDiscounts);
        }

        public void AddProductDiscount(IProductDiscount productDiscount)
        {
            if (productDiscount is null)
            {
                throw new ArgumentNullException(nameof(productDiscount));
            }

            this.productDiscounts.Add(productDiscount);
        }

        private IEnumerable<Discount> CalculateBestDiscount(IEnumerable<IProduct> products, IEnumerable<IProductDiscount> productDiscounts)
        {
            IEnumerable<Discount> bestDiscounts = Enumerable.Empty<Discount>();

            // This will loop all the discounts, the recursively call again to determine the best combination of discounts across all the products.
            foreach (IProductDiscount productDiscount in productDiscounts)
            {
                // Take a copy of the passed in products as we are going to be removing them from the list once a discount is applied to it.
                List<IProduct> productsToDiscount = new List<IProduct>(products);

                IEnumerable<Discount> discountsToApply = productDiscount.CalculateDiscounts(productsToDiscount);

                foreach (IProduct discountedProduct in discountsToApply.SelectMany(discount => discount.Products))
                {
                    productsToDiscount.Remove(discountedProduct);
                }

                if (productsToDiscount.Any())
                {
                    // Now work out the best discounts from the remaining discounts and products.
                    discountsToApply = discountsToApply.Concat(this.CalculateBestDiscount(productsToDiscount, productDiscounts.Except(new[] { productDiscount })));
                }

                if (bestDiscounts.Sum(discount => discount.DiscountPrice) < discountsToApply.Sum(discount => discount.DiscountPrice))
                {
                    bestDiscounts = discountsToApply;
                }
            }

            return bestDiscounts;
        }
    }
}
