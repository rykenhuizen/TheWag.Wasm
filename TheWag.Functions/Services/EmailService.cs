
using Microsoft.Extensions.Logging;
using TheWag.Wasm.Util;
using TheWag.Models;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.RegularExpressions;

namespace TheWag.Functions.Services
{
    public class EmailService(AppSettings appSettings, ILogger<EmailService> logger)
    {
        private readonly AppSettings _appSettings = appSettings;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task Send(int orderId, CustomerCart cart)
        {
            string? apiKey = _appSettings.SendGridApiKey;
            var sendGridClient = new SendGridClient(apiKey);
            sendGridClient.UrlPath = "https://api.sendgrid.com/v3/";

            var from = new EmailAddress("noreply@TheWag.cloud", "TheWag.cloud");
            var to = new EmailAddress(cart.Customer.Email);
            var subject = $"Order Confirmation - Order #{orderId}";
            var plainTextContent = Regex.Replace("test", "<[^>]*>", $"Your order #{orderId} has been received.");
            var htmlContent = "Thanks for shopping at TheWag!<br/>" +
                              $"Your order #{orderId} has been received.<br/>" +
                              "We will send you an email when your order is shipped.";
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}
