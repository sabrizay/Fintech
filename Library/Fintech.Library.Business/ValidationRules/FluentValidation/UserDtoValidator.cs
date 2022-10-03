using FluentValidation;

namespace Fintech.Library.Business.ValidationRules.FluentValidation;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(user => user.MerchantId).GreaterThan(0).WithMessage("Merchant Id cannot be empty");

        RuleFor(user => user.Email).NotEmpty().WithMessage("Email cannot be empty");
        RuleFor(user => user.Email).EmailAddress().WithMessage("Email is not valid");


        RuleFor(user => user.FirstName).NotEmpty();
        RuleFor(user => user.LastName).NotEmpty();
    }
}
