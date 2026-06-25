using FluentValidation;
using Lararium.Authorization.Jwt.Models.Requests;

namespace Lararium.Authorization.Jwt.Validators;

internal sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    private const int MinLoginLength = 3;

    public LoginRequestValidator()
    {
        RuleFor(x => x.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("LOGIN_REQUIRED")
            .MinimumLength(MinLoginLength).WithErrorCode("LOGIN_TOO_SHORT");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("PASSWORD_REQUIRED");
    }
}
