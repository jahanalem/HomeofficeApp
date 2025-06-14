using System.ComponentModel.DataAnnotations;

namespace Homeoffice.Contracts.Dtos
{
    public record LoginRequestDto(
        [Required] string Username,
        [Required] string Password
    );
}
