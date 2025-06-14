using Homeoffice.Models.Entities.Identity;

namespace Homeoffice.Contracts.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
