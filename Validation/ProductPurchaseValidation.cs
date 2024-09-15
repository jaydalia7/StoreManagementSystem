using FluentValidation;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Validation
{
    public class ProductPurchaseValidation : AbstractValidator<PurchaseProductAddModel>
    {
        public ProductPurchaseValidation()
        {
            RuleFor(pp => pp.ProductId)
                .NotEmpty().NotNull().WithMessage("ProductId Should Not Be Null or Empty");

            RuleFor(pp => pp.Quantity)
                .NotEmpty().NotNull().WithMessage("Quantity Should Not Be Null or Empty");
        }
    }
}
