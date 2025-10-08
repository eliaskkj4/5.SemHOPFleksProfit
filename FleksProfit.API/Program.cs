using Microsoft.EntityFrameworkCore;
using FleksProfit.API.Data;
using FleksProfit.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext with SQL Server
builder.Services.AddDbContext<FleksProfitDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=FleksProfitDb;Trusted_Connection=true;MultipleActiveResultSets=true"));

// Add HttpClient for external APIs
builder.Services.AddHttpClient<IEnerginetService, EnerginetService>();
builder.Services.AddHttpClient<IElectricityPriceService, ElectricityPriceService>();

// Add application services
builder.Services.AddScoped<IProfitCalculationService, ProfitCalculationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FleksProfit API",
        Version = "v1",
        Description = "API for calculating profit from offering capacity as a system service (frequency control). " +
                      "Fetches historical system performance data from Energinet and electricity prices to estimate earnings."
    });
});

// Add CORS for Blazor frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FleksProfit API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazor");

app.UseAuthorization();

app.MapControllers();

app.Run();
