using Homeoffice.Models.Entities;

namespace Homeoffice.Contracts.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(HomeOfficeEntry homeOfficeEntry);
    }
}
