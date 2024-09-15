using FluentValidation;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Validation
{
    public class UserValidation : AbstractValidator<UserAddModel>
    {
        public UserValidation()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Name Should Not Be Empty")
                .Matches("^[a-zA-Z ]+$").WithMessage("Name Has Alphabets Only")
                .Length(2, 20).WithMessage("Name length is Between 2 to 20 Characters");

            RuleFor(m => m.EmailAddress)
               .NotEmpty().WithMessage("Email Address Should Not Be Empty")
               .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("Enter Valid Email Address.");
        }
    }
}
