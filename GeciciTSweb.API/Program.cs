using AutoMapper;
using GeciciTSweb.Application.DTOs;

using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Application.Mapper;
using GeciciTSweb.Application.Services;
using GeciciTSweb.Application.Services.Interfaces;
using GeciciTSweb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GeciciTSwebDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add services to the container.
// Program.cs içinde



builder.Services.AddControllers();
builder.Services.AddScoped<IConsoleService, ConsoleService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITemporaryMaintenanceTypeService, TemporaryMaintenanceTypeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMaintenanceRequestService, MaintenanceRequestService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IUnitService, UnitService>();


// Program.cs içinde


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<GeciciTSweb.Application.Mapper.MappingProfile>();
});
builder.Services.AddScoped<IConsoleService, ConsoleService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();






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
