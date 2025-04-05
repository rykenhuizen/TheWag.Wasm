using System.Collections;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using TheWag.Models;
using TheWag.Wasm.Util;

namespace TheWag.Wasm.Services
{
    public class ProductService(HttpClient http, AppSettings appSettings, ILogger<ProductService> logger)
    {
        private readonly AppSettings _appSettings = appSettings;
        private readonly HttpClient _http = http;
        private readonly ILogger<ProductService> _logger = logger;

        public Task<ProductDTO[]?> GetAllProducts()
        {
            //var task = Task.Run(() => _http.GetFromJsonAsync<ProductDTO[]>($"{_appSettings.FunctionHostUrl}/api/GetAllProducts"));
            return _http.GetFromJsonAsync<ProductDTO[]>($"{_appSettings.FunctionHostUrl}/api/GetAllProducts");

        }

        public async Task SaveProductAsync(string description, decimal price, int stock, string url, IList<string> tags)
        {
            var product = new ProductDTO()
            {
                Description = description,
                Price = price,
                Stock = stock,
                URL = url,
                Tags = tags.Select(x => new TagDTO() { Text = x }).ToList(),
                Vendor = null
            };
            //save to db
            await _http.PostAsJsonAsync<ProductDTO>($"{_appSettings.FunctionHostUrl}/api/CreateProduct", product);

            //move to product container on blob storage
            var res = await _http.PostAsJsonAsync<string>($"{_appSettings.FunctionHostUrl}/api/MoveTempPic", product.URL);

        }

        public async Task<IList<string>> GetBlobList()
        {
            var blobList = await _http.GetFromJsonAsync<IList<string>>($"{_appSettings.FunctionHostUrl}/api/GetPicList");

            return blobList ?? [];
        }

        public async Task<ImageAnalysisResults> AnalyzeImage(IBrowserFile image, string guidName)
        {

            //TODO: check for duplicate pic

            try
            {
                //resize image to save time and bandwidth, not too worried about quality
                var resizedFile = await image.RequestImageFileAsync("image/png", 300, 500);

                using var ms = resizedFile.OpenReadStream(resizedFile.Size);
                var content = new MultipartFormDataContent
                    {
                        { new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", guidName }
                    };

                //get image analysis
                var analysisResponse = await _http.PostAsync($"{appSettings.FunctionHostUrl}/api/GetAnalysis", content);
                var imageAnalysis = await analysisResponse.Content.ReadFromJsonAsync<ImageAnalysisResults>();

                //save the pic
                var functionToCall = imageAnalysis.IsDog ? "SaveTempPic" : "SaveInvalidPic";
                await _http.PostAsync($"{appSettings.FunctionHostUrl}/api/{functionToCall}", content);
                return imageAnalysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing image");
                return new ImageAnalysisResults
                {
                    Description = "Error analyzing image" + ex.ToString(),
                    Tags = [],
                };
            }
        }
    }
}
