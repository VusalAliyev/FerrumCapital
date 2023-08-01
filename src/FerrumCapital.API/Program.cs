using FerrumCapital.Application;
using FerrumCapital.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Core;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Configuration for Serilog
Logger log = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt")
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, 
            ValidateIssuer = true, 
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true, 

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            
        };
    });

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        // Add Remote IP Adress to log
        if (httpContext?.Connection?.RemoteIpAddress != null)
        {
            diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress.ToString());
        }
    };
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Downloaded")),
    RequestPath = "/Downloaded"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
