using System;
using System.Threading.Tasks;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Data;
using GeciciTSweb.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Console = GeciciTSweb.Infrastructure.Entities.Console;

namespace GeciciTSweb.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GeciciTSwebDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(GeciciTSwebDbContext context)
        {
            _context = context;
            
            Companies = new Repository<Companies>(_context);
            Consoles = new Repository<Console>(_context);
            Departments = new Repository<Department>(_context);
            MaintenanceRequests = new Repository<MaintenanceRequest>(_context);
            RequestLogs = new Repository<RequestLog>(_context);
            Roles = new Repository<Role>(_context);
            TemporaryMaintenanceTypes = new Repository<TemporaryMaintenanceType>(_context);
            Units = new Repository<Unit>(_context);
            Users = new Repository<User>(_context);
        }

        public IRepository<Companies> Companies { get; }
        public IRepository<Console> Consoles { get; }
        public IRepository<Department> Departments { get; }
        public IRepository<MaintenanceRequest> MaintenanceRequests { get; }
        public IRepository<RequestLog> RequestLogs { get; }
        public IRepository<Role> Roles { get; }
        public IRepository<TemporaryMaintenanceType> TemporaryMaintenanceTypes { get; }
        public IRepository<Unit> Units { get; }
        public IRepository<User> Users { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
