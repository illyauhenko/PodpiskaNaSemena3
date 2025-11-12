using Microsoft.Extensions.DependencyInjection;
using PodpiskaNaSemena.Application.Services.Abstractions;
using PodpiskaNaSemena.Application.Services.Mapping;

namespace PodpiskaNaSemena.Application.Services
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register Application Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISeedService, SeedService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IReviewService, ReviewService>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(ApplicationProfile));

            return services;
        }
    }
}