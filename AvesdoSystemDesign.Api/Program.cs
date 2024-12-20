using AvesdoSystemDesign.Api.Configurations;
using AvesdoSystemDesign.Infrastructure.Cache;
using AvesdoSystemDesign.Infrastructure.Db;
using AvesdoSystemDesign.Infrastructure.Services;
using AvesdoSystemDesign.Shared;
using AvesdoSystemDesign.Shared.Logger;
using AvesdoSystemDesign.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var cacheSettings = builder.Configuration.GetSection("CacheSettings");

builder.Services.Configure<CacheSettings>(cacheSettings);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddDbContext<LeadsDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("ReadOnly")));

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ILeadService, LeadService>();

// Add caching services
var cacheProvider = cacheSettings["Provider"];
if (cacheProvider == "Redis")
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = cacheSettings["RedisConnection"];
    });
    builder.Services.AddTransient<ICacheService, RedisCacheService>();
}
else
{
    builder.Services.AddMemoryCache();
    builder.Services.AddTransient<ICacheService, MemoryCacheService>();
}

var app = builder.Build();

app.UseMiddleware<HttpLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();