using FluentValidation;
using StoreManagementSystem.Models.ViewModels;

namespace StoreManagementSystem.Validation
{
    public class LoginValidation : AbstractValidator<UserLoginModel>
    {
        public LoginValidation()
        {
            RuleFor(m => m.EmailAddress)
              .NotEmpty().WithMessage("Email Address Should Not Be Empty")
              .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("Enter Valid Email Address.");
        }
    }
}
