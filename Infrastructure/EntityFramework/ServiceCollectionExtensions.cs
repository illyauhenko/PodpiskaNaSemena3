using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PodpiskaNaSemena3.Infrastructure.EntityFramework
{
    /// <summary>
    /// Методы расширения для регистрации Entity Framework в DI контейнере
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует ApplicationDbContext и настраивает подключение к PostgreSQL
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <returns>Коллекция сервисов для цепочки вызовов</returns>
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Получаем строку подключения из конфигурации
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    //  Включаем повторные попытки при временных ошибках
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);

                    //  Указываем сборку с миграциями
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });

                //  Дополнительные настройки для разработки
#if DEBUG
                options.EnableSensitiveDataLogging();  // Показывать параметры запросов
                options.EnableDetailedErrors();        // Подробные ошибки
#endif
            });

            return services;
        }
    }
}