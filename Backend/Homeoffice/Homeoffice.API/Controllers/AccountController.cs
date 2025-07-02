using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homeoffice.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AccountController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            _tokenService.DeleteRefreshTokenCookie();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        // POST /api/account/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            var loginResponse = await _authService.LoginAsync(loginRequest);

            if (loginResponse is null)
            {
                // Error: 401 Unauthorized
                return Unauthorized(new { message = "Ungültiger Benutzername oder ungültiges Passwort." });
            }

            return Ok(loginResponse);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            string? newAccessToken = await _authService.RefreshTokenAsync();
            if (string.IsNullOrEmpty(newAccessToken))
            {
                return Unauthorized();
            }

            return Ok(new { token = newAccessToken });
        }

        // GET /api/account/currentuser
        [HttpGet("currentuser")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);

            if (userId is null)
            {
                return Unauthorized();
            }

            return Ok(new { id = userId, username = userName });
        }
    }
}
