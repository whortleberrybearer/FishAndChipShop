namespace FishAndChipShop.Basket.Tests
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Xunit2;
    using Bogus;
    using FishAndChipShop.Basket.Services;
    using FishAndChipShop.Basket.Tests.Fakes;
    using NodaTime;
    using NodaTime.Testing;

    internal class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() => CreateFixture())
        {
        }

        private static IFixture CreateFixture()
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
            Faker faker = new Faker();

            fixture.Register<LocalDate>(() => LocalDate.FromDateTime(faker.Date.Future()));
            fixture.Register<FakeClock>(() => new FakeClock(SystemClock.Instance.GetCurrentInstant()));
            fixture.Register<DiscountPercent>(() => new DiscountPercentFaker().Generate());

            return fixture;
        }
    }
}
