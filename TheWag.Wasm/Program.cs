using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TheWag.Wasm;
using TheWag.Wasm.Services;
using TheWag.Wasm.Util;
using System.Globalization;
using BlazorApplicationInsights;
using BlazorApplicationInsights.Interfaces;
using BlazorApplicationInsights.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SessionStorage>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<AppSettings>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CheckoutService>();

//set up with fake currency symbol Biscuts
var customCulture = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.InvariantCulture.Clone();
customCulture.NumberFormat.CurrencySymbol = "฿$₭৳";
CultureInfo.DefaultThreadCurrentCulture = customCulture;

builder.Services.AddBlazorApplicationInsights(config =>
{
    config.ConnectionString = "InstrumentationKey=408010ab-ac53-43a6-a68d-8d4cfe8591dd;IngestionEndpoint=https://canadaeast-0.in.applicationinsights.azure.com/;LiveEndpoint=https://canadaeast.livediagnostics.monitor.azure.com/;ApplicationId=f467e1b2-b5b1-47fa-a26d-3bc35f3a1f36";
},
    async applicationInsights =>
    {
    var telemetryItem = new TelemetryItem()
    {
        Tags = new Dictionary<string, object?>()
            {
                { "ai.cloud.role", "SPA" },
                { "ai.cloud.roleInstance", "Blazor Wasm" },
             }
    };
    await applicationInsights.AddTelemetryInitializer(telemetryItem);
});

await builder.Build().RunAsync();
