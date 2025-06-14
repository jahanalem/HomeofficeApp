using Homeoffice.Contracts.Dtos;

namespace Homeoffice.Contracts.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    }
}
