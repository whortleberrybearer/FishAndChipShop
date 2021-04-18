namespace FishAndChipShop.Basket.Products
{
    using NodaTime;

    public record Pie : IProduct
    {
        public Pie(Price price, LocalDate expiryDate)
        {
            this.Price = price;
            this.ExpiryDate = expiryDate;
        }

        public Price Price { get; }

        public LocalDate ExpiryDate { get; }
    }
}
