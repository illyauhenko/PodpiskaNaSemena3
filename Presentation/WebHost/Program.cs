using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PodpiskaNaSemena3.Infrastructure.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Добавляем DbContext с подключением к PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавьте другие сервисы, например, controllers или Razor Pages, если нужно
builder.Services.AddControllers();

var app = builder.Build();

// Вызываем миграции при старте приложения
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Миграции применены успешно.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при применении миграций: {ex.Message}");
        throw;
    }
}

app.MapControllers();

// Конфигурация middleware – к примеру, минимальная настройка, чтобы сервер работал
app.Run();
