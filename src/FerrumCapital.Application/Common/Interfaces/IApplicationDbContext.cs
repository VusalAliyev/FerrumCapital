﻿using FerrumCapital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Product> Reminders { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
