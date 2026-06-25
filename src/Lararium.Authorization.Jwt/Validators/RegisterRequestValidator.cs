using FluentValidation;
using Lararium.Authorization.Jwt.Models.Requests;

namespace Lararium.Authorization.Jwt.Validators;

internal sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private const int MinLoginLength = 3;
    private const int MinPasswordLength = 5;
    private const int MaxFirstNameLength = 50;
    private const int MaxMiddleNameLength = 50;
    private const int MaxLastNameLength = 50;

    public RegisterRequestValidator()
    {
        // Allows letters, numbers, and underscores
        RuleFor(x => x.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("LOGIN_REQUIRED")
            .MinimumLength(MinLoginLength).WithErrorCode("LOGIN_TOO_SHORT")
            .Matches(@"^[a-zA-Z0-9_]+$").WithErrorCode("LOGIN_INVALID_FORMAT"); 

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("PASSWORD_REQUIRED")
            .MinimumLength(MinPasswordLength).WithErrorCode("PASSWORD_TOO_SHORT");

        RuleFor(x => x.PasswordConfirmation)
            .Equal(x => x.Password).WithErrorCode("PASSWORD_CONFIRMATION_MISMATCH");

        RuleFor(x => x.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("LAST_NAME_REQUIRED")
            .MaximumLength(MaxLastNameLength).WithErrorCode("LAST_NAME_TOO_LONG");

        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("FIRST_NAME_REQUIRED")
            .MaximumLength(MaxFirstNameLength).WithErrorCode("FIRST_NAME_TOO_LONG");

        RuleFor(x => x.MiddleName)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(MaxMiddleNameLength).WithErrorCode("MIDDLE_NAME_TOO_LONG");
    }
}