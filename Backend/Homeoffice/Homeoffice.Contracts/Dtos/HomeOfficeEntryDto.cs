namespace Homeoffice.Contracts.Dtos
{
    public record HomeOfficeEntryDto(int Id,
        DateTimeOffset StartTime,
        DateTimeOffset? EndTime,
        string? Description,
        bool IsEmailSent
    );
}
