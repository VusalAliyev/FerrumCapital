using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Infrastructure.Persistance;

public class AppDbContextInitialiser
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AppDbContextInitialiser> _logger;

    public AppDbContextInitialiser(AppDbContext dbContext,
                              ILogger<AppDbContextInitialiser> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async System.Threading.Tasks.Task InitializeAsync()
    {
        try
        {
            // Check if database exist or not
            if (_dbContext.Database.GetDbConnection().State != ConnectionState.Open)
            {
                _dbContext.Database.GetDbConnection().Open();
            }

            bool isExists = await _dbContext.Database.EnsureCreatedAsync();

            if (isExists)
            {
                _logger.LogInformation("MySQL databse is already exits.");
            }
            else
            {
                _logger.LogInformation("MySQL database  have been created.");
            }

             await _dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}
