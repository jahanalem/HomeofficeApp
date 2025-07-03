using Homeoffice.Contracts.Constants;
using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Homeoffice.DataAccess;
using Homeoffice.Models.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Homeoffice.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            ITokenService tokenService,
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);
            if (user is null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var accessToken = _tokenService.CreateAccessToken(user);
            string refreshToken = null;
            try
            {
                refreshToken = await _tokenService.CreateRefreshTokenAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating refresh token: {ex.Message}");
                return null;
            }

            _tokenService.SetRefreshTokenCookie(refreshToken);

            return new LoginResponseDto(user.Id, user.UserName!, accessToken);
        }

        public async Task<string?> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies[TokenConstants.RefreshToken];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            // Find user by token value
            var tokenEntry = await _context.Set<ApplicationUserToken>()
                .FirstOrDefaultAsync(t =>
                    t.LoginProvider == TokenConstants.LocalProvider &&
                    t.Name == TokenConstants.RefreshToken &&
                    t.Value == refreshToken);

            if (tokenEntry is null)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(tokenEntry.UserId);
            if (user is null)
            {
                return null;
            }

            // Remove old token
            _context.Set<ApplicationUserToken>().Remove(tokenEntry);
            await _context.SaveChangesAsync();

            // Generate new tokens
            var newAccessToken = _tokenService.CreateAccessToken(user);
            var newRefreshToken = await _tokenService.CreateRefreshTokenAsync(user);

            // Update cookie
            _tokenService.SetRefreshTokenCookie(newRefreshToken);

            return newAccessToken;
        }
    }
}
