using FerrumCapital.Application.Common.Interfaces;
using FerrumCapital.Domain.Entities;
using FerrumCapital.Infrastructure.Persistance.Interceptors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Infrastructure.Persistance
{
    public class AppDbContext:DbContext,IApplicationDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _interceptor;
        public AppDbContext(
                          DbContextOptions<AppDbContext> options,
                          AuditableEntitySaveChangesInterceptor interceptor)
                          : base(options)
        {
            _interceptor = interceptor;
        }

        public DbSet<Product> Reminders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_interceptor);

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
