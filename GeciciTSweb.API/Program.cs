using AutoMapper;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Application.Mapper;
using GeciciTSweb.Application.Services;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Data;
using GeciciTSweb.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using GeciciTSweb.Application.Caching;
using GeciciTSweb.Application.Helpers;

var builder = WebApplication.CreateBuilder(args);



// Render, ngrok veya başka bilgisayar bağlantısı için dış IP’den dinleme
builder.WebHost.UseUrls("http://0.0.0.0:5277", "http://0.0.0.0:7039");


// Database Context
builder.Services.AddDbContext<GeciciTSwebDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers with JSON options
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:7039","http://localhost:5277","http://172.20.10.8:3000/")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Repository Pattern
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Application Services
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IConsoleService, ConsoleService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<ITemporaryMaintenanceTypeService, TemporaryMaintenanceTypeService>();
builder.Services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();
builder.Services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();
builder.Services.AddScoped<IMaintenanceRequestSummaryService, MaintenanceRequestSummaryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICacheManager, MemoryCacheHelper>();


//Memory cache
builder.Services.AddMemoryCache();

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database with test data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GeciciTSwebDbContext>();
        // DataSeeder removed - use /api/test/seed-data endpoint instead
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Test verilerini eklerken bir hata oluştu.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS'u Authorization'dan önce ekle
app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
