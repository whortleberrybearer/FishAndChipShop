namespace FishAndChipShop.Basket.Services
{
    using System.Collections.Generic;
    using FishAndChipShop.Basket.Products;

    public interface IDiscountService
    {
        Price ApplyDiscounts(IEnumerable<IProduct> products);
    }
}
