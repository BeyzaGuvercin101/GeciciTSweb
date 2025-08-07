using System;
using System.Threading.Tasks;
using GeciciTSweb.Infrastructure.Entities;
using Console = GeciciTSweb.Infrastructure.Entities.Console;

namespace GeciciTSweb.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Companies> Companies { get; }
        IRepository<Console> Consoles { get; }
        IRepository<Department> Departments { get; }
        IRepository<MaintenanceRequest> MaintenanceRequests { get; }
        IRepository<RequestLog> RequestLogs { get; }
        IRepository<Role> Roles { get; }
        IRepository<TemporaryMaintenanceType> TemporaryMaintenanceTypes { get; }
        IRepository<Unit> Units { get; }
        IRepository<User> Users { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
