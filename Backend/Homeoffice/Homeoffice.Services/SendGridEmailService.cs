using Homeoffice.Contracts.Configurations;
using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Homeoffice.Services
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridSettings _sendGridSettings;

        public SendGridEmailService(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
        }

        public async Task SendEmailAsync(HomeOfficeEntry homeOfficeEntry)
        {
            var apiKey = _sendGridSettings.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_sendGridSettings.SenderEmail, _sendGridSettings.FromName);
            var toEmail = new EmailAddress(_sendGridSettings.HrEmailAddress);

            var subject = $"Home Office Zeit für {homeOfficeEntry.User.UserName}";
            
            var body = $"""
            <!DOCTYPE html>
            <html lang="de">
            <head>
                <meta charset="UTF-8">
                <title>{subject}</title>
            </head>
            <body style="font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.6;">
            
                <h3 style="color: #2c3e50;">Abgeschlossener Homeoffice-Eintrag</h3>
                
                <p><strong>Mitarbeiter:</strong> {homeOfficeEntry.User.UserName}</p>
                <p><strong>Start:</strong> {homeOfficeEntry.StartTime:dd.MM.yyyy HH:mm} Uhr</p>
                <p><strong>Ende:</strong> {homeOfficeEntry.EndTime:dd.MM.yyyy HH:mm} Uhr</p>
                <p><strong>Beschreibung:</strong> {homeOfficeEntry.Description ?? "Keine Angabe"}</p>
            
            </body>
            </html>
            """;


            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, "", body);
            msg.HtmlContent = body;

            var response = await client.SendEmailAsync(msg);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"Failed to send email: {response.StatusCode} - {responseBody}");

                throw new InvalidOperationException($"Failed to send email: {response.StatusCode} - {response.Body.ReadAsStringAsync().Result}");
            }
        }
    }
}
