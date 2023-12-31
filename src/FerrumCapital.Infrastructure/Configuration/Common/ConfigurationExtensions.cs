﻿using FerrumCapital.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Infrastructure.Configuration.Common;

public static class ConfigurationExtensions
{
    public static EntityTypeBuilder<TEntity> ConfigureAuditableBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
       where TEntity : BaseAuditableEntity
    {
        //builder.Property(e => e.CreatedDate).HasDefaultValueSql("Getutcdate()");
        return builder;
    }
}
