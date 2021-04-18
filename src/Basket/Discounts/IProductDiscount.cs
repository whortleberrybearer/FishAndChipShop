namespace FishAndChipShop.Basket.Discounts
{
    using System.Collections.Generic;
    using FishAndChipShop.Basket.Products;

    public interface IProductDiscount
    {
        IEnumerable<Discount> CalculateDiscounts(IEnumerable<IProduct> products);
    }
}
