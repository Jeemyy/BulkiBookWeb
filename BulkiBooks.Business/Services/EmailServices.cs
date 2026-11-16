
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BulkiBook.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BulkiBook.Business.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EmailServices> _logger;

        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailServices(
            HttpClient httpClient,
            IConfiguration configure,
            ILogger<EmailServices> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _apiKey = configure["MailSettings:Apikey"]
                ?? throw new InvalidOperationException("Mailtrap API key is missing.");

            _senderEmail = configure["MailSettings:SenderEmail"]
                ?? throw new InvalidOperationException("Sender email is missing.");

            _senderName = configure["MailSettings:SenderName"]
                ?? throw new InvalidOperationException("Sender name is missing.");
        }

        public async Task<bool> SendEmailAsync(
            string toEmail,
            string subject,
            string htmlContent)
        {
            try
            {
                var emailData = new
                {
                    from = new
                    {
                        email = _senderEmail,
                        name = _senderName
                    },
                    to = new[]
                    {
                        new { email = toEmail }
                    },
                    subject = subject,
                    html = htmlContent
                };

                var jsonContent = JsonSerializer.Serialize(emailData);

                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://send.api.mailtrap.io/api/send");

                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                request.Content = new StringContent(
                    jsonContent,
                    Encoding.UTF8,
                    "application/json");

                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse =
                        await response.Content.ReadAsStringAsync();

                    _logger.LogError(
                        "Mailtrap email failed. Status: {StatusCode}. Response: {Response}",
                        response.StatusCode,
                        errorResponse);

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending email through Mailtrap.");
                return false;
            }
        }

        public async Task<bool> SendOrderEmailConfirmationAsync(
            string toEmail,
            int orderId,
            decimal orderTotal)
        {
            var htmlContent = $@"
                <h1>Thank you for your order!</h1>
                <p>Your order ID is <strong>{orderId}</strong>.</p>
                <p>
                    The total amount is
                    <strong>{orderTotal:N2} EGP</strong>.
                </p>
                <p>Thank you for shopping with BulkiBook!</p>";

            return await SendEmailAsync(
                toEmail,
                "Order Confirmation",
                htmlContent);
        }
    }
}
