namespace Homeoffice.Contracts.Dtos
{
    public record LoginResponseDto(
        string UserId,
        string Username,
        string Token
    );
}
