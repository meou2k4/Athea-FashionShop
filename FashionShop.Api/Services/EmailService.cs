using FashionShop.Api.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace FashionShop.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailConfig.SenderEmail, _emailConfig.Password);
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

        public async Task SendContactEmailAsync(string customerName, string customerEmail, string customerPhone, string content)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.SenderName, _emailConfig.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("Admin", _emailConfig.SenderEmail)); // Gửi về chính email admin
            emailMessage.Subject = $"Liên hệ mới từ khách hàng: {customerName}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h2>Thông tin khách hàng liên hệ</h2>
                    <p><strong>Họ tên:</strong> {customerName}</p>
                    <p><strong>Email:</strong> {customerEmail}</p>
                    <p><strong>Số điện thoại:</strong> {customerPhone}</p>
                    <p><strong>Nội dung:</strong></p>
                    <p>{content.Replace("\n", "<br/>")}</p>
                    <hr/>
                    <p><i>Email này được gửi tự động từ hệ thống FashionShop.</i></p>"
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();

            await SendEmailAsync(emailMessage);
        }
    }
}
