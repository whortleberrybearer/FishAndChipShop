namespace FishAndChipShop.Basket.Tests.Fakes
{
    using Bogus;

    internal class ProductFaker : Faker<FakeProduct>
    {
        internal ProductFaker()
        {
            this.StrictMode(true);
            this.RuleFor(m => m.Price, f => new Price(f.Finance.Amount()));
            this.AssertConfigurationIsValid();
        }
    }
}
