using FluentValidation;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Validation
{
    public class ProductValidation : AbstractValidator<ProductAddModel>
    {
        public ProductValidation()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Product Name Should Not Be Empty")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Invalid Product Name")
                .Length(2, 20).WithMessage("Product Name length is Between 2 to 20 Characters");

            RuleFor(m => m.Price)
                .NotEmpty().WithMessage("Product Price Should Not Be 0");
        }
    }
}
