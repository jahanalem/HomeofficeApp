using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities;
using Microsoft.Extensions.Configuration;

namespace Homeoffice.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendHomeOfficeCompletionEmailAsync(HomeOfficeEntry homeOfficeEntry)
        {
            var hrEmailAddress = _configuration["MailSettings:HrEmailAddress"];

            var subject = $"Home Office Zeit für {homeOfficeEntry.User.UserName}";
            var body = $"Mitarbeiter {homeOfficeEntry.User.UserName} hat von {homeOfficeEntry.StartTime} bis {homeOfficeEntry.EndTime} gearbeitet.\nBeschreibung: {homeOfficeEntry.Description}";

            throw new NotImplementedException();
        }
    }
}
