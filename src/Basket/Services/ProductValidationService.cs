namespace FishAndChipShop.Basket.Services
{
    using System;
    using System.Collections.Generic;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Validators;
    using FluentValidation.Results;

    public class ProductValidationService : IProductValidationService
    {
        private readonly List<IProductValidator> procoductValidators = new List<IProductValidator>();

        public IReadOnlyList<IProductValidator> Validators => this.procoductValidators.AsReadOnly();

        public void AddValidator(IProductValidator validator)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            this.procoductValidators.Add(validator);
        }

        public ValidationResult Validate(IProduct product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            foreach (IProductValidator productValidator in this.procoductValidators)
            {
                ValidationResult validationResult = productValidator.Validate(product);

                if (!validationResult.IsValid)
                {
                    return validationResult;
                }
            }

            return new ValidationResult();
        }
    }
}
