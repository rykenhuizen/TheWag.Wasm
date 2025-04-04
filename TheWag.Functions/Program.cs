using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWag.Api.WagDB.EF;
using Microsoft.EntityFrameworkCore;
using Util.Azure.ComputerVision;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<ComputerVisionClient>(serviceProvider => new ComputerVisionClient());

builder.Services.AddDbContext<WagDbContext>(options =>
            options.UseSqlServer("Server=tcp:wagsqlserver.database.windows.net,1433;Initial Catalog=WagDB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=120;Authentication='Active Directory Default'",
            options => options.EnableRetryOnFailure())
            );


// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();


builder.Build().Run();
