using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PodpiskaNaSemena3.Infrastructure.EntityFramework
{
    /// <summary>
    /// Фабрика для создания контекста БД во время разработки
    /// Используется инструментами EF Core для миграций
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Создает экземпляр ApplicationDbContext для миграций
        /// </summary>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Строка подключения для разработки (миграций)
            // В продакшене будет браться из appsettings.json
            var connectionString = "Host=localhost;Port=5432;Database=podpiska;Username=developer;Password=password;";

            optionsBuilder.UseNpgsql(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}