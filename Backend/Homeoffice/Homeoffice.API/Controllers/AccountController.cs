using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homeoffice.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
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
