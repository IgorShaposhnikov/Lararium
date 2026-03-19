using Asp.Versioning;
using Lararium.Authorization.Jwt.Models.Requests;
using Lararium.Authorization.Jwt.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lararium.Authorization.Jwt.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IJwtIdentityService _serivce;

        public AuthorizationController(IJwtIdentityService jwtAuthorizationProvider)
        {
            _serivce = jwtAuthorizationProvider;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(RegisterRequest data)
        {
            try
            {
                var response = await _serivce.RegisterUserAsync(data);
                return response;
            }
            catch(Exception e) 
            {
                return BadRequest(e);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(LoginRequest request)
        {
            try
            {
                var response = await _serivce.LoginUserAsync(request);
                return response;
            }
            catch (Exception e)
            {
                return StatusCode(409, e);
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<RefreshTokenResponse>> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _serivce.RefreshTokenAsync(request);

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
    }
}
