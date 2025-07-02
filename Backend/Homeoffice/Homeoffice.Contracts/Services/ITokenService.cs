using Homeoffice.Models.Entities.Identity;

namespace Homeoffice.Contracts.Services
{
    public interface ITokenService
    {
        string CreateAccessToken(User user);
        Task<string> CreateRefreshTokenAsync(User user);
        string GenerateRefreshToken();
        void SetRefreshTokenCookie(string refreshToken);
        void DeleteRefreshTokenCookie();
    }
}
