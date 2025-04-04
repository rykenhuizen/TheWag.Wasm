using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TheWag.Wasm;
using TheWag.Wasm.Services;
using TheWag.Wasm.Util;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<SessionStorage>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<AppSettings>();
builder.Services.AddScoped<ProductService>();

await builder.Build().RunAsync();
