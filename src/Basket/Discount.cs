namespace FishAndChipShop.Basket
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FishAndChipShop.Basket.Products;

    public readonly struct Discount
    {
        public Discount(Price discountPrice, params IProduct[] products)
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            this.Products = products.ToArray();
            this.DiscountPrice = discountPrice;
        }

        public IEnumerable<IProduct> Products { get; }

        public Price DiscountPrice { get; }

        public static bool operator ==(Discount left, Discount right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Discount left, Discount right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Discount discount)
            {
                return this.Equals(discount);
            }

            return false;
        }

        public bool Equals(Discount other)
        {
            return this.DiscountPrice == other.DiscountPrice && this.Products.SequenceEqual(other.Products);
        }

        public override int GetHashCode()
        {
            return new { this.Products, this.DiscountPrice }.GetHashCode();
        }
    }
}
