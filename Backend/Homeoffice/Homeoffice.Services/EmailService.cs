using Homeoffice.Contracts.Configurations;
using Homeoffice.Contracts.Services;
using Homeoffice.Models.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Homeoffice.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(HomeOfficeEntry homeOfficeEntry)
        {
            var hrEmailAddress = _mailSettings.HrEmailAddress;
            var senderEmail = _mailSettings.SenderEmail;
            var senderName = _mailSettings.SenderName;

            var smtpHost = _mailSettings.SmtpHost;
            var smtpPort = _mailSettings.SmtpPort;
            var smtpUser = _mailSettings.SmtpUser;
            var smtpPass = _mailSettings.SmtpPass;

            var subject = $"Home Office Zeit für {homeOfficeEntry.User.UserName}";
            var body = new TextPart("html")
            {
                Text = $"<h3>Abgeschlossener Homeoffice-Eintrag</h3>" +
                       $"<p><strong>Mitarbeiter:</strong> {homeOfficeEntry.User.UserName}</p>" +
                       $"<p><strong>Start:</strong> {homeOfficeEntry.StartTime:dd.MM.yyyy HH:mm} Uhr</p>" +
                       $"<p><strong>Ende:</strong> {homeOfficeEntry.EndTime:dd.MM.yyyy HH:mm} Uhr</p>" +
                       $"<p><strong>Beschreibung:</strong> {homeOfficeEntry.Description ?? "Keine"}</p>"
            };

            var email = new MimeMessage();
            email.Subject = subject;
            email.From.Add(new MailboxAddress(senderName, senderEmail));
            email.To.Add(MailboxAddress.Parse(hrEmailAddress));
            email.Body = body;
            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(smtpUser, smtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw new InvalidOperationException("Failed to send email notification.", ex);
            }

        }
    }
}
