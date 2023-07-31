using FerrumCapital.Domain.Entities;
using FerrumCapital.Infrastructure.Configuration.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Infrastructure.Configuration
{
    public class ProductConfiguration /*: IEntityTypeConfiguration<Product>*/
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ConfigureAuditableBaseEntity();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.FilePath).IsRequired();
        }
    }
}
