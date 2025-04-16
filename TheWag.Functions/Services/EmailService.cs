
using Microsoft.Extensions.Logging;
using TheWag.Wasm.Util;
using TheWag.Models;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Text.RegularExpressions;
using System.IO;

namespace TheWag.Functions.Services
{
    public class EmailService(AppSettings appSettings, ILogger<EmailService> logger, BlobService blobService)
    {
        private readonly AppSettings _appSettings = appSettings;
        private readonly ILogger<EmailService> _logger = logger;
        private readonly BlobService _blobService = blobService;

        public async Task Send(int orderId, CustomerCart cart)
        {
            try
            {
                string apiKey = _appSettings.SendGridApiKey;
                var sendGridClient = new SendGridClient(apiKey);
                sendGridClient.UrlPath = "https://api.sendgrid.com/v3/";

                var from = new EmailAddress("noreply@TheWag.cloud", "TheWag.cloud");
                var to = new EmailAddress(cart.Customer.Email);
                var subject = $"Order Confirmation - Order #{orderId}";
                var plainTextContent = Regex.Replace("test", "<[^>]*>", $"Your order #{orderId} has been received.");
                var htmlWriter = new StringWriter();
                htmlWriter.WriteLine("Thanks for shopping at TheWag!<br/>");
                htmlWriter.WriteLine($"Attached are your pictures from order #{orderId}.<br/>");

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlWriter.ToString());
                foreach (var item in cart.Items)
                {
                    msg.AddAttachment(CreateAttachment(item.Product.URL, item.Product.Id.ToString()));
                }
                var response = await sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email for order {orderId}", orderId);
                throw;
            }
            
        }

        private Attachment CreateAttachment(string fileName, string contentId)
        {
            var blogStream = _blobService.GetPicStream(_appSettings.ValidContainerName, fileName);
            var file = Convert.ToBase64String(blogStream.ToArray());
            return new Attachment
            {
                Content = file,
                Type = "application/jpeg",
                Filename = fileName,
                ContentId = contentId
            };
        }
    }
}
