using Homeoffice.Contracts.Configurations;
using Homeoffice.Contracts.Constants;
using Homeoffice.Contracts.Services;
using Homeoffice.DataAccess;
using Homeoffice.Models.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Homeoffice.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly SymmetricSecurityKey _key;

        public TokenService(
            IOptions<JwtSettings> jwtSettings,
            ApplicationDbContext context,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
        }

        public string CreateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(TokenConstants.DefaultExpirationSeconds),
                SigningCredentials = creds,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Token creation failed", ex);
            }
        }

        public async Task<string> CreateRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            try
            {
                // Check for existing token
                var existingToken = await _context.Set<ApplicationUserToken>()
                    .FirstOrDefaultAsync(t =>
                        t.UserId == user.Id &&
                        t.LoginProvider == TokenConstants.LocalProvider &&
                        t.Name == TokenConstants.RefreshToken);

                if (existingToken != null)
                {
                    // Update existing token
                    existingToken.Value = refreshToken;
                }
                else
                {
                    // Add new token
                    await _context.Set<ApplicationUserToken>().AddAsync(new ApplicationUserToken
                    {
                        UserId = user.Id,
                        LoginProvider = TokenConstants.LocalProvider,
                        Name = TokenConstants.RefreshToken,
                        Value = refreshToken
                    });
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating refresh token: {ex.Message}");
                throw new InvalidOperationException("Failed to create refresh token", ex);
            }

            return refreshToken;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public void SetRefreshTokenCookie(string refreshToken)
        {
            if (_httpContextAccessor.HttpContext is not null)
            {
                var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = !isDevelopment,
                    SameSite = isDevelopment ? SameSiteMode.Lax : SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(30),
                    Path = "/"
                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append(
                    TokenConstants.RefreshToken,
                    refreshToken,
                    cookieOptions);
            }
            else
            {
                throw new Exception("HttpContext is null");
            }
        }

        public void DeleteRefreshTokenCookie()
        {
            if (_httpContextAccessor.HttpContext is null) return;

            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                TokenConstants.RefreshToken,
                "",
                new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    HttpOnly = true,
                    Secure = !isDevelopment,
                    SameSite = isDevelopment ? SameSiteMode.Lax : SameSiteMode.Strict,
                    Path = "/"
                });
        }
    }
}