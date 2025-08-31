using Basket.API.Authentication;
using Basket.API.Endpoints;
using Basket.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Configuration.AddAzureKeyVaultSecrets(connectionName: "key-vault");

var apiKey = builder.Configuration.GetValue<string>("ApiKey");

const string scheme = ApiKeyAuthenticationOptions.DefaultScheme;

builder.Services
    .AddAuthentication(scheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(scheme, o => { o.ApiKey = apiKey; });

builder.Services.AddAuthorization();

builder.Services.AddScoped<BasketService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapDefaultEndpoints();

app.MapBasketEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.RunAsync();