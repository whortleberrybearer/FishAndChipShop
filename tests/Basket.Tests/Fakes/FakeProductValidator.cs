namespace FishAndChipShop.Basket.Tests.Fakes
{
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Validators;
    using FluentValidation.Results;

    public class FakeProductValidator : IProductValidator
    {
        public ValidationResult Validate(IProduct product)
        {
            throw new System.NotImplementedException();
        }
    }
}
