using WebApplication1.Shared;
using WebApplication1.Shared.EnergyPrices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ChargingCalculation>();
builder.Services.AddScoped<EnergyPrices>();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddHttpClient<EnergyPrices>();
builder.Services.AddHttpClient("BypassSSL", client =>
    {
        // Configure client, if needed
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
    });
// builder.Services.AddHttpClient(); // This line registers IHttpClientFactory
builder.Services.AddScoped<ShellyToggle>();
builder.Services.AddScoped<DAO>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();