using System.Collections;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using TheWag.Models;
using TheWag.Wasm.Util;

namespace TheWag.Wasm.Services
{
    public class ProductService(HttpClient http, AppSettings appSettings)
    {
        private readonly AppSettings _appSettings = appSettings;
        private readonly HttpClient _http = http;

        public Task<ProductDTO[]> GetAllProducts()
        {
            var task = Task.Run(() => _http.GetFromJsonAsync<ProductDTO[]>($"{_appSettings.FunctionHostUrl}/api/GetAllProducts"));
            //var products = _http.GetFromJsonAsync<ProductDTO[]>($"{_appSettings.FunctionHostUrl}/api/GetAllProducts");
            return task;
        }

        public async Task SaveProductAsync(ProductDTO product)
        {
            await _http.PostAsJsonAsync<ProductDTO>($"{_appSettings.FunctionHostUrl}/api/CreateProduct", product);
        }

        public async Task<IList<string>> GetBlobList()
        {
            var blobList = await _http.GetFromJsonAsync<IList<string>>($"{_appSettings.FunctionHostUrl}/api/GetPicList");

            return blobList ?? new List<string>();
        }
    }
}
