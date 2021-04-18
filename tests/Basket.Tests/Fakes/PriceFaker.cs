namespace FishAndChipShop.Basket.Tests.Fakes
{
    using Bogus;

    public class PriceFaker
    {
        private readonly Faker faker = new Faker();

        public Price Generate()
        {
            return new Price(this.faker.Finance.Amount());
        }
    }
}
