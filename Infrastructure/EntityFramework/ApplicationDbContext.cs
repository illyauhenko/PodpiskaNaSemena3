using Microsoft.EntityFrameworkCore;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Entities.Base;

namespace PodpiskaNaSemena3.Infrastructure.EntityFramework
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet для каждой сущности домена
        public DbSet<User> Users { get; set; }
        public DbSet<Seed> Seeds { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Глобальные фильры (например, только доступные семена)
            modelBuilder.Entity<Seed>().HasQueryFilter(s => s.IsAvailable);

            //  Глобальные соглашения для всех сущностей
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Имя таблицы = имя сущности
                if (entityType.ClrType.IsSubclassOf(typeof(Entity<int>)))
                {
                    entityType.SetTableName(entityType.ClrType.Name);
                }
            }

            //  Применяем конфигурации сущностей
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        // Переопределяем SaveChanges для автоматических полей
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Автоматическая установка дат
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity<int> &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                // Здесь можно добавить логику для автоматических полей
                // Например, установку CreatedAt, UpdatedAt
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        //  Переопределяем для синхронного вызова
        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}