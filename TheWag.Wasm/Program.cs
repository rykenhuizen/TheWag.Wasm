using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TheWag.Wasm;
using TheWag.Wasm.Services;
using TheWag.Wasm.Util;
using System.Globalization;
using BlazorApplicationInsights;

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

builder.Services.AddBlazorApplicationInsights();

await builder.Build().RunAsync();
