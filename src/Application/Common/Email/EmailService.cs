using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Application.Common.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _settings = new EmailSettings()
            {
                SmtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new ArgumentNullException("EmailSettings:SmtpServer"),
                SmtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587"),
                SenderName = _configuration["EmailSettings:SenderName"] ?? throw new ArgumentNullException("EmailSettings:SenderName"),
                SenderEmail = _configuration["EmailSettings:SenderEmail"] ?? throw new ArgumentNullException("EmailSettings:SenderEmail"),
                SenderPassword = _configuration["EmailSettings:SenderPassword"] ?? throw new ArgumentNullException("EmailSettings:SenderPassword")
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.SenderPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
