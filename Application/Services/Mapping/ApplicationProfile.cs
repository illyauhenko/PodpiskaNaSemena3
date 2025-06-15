using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.ValueObjects;
using AutoMapper;
using PodpiskaNaSemena.Application.Models.Payment;
using PodpiskaNaSemena.Application.Models.Review;
using PodpiskaNaSemena.Application.Models.Seed;
using PodpiskaNaSemena.Application.Models.Subscription;
using PodpiskaNaSemena.Application.Models.User;

namespace PodpiskaNaSemena.Application.Services.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            // User
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            // Seed
            CreateMap<Seed, SeedModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value));

            // Subscription
            CreateMap<Subscription, SubscriptionModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Payment
            CreateMap<Payment, PaymentModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // Review
            CreateMap<Review, ReviewModel>()
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Value));
        }
    }
}