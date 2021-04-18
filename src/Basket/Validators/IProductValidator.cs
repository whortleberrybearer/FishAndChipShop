namespace FishAndChipShop.Basket.Validators
{
    using FishAndChipShop.Basket.Products;
    using FluentValidation.Results;

    public interface IProductValidator
    {
        ValidationResult Validate(IProduct product);
    }
}
