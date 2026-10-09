using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PharmacyBackend.Service
{
    public class RazorpayService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        private readonly string _key;
        private readonly string _secret;

        public RazorpayService(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();

            _key = _config["Razorpay:Key"];
            _secret = _config["Razorpay:Secret"];

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_key}:{_secret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task<string> CreateOrderAsync(decimal amountInINR)
        {
            var payload = new
            {
                amount = (int)(amountInINR * 100), // paise
                currency = "INR",
                receipt = Guid.NewGuid().ToString(),
                payment_capture = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.razorpay.com/v1/orders", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}