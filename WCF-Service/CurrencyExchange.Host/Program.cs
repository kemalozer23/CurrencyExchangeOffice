using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CurrencyExchange.Data;
using CurrencyExchange.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5050");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddScoped<ExchangeService>();

var app = builder.Build();

// Auto-apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<ExchangeService>();
    serviceBuilder.AddServiceEndpoint<ExchangeService, IExchangeService>(
        new BasicHttpBinding { Security = { Mode = CoreWCF.Channels.BasicHttpSecurityMode.None } },
        "/ExchangeService"
    );
});

var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = true;

app.Run();