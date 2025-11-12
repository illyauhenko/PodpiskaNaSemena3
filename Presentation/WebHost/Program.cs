using Microsoft.EntityFrameworkCore;
using PodpiskaNaSemena.Application.Services;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena.Presentation.WebHost.Configuration;
using PodpiskaNaSemena.Presentation.WebHost.Filters;
using PodpiskaNaSemena.Presentation.WebHost.Middleware;
using PodpiskaNaSemena3.Infrastructure.EntityFramework;
using PodpiskaNaSemena3.Infrastructure.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioningConfiguration();

// Add Application Services
builder.Services.AddApplicationServices();

// Add Infrastructure
builder.Services.AddEntityFramework(builder.Configuration);

// Add Repositories and Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure JSON Options
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Apply migrations in development
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseRequestLogging();
app.UseExceptionHandling();

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }