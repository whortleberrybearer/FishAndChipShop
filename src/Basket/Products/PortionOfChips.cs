namespace FishAndChipShop.Basket.Products
{
    public record PortionOfChips : IProduct
    {
        public PortionOfChips(Price price)
        {
            this.Price = price;
        }

        public Price Price { get; }
    }
}
