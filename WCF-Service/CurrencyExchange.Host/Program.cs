using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using CurrencyExchange.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<HelloService>();
builder.WebHost.UseUrls("http://localhost:5050");

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<HelloService>();
    serviceBuilder.AddServiceEndpoint<HelloService, IHelloService>(
        new BasicHttpBinding { Security = { Mode = CoreWCF.Channels.BasicHttpSecurityMode.None } },
        "/HelloService"
    );
});

var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = true;

app.Run();