using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Infrastructure.Data
{
    internal class GeciciTSwebDbContextFactory : IDesignTimeDbContextFactory<GeciciTSwebDbContext>
    {
        public GeciciTSwebDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeciciTSwebDbContext>();

            // Buraya kendi connection string'ini yaz
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GeciciTamir;Trusted_Connection=True;TrustServerCertificate=True;");

            return new GeciciTSwebDbContext(optionsBuilder.Options);
        }
    }
}
