using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWag.Functions.EF;
using Microsoft.EntityFrameworkCore;
using Util.Azure.ComputerVision;
using TheWag.Wasm.Util;
using TheWag.Functions.Services;
using SendGrid;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<ComputerVisionService>();
builder.Services.AddSingleton<AppSettings>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<BlobService>();


//Scaffold-DbContext "Server=tcp:wagsqlserver.database.windows.net,1433;Initial Catalog=WagDB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=120;Authentication='Active Directory Default'" Microsoft.EntityFrameworkCore.SqlServer -OutputDir EF
builder.Services.AddDbContext<WagDbContext>(options =>
            options.UseSqlServer("Server=tcp:wagsqlserver.database.windows.net,1433;Initial Catalog=WagDB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=120;Authentication='Active Directory Default'",
            options => options.EnableRetryOnFailure())
            );


// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();


builder.Build().Run();
