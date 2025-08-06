using AutoMapper;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Application.Services;
using GeciciTSweb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;




var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<GeciciTSwebDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IConsoleService, ConsoleService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ITemporaryMaintenanceTypeService, TemporaryMaintenanceTypeService>();
builder.Services.AddScoped<IUserService, UserService>();



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
builder.Services.AddScoped<ICompanyService, CompanyService>();





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
