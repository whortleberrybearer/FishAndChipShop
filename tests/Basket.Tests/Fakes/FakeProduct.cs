namespace FishAndChipShop.Basket.Tests.Fakes
{
    using FishAndChipShop.Basket.Products;

    public class FakeProduct : IProduct
    {
        public Price Price { get; init; }
    }
}