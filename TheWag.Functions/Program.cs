using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TheWag.Api.WagDB.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Util.Azure.ComputerVision;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<ComputerVisionClient>(serviceProvider => new ComputerVisionClient());

//Server=demo.database.windows.net; Authentication=Active Directory Managed Identity; Database=testdb
builder.Services.AddDbContext<WagDbContext>(options =>
            //options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_Conn"))
            options.UseSqlServer("Server=tcp:wagsqlserver.database.windows.net,1433;Initial Catalog=WagDB;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication='Active Directory Default'")
            );

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();


builder.Build().Run();
