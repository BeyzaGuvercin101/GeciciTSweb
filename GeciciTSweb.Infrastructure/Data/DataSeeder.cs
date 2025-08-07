using GeciciTSweb.Domain.Enums;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GeciciTSweb.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GeciciTSwebDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<GeciciTSwebDbContext>>();

            try
            {
                // Ensure database is created
                await context.Database.EnsureCreatedAsync();

                // Check if data already exists
                if (await context.Companies.AnyAsync())
                {
                    logger.LogInformation("Database already seeded.");
                    return;
                }

                logger.LogInformation("Seeding database...");

                // 1. Companies
                var companies = new[]
                {
                    new Companies { Name = "Star Rafineri" },
                    new Companies { Name = "Petkim" },
                    new Companies { Name = "TÜPRAŞ" }
                };
                await context.Companies.AddRangeAsync(companies);
                await context.SaveChangesAsync();

                // 2. Departments
                var departments = new[]
                {
                    new Department { Name = "Teknik" },
                    new Department { Name = "Operasyon" },
                    new Department { Name = "Bakım" }
                };
                await context.Departments.AddRangeAsync(departments);
                await context.SaveChangesAsync();

                // 3. Roles
                var roles = new[]
                {
                    new Role { Name = "Personel" },
                    new Role { Name = "Mühendis" },
                    new Role { Name = "Müdür" },
                    new Role { Name = "Direktör" },
                    new Role { Name = "Admin" }
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();

                // 4. TemporaryMaintenanceTypes
                var maintenanceTypes = new[]
                {
                    new TemporaryMaintenanceType { Name = "Acil Bakım" },
                    new TemporaryMaintenanceType { Name = "Periyodik Bakım" },
                    new TemporaryMaintenanceType { Name = "Önleyici Bakım" },
                    new TemporaryMaintenanceType { Name = "Onarım" }
                };
                await context.TemporaryMaintenanceTypes.AddRangeAsync(maintenanceTypes);
                await context.SaveChangesAsync();

                // 5. Consoles
                var consoles = new[]
                {
                    new Infrastructure.Entities.Console { Name = "Ana Konsol", CompanyId = companies[0].Id },
                    new Infrastructure.Entities.Console { Name = "Yardımcı Konsol", CompanyId = companies[0].Id },
                    new Infrastructure.Entities.Console { Name = "Petkim Ana Konsol", CompanyId = companies[1].Id }
                };
                await context.Consoles.AddRangeAsync(consoles);
                await context.SaveChangesAsync();

                // 6. Units
                var units = new[]
                {
                    new Unit { Name = "Ünite A", ConsoleId = consoles[0].Id },
                    new Unit { Name = "Ünite B", ConsoleId = consoles[0].Id },
                    new Unit { Name = "Ünite C", ConsoleId = consoles[1].Id },
                    new Unit { Name = "Petkim Ünite 1", ConsoleId = consoles[2].Id }
                };
                await context.Units.AddRangeAsync(units);
                await context.SaveChangesAsync();

                // 7. Users
                var users = new[]
                {
                    new User 
                    { 
                        KeycloakUserId = "user-123", 
                        DepartmentId = departments[0].Id, 
                        RoleId = roles[1].Id // Mühendis
                    },
                    new User 
                    { 
                        KeycloakUserId = "user-456", 
                        DepartmentId = departments[1].Id, 
                        RoleId = roles[0].Id // Personel
                    },
                    new User 
                    { 
                        KeycloakUserId = "admin-789", 
                        DepartmentId = departments[0].Id, 
                        RoleId = roles[4].Id // Admin
                    }
                };
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();

                // 8. MaintenanceRequests
                var maintenanceRequests = new[]
                {
                    new MaintenanceRequest
                    {
                        UnitId = units[0].Id,
                        BildirimNumarasi = "BLD-2024-001",
                        EquipmentNumber = "EQ-123",
                        TempMaintenanceTypeId = maintenanceTypes[0].Id,
                        Temperature = 45.2m,
                        Pressure = 1.2m,
                        Fluid = "Su",
                        Status = MaintenanceWorkflowStatus.YeniTalep.ToString(),
                        IsClosed = false,
                        CreatedByUserId = users[0].Id,
                        CreatedAt = DateTime.Now
                    },
                    new MaintenanceRequest
                    {
                        UnitId = units[1].Id,
                        BildirimNumarasi = "BLD-2024-002",
                        EquipmentNumber = "EQ-124",
                        TempMaintenanceTypeId = maintenanceTypes[1].Id,
                        Temperature = 55.8m,
                        Pressure = 2.1m,
                        Fluid = "Yağ",
                        Status = MaintenanceWorkflowStatus.Onaylandi.ToString(),
                        IsClosed = true,
                        CreatedByUserId = users[1].Id,
                        CreatedAt = DateTime.Now.AddDays(-2)
                    },
                    new MaintenanceRequest
                    {
                        UnitId = units[2].Id,
                        BildirimNumarasi = "BLD-2024-003",
                        EquipmentNumber = "EQ-125",
                        TempMaintenanceTypeId = maintenanceTypes[2].Id,
                        Temperature = 32.5m,
                        Pressure = 0.8m,
                        Fluid = "Hava",
                        Status = MaintenanceWorkflowStatus.GeriGonderildi.ToString(),
                        IsClosed = false,
                        CreatedByUserId = users[0].Id,
                        CreatedAt = DateTime.Now.AddDays(-1)
                    }
                };
                await context.MaintenanceRequests.AddRangeAsync(maintenanceRequests);
                await context.SaveChangesAsync();

                // 9. RequestLogs
                var requestLogs = new[]
                {
                    new RequestLog
                    {
                        MaintenanceRequestId = maintenanceRequests[0].Id,
                        AuthorUserId = users[0].Id,
                        LogType = "Onaylandi",
                        Comment = "Uygun bulundu, işleme alındı.",
                        CreatedAt = DateTime.Now
                    },
                    new RequestLog
                    {
                        MaintenanceRequestId = maintenanceRequests[2].Id,
                        AuthorUserId = users[2].Id,
                        LogType = "GeriGonderildi",
                        Comment = "Eksik bilgi var, basınç değeri kontrol edilmeli.",
                        Reason = "Basınç değeri girilmemiş",
                        CreatedAt = DateTime.Now.AddMinutes(-30)
                    }
                };
                await context.RequestLogs.AddRangeAsync(requestLogs);
                await context.SaveChangesAsync();

                logger.LogInformation("Database seeded successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}
