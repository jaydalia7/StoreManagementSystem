using FluentValidation;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Validation
{
    public class SellProductValidation : AbstractValidator<SellProductAddModel>
    {
        public SellProductValidation()
        {
            RuleFor(sp => sp.ProductId)
                .NotEmpty().NotNull().WithMessage("ProductIs Should Not Be NUll or Empty");

            RuleFor(sp => sp.Quantity)
                .NotEmpty().NotNull().WithMessage("Quantity Should Not Be Null or Empty");
        }
    }
}
