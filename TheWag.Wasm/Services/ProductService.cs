using System.Collections;
using System.Net.Http.Json;
using TheWag.Models;
using TheWag.Wasm.Util;

namespace TheWag.Wasm.Services
{
    public class ProductService(HttpClient http, AppSettings appSettings)
    {
        private readonly AppSettings _appSettings = appSettings;
        private readonly HttpClient _http = http;

        public async Task<ProductDTO[]> GetAllProducts()
        {
            var products = await _http.GetFromJsonAsync<ProductDTO[]>($"{_appSettings.FunctionHostUrl}/api/GetAllProducts");
            return products ?? [];
        }

        public async Task SaveProductAsync(ProductDTO product)
        {
            await _http.PostAsJsonAsync<ProductDTO>($"{appSettings.FunctionHostUrl}/api/CreateProduct", product);
        }
    }
}
