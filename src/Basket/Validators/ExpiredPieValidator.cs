namespace FishAndChipShop.Basket.Validators
{
    using System;
    using FishAndChipShop.Basket.Products;
    using FluentValidation.Results;
    using NodaTime;

    public class ExpiredPieValidator : IProductValidator
    {
        private readonly IClock clock;

        public ExpiredPieValidator(IClock clock)
        {
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public ValidationResult Validate(IProduct product)
        {
            if (product is Pie pie && (pie.ExpiryDate < this.clock.GetCurrentInstant().InUtc().Date))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(Pie.ExpiryDate), "Pie has expired.") });
            }

            return new ValidationResult();
        }
    }
}
