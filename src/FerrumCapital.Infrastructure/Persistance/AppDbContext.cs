using FerrumCapital.Application.Common.Interfaces;
using FerrumCapital.Domain.Entities;
using FerrumCapital.Domain.Identity;
using FerrumCapital.Infrastructure.Persistance.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Infrastructure.Persistance
{
    public class AppDbContext:IdentityDbContext<AppUser>,IApplicationDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _interceptor;
        public AppDbContext(
                          DbContextOptions<AppDbContext> options,
                          AuditableEntitySaveChangesInterceptor interceptor)
                          : base(options)
        {
            _interceptor = interceptor;
        }

        public DbSet<Product> Products { get; set; }

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
