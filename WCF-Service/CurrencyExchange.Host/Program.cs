using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CurrencyExchange.Service;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5050");
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<ExchangeService>();

var app = builder.Build();

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