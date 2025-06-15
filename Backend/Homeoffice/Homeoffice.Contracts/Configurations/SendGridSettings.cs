namespace Homeoffice.Contracts.Configurations
{
    public class SendGridSettings
    {
        public string HrEmailAddress { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }
}
