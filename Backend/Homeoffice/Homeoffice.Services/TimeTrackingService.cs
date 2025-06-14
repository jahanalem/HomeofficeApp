using Homeoffice.Contracts.Dtos;
using Homeoffice.Contracts.Services;

namespace Homeoffice.Services
{
    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        public TimeTrackingService(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }
        public Task<HomeOfficeEntryDto> StartTrackingAsync(string userId, string? description = null)
        {
            throw new NotImplementedException();
        }
        public Task<HomeOfficeEntryDto> StopTrackingAsync(string userId, string? description = null)
        {
            throw new NotImplementedException();
        }
        public Task<IList<HomeOfficeEntryDto>> GetOverviewAsync(string userId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            throw new NotImplementedException();
        }
    }
}
