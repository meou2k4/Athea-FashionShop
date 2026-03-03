using MimeKit;

namespace FashionShop.Api.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(MimeMessage message);
        Task SendContactEmailAsync(string customerName, string customerEmail, string customerPhone, string message);
    }
}
