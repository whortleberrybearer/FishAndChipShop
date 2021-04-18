namespace FishAndChipShop.Basket
{
    using System;
    using System.Collections.Generic;
    using FishAndChipShop.Basket.Products;
    using FishAndChipShop.Basket.Services;
    using FluentValidation.Results;

    public class Basket
    {
        private readonly List<IProduct> products = new List<IProduct>();
        private readonly IProductValidationService productValidationService;
        private readonly IDiscountService discountService;

        public Basket(IProductValidationService productValidationService, IDiscountService discountService)
        {
            this.productValidationService = productValidationService ?? throw new ArgumentNullException(nameof(productValidationService));
            this.discountService = discountService ?? throw new ArgumentNullException(nameof(discountService));
        }

        public Price TotalCost { get; private set; }

        public IReadOnlyCollection<IProduct> Products => this.products;

        public AddResult Add(IProduct product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            ValidationResult validationResult = this.productValidationService.Validate(product);

            if (!validationResult.IsValid)
            {
                return new AddResult(false);
            }

            this.products.Add(product);

            this.TotalCost = this.discountService.ApplyDiscounts(this.products);

            return AddResult.Succeeded;
        }
    }
}
