using Homeoffice.Contracts.Dtos;

namespace Homeoffice.Contracts.Services
{
    public interface ITimeTrackingService
    {
        Task<HomeOfficeEntryDto> StartTrackingAsync(string userId, string? description = null);
        Task<HomeOfficeEntryDto> StopTrackingAsync(string userId, string? description = null);
        Task<IList<HomeOfficeEntryDto>> GetOverviewAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
