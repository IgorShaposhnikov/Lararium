using FluentValidation;
using Lararium.Authorization.Jwt.Models.Requests;

namespace Lararium.Authorization.Jwt.Validators;

internal sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    private const int MinRefreshTokenLength = 32;
    private const int MaxRefreshTokenLength = 128;

    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("ACCESS_TOKEN_REQUIRED")
            .Must(predicate: BeValidJwtFormat).WithErrorCode("ACCESS_TOKEN_INVALID_FORMAT")
            .When(x => !string.IsNullOrEmpty(x.AccessToken));

        RuleFor(x => x.RefreshToken)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("REFRESH_TOKEN_REQUIRED")
            .MinimumLength(MinRefreshTokenLength).WithErrorCode("REFRESH_TOKEN_TOO_SHORT")
            .MaximumLength(MaxRefreshTokenLength).WithErrorCode("REFRESH_TOKEN_TOO_LONG");
    }

    private bool BeValidJwtFormat(string? accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return false;

        var parts = accessToken.Split('.');
        return parts.Length == 3 &&
               !string.IsNullOrEmpty(parts[0]) &&
               !string.IsNullOrEmpty(parts[1]) &&
               !string.IsNullOrEmpty(parts[2]);
    }
}
