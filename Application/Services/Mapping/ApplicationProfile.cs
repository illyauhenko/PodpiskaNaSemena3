using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.ValueObjects;
using AutoMapper;
using PodpiskaNaSemena.Application.Models.User;
using PodpiskaNaSemena.Application.Models.Seed;
using PodpiskaNaSemena.Application.Models.Subscription;
using PodpiskaNaSemena.Application.Models.Payment;
using PodpiskaNaSemena.Application.Models.Review;
using System.Linq;
using System;

namespace PodpiskaNaSemena.Application.Services.Mapping
{
    /// <summary>
    /// Профиль AutoMapper для преобразования между Domain и Application моделями
    /// Определяет правила автоматического маппинга объектов разных слоев
    /// </summary>
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            ConfigureUserMappings();
            ConfigureSeedMappings();
            ConfigureSubscriptionMappings();
            ConfigurePaymentMappings();
            ConfigureReviewMappings();
        }

        /// <summary>
        /// Настройка маппингов для пользователей
        /// Преобразование между Domain.User и Application User моделями
        /// </summary>
        private void ConfigureUserMappings()
        {
            // Domain Entity → DTO Response (для возврата из API)
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            // Domain Entity → Detailed DTO (для админки с статистикой)
            CreateMap<User, UserDetailsResponse>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin))
                .ForMember(dest => dest.ActiveSubscriptionsCount,
                    opt => opt.MapFrom(src => src.Subscriptions.Count(s => s.IsActive(System.DateTime.UtcNow))))
                .ForMember(dest => dest.TotalSubscriptionsCount,
                    opt => opt.MapFrom(src => src.Subscriptions.Count))
                .ForMember(dest => dest.ReviewsCount,
                    opt => opt.MapFrom(src => src.Reviews.Count));

            // Request → Domain Entity (для создания пользователя)
            CreateMap<CreateUserRequest, User>()
                .ConstructUsing(src => new User(
                    0, // ID генерируется в репозитории
                    new Username(src.Username),
                    new Email(src.Email)
                ));
        }

        /// <summary>
        /// Настройка маппингов для семян
        /// Преобразование между Domain.Seed и Application Seed моделями  
        /// </summary>
        private void ConfigureSeedMappings()
        {
            // Domain Entity → DTO Response (для каталога)
            CreateMap<Seed, SeedResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.CalculateAverageRating()));

            // Domain Entity → Detailed DTO (для страницы семян)
            CreateMap<Seed, SeedDetailsResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src => src.CalculateAverageRating()))
                .ForMember(dest => dest.ReviewCount, opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.SubscriptionCount,
                    opt => opt.MapFrom(src => src.Subscriptions.Count(s => s.IsActive(System.DateTime.UtcNow))));

            // Request → Domain Entity (для создания семян админом)
            CreateMap<CreateSeedRequest, Seed>()
                .ConstructUsing(src => new Seed(
                    0, // ID генерируется в репозитории
                    new SeedName(src.Name),
                    new Description(src.Description),
                    new Price(src.Price)
                ));
        }

        /// <summary>
        /// Настройка маппингов для подписок
        /// Преобразование между Domain.Subscription и Application Subscription моделями
        /// </summary>
        private void ConfigureSubscriptionMappings()
        {
            // Domain Entity → DTO Response (для списка подписок)
            CreateMap<Subscription, SubscriptionResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.IsPaid,
                    opt => opt.MapFrom(src => src.Payment != null && src.Payment.Status == Domain.Enums.PaymentStatus.Completed))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive(System.DateTime.UtcNow)));

            // Domain Entity → Detailed DTO (для страницы подписки)
            CreateMap<Subscription, SubscriptionDetailsResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.IsPaid,
                    opt => opt.MapFrom(src => src.Payment != null && src.Payment.Status == Domain.Enums.PaymentStatus.Completed))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive(System.DateTime.UtcNow)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.CalculatePrice().Value))
                .ForMember(dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.Payment != null ? src.Payment.Status.ToString() : "NotCreated"))
                .ForMember(dest => dest.DaysRemaining,
                    opt => opt.MapFrom(src => src.IsActive(System.DateTime.UtcNow) ?
                        (src.EndDate - System.DateTime.UtcNow).Days : 0))
                .ForMember(dest => dest.CanCancel,
                    opt => opt.MapFrom(src => src.Status == Domain.Enums.SubscriptionStatus.Active))
                .ForMember(dest => dest.CanRenew,
                    opt => opt.MapFrom(src => !src.IsActive(System.DateTime.UtcNow)));
        }

        /// <summary>
        /// Настройка маппингов для платежей
        /// Преобразование между Domain.Payment и Application Payment моделями
        /// </summary>
        private void ConfigurePaymentMappings()
        {
            // Domain Entity → DTO Response (для информации о платеже)
            CreateMap<Payment, PaymentResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Value));

            // Domain Entity → Detailed DTO (для админки платежей)
            CreateMap<Payment, PaymentDetailsResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.Value))
                .ForMember(dest => dest.FailureReason, opt => opt.MapFrom(src => src.FailureReason));

            // Request → Domain Entity (для создания платежа)
            CreateMap<CreatePaymentRequest, Payment>()
                .ConstructUsing(src => new Payment(
                    src.SubscriptionId,
                    new Amount(src.Amount),
                    new PaymentMethod(src.PaymentMethod)
                ));
        }

        /// <summary>
        /// Настройка маппингов для отзывов
        /// Преобразование между Domain.Review и Application Review моделями
        /// </summary>
        private void ConfigureReviewMappings()
        {
            // Domain Entity → DTO Response (для отображения отзывов)
            CreateMap<Review, ReviewResponse>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Value))
                .ForMember(dest => dest.Comment,
                    opt => opt.MapFrom(src => src.Comment != null ? src.Comment.Value : null))
                .ForMember(dest => dest.IsEdited, opt => opt.MapFrom(src => src.IsEdited));

            // Request → Domain Entity (для создания отзыва)
            CreateMap<CreateReviewRequest, Review>()
                .ConstructUsing(src => new Review(
                    src.UserId,
                    src.SeedId,
                    new Rating(src.Rating),
                    src.Comment != null ? new Comment(src.Comment) : null
                ));
        }
    }
}