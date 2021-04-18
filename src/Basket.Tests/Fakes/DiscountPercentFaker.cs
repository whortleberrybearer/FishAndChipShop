namespace FishAndChipShop.Basket.Tests.Fakes
{
    using Bogus;

    public class DiscountPercentFaker
    {
        private readonly Faker faker = new Faker();

        public DiscountPercent Generate()
        {
            return new DiscountPercent(this.faker.Random.Int(1, 100));
        }
    }
}
