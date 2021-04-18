namespace FishAndChipShop.Basket.Services
{
    using FishAndChipShop.Basket.Products;
    using FluentValidation.Results;

    public interface IProductValidationService
    {
        ValidationResult Validate(IProduct product);
    }
}
