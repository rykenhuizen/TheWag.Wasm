using System.Net.Http.Json;
using TheWag.Models;
using TheWag.Wasm.Util;

namespace TheWag.Wasm.Services
{
    public class CheckoutService(HttpClient http, AppSettings appSettings, ILogger<CheckoutService> logger)
    {
        private readonly HttpClient _http = http;
        private readonly AppSettings _appSettings = appSettings;
        private readonly ILogger<CheckoutService> _logger = logger;

        public async Task<int> Checkout(CustomerCart cart)
        {
            var result = _http.PostAsJsonAsync<CustomerCart>($"{_appSettings.FunctionHostUrl}/api/Checkout", cart);
            var res = await result;
            Console.WriteLine(res.ToString());
            return 1;

           

        } 
    }
}
