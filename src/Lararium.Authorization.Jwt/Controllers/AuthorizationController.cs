using Asp.Versioning;
using FluentValidation;
using Lararium.Authorization.Jwt.Exceptions;
using Lararium.Authorization.Jwt.Models.Requests;
using Lararium.Authorization.Jwt.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lararium.Authorization.Jwt.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/auth")]
public sealed class AuthorizationController(IJwtIdentityService jwtAuthorizationProvider) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register(RegisterRequest request, [FromServices] IValidator<RegisterRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return GetValidationBadRequest(validationResult);
        }

        try
        {
            var response = await jwtAuthorizationProvider.RegisterUserAsync(request);
            return response;
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request, [FromServices] IValidator<LoginRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return GetValidationBadRequest(validationResult);
        }

        try
        {
            var response = await jwtAuthorizationProvider.LoginUserAsync(request);
            return response;
        }
        catch (UserNotFoundException)
        {
            return StatusCode(
                404,
                new { Code = "LOGIN_USER_NOT_FOUND" });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResponse>> Refresh(
        [FromBody] RefreshTokenRequest request,
        [FromServices] IValidator<RefreshTokenRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return GetValidationBadRequest(validationResult);
        }

        try
        {
            var response = await jwtAuthorizationProvider.RefreshTokenAsync(request);
            return response;
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private BadRequestObjectResult GetValidationBadRequest(FluentValidation.Results.ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(e => new
        {
            Code = e.ErrorCode,
            Field = e.PropertyName
        });
        return BadRequest(new { Errors = errors });
    }
}
