﻿using FerrumCapital.Application.Common.Interfaces;
using FerrumCapital.Infrastructure.Persistance.Interceptors;
using FerrumCapital.Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FerrumCapital.Infrastructure.Services;
using FerrumCapital.Application.Common.Security.Jwt;
using FerrumCapital.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace FerrumCapital.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<AuditableEntitySaveChangesInterceptor>();


            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());
            services.AddScoped<AppDbContextInitialiser>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddTransient<IDateTime, DateTimeService>();
            return services;
        }
    }
}
