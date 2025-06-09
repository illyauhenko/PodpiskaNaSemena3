using Microsoft.EntityFrameworkCore;
using PodpiskaNaSemena.Domain.Entities;


namespace PodpiskaNaSemena3.Infrastructure.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        // Конструктор с передачей параметров
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet для каждой сущности домена
        public DbSet<User> Users { get; set; }
        public DbSet<Seed> Seeds { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Конфигурация моделей
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Применяем конфигурации сущностей
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
