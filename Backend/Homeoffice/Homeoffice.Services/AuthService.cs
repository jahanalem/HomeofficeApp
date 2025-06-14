using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Homeoffice.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
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

            var token = _tokenService.CreateToken(user);

            return new LoginResponseDto(user.Id, user.UserName!, token);
        }
    }
}
