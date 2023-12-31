using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using HttpClients.ClientInterface;
using HttpClients.Implementations;
using WebApplication1.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
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
// builder.Services.AddScoped(SP => new HttpClient()); // Registers IHttpClientFactory
builder.Services.AddScoped<DAO>();
builder.Services.AddScoped<ShellyToggle>();
builder.Services.AddScoped<IChargerService, ChargerHttpClient>();
builder.Services.AddScoped<IChargingTaskService, ChargingTaskHttpClient>();
builder.Services.AddScoped<ITruckTypeService, TruckTypeHttpClient>();
builder.Services.AddDbContext<DatabaseContext>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();