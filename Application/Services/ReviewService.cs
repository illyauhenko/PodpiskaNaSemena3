using PodpiskaNaSemena.Application.Models.Review;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.ValueObjects;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PodpiskaNaSemena.Application.Services.Abstractions;

namespace PodpiskaNaSemena.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IUserRepository _userRepo;
        private readonly ISeedRepository _seedRepo;
        private readonly ISubscriptionRepository _subscriptionRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(
            IReviewRepository reviewRepo,
            IUserRepository userRepo,
            ISeedRepository seedRepo,
            ISubscriptionRepository subscriptionRepo,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _reviewRepo = reviewRepo;
            _userRepo = userRepo;
            _seedRepo = seedRepo;
            _subscriptionRepo = subscriptionRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReviewResponse> CreateReviewAsync(CreateReviewRequest request, CancellationToken ct = default)
        {
            // Проверяем существование пользователя и семян
            var user = await _userRepo.GetByIdAsync(request.UserId, ct)
                ?? throw new EntityNotFoundException("User", request.UserId);
            var seed = await _seedRepo.GetByIdAsync(request.SeedId, ct)
                ?? throw new EntityNotFoundException("Seed", request.SeedId);

            // Проверяем что пользователь имеет активную подписку на эти семена
            var hasActiveSubscription = await _subscriptionRepo.HasActiveSubscriptionAsync(user.Id, seed.Id, ct);
            if (!hasActiveSubscription)
                throw new DomainException("Для оставления отзыва нужна активная подписка на эти семена");

            // Проверяем что пользователь еще не оставлял отзыв на эти семена
            var hasExistingReview = await _reviewRepo.HasUserReviewedSeedAsync(user.Id, seed.Id, ct);
            if (hasExistingReview)
                throw new DomainException("Вы уже оставляли отзыв на эти семена");

            // Создаем отзыв через Value Objects
            var review = new Review(
                user.Id,
                seed.Id,
                new Rating(request.Rating),
                request.Comment != null ? new Comment(request.Comment) : null
            );

            await _reviewRepo.AddAsync(review, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<ReviewResponse> UpdateReviewAsync(int reviewId, CreateReviewRequest request, CancellationToken ct = default)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId, ct)
                ?? throw new EntityNotFoundException("Review", reviewId);

            // Проверяем что пользователь существует
            var user = await _userRepo.GetByIdAsync(request.UserId, ct)
                ?? throw new EntityNotFoundException("User", request.UserId);

            // Проверяем права на редактирование (только автор или админ)
            if (review.UserId != user.Id && !user.IsAdmin)
                throw new DomainException("Недостаточно прав для редактирования этого отзыва");

            // Обновляем отзыв
            review.UpdateReview(
                new Rating(request.Rating),
                request.Comment != null ? new Comment(request.Comment) : null,
                user
            );

            await _reviewRepo.UpdateAsync(review, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task DeleteReviewAsync(int reviewId, CancellationToken ct = default)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId, ct)
                ?? throw new EntityNotFoundException("Review", reviewId);

            // В реальной системе здесь была бы мягкое удаление
            // review.MarkAsDeleted();
            await _reviewRepo.DeleteAsync(review, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<ReviewResponse> GetReviewAsync(int id, CancellationToken ct = default)
        {
            var review = await _reviewRepo.GetByIdAsync(id, ct);
            if (review == null)
                throw new EntityNotFoundException("Review", id);

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<IReadOnlyList<ReviewResponse>> GetReviewsForSeedAsync(int seedId, CancellationToken ct = default)
        {
            var reviews = await _reviewRepo.GetBySeedIdAsync(seedId, ct);
            return _mapper.Map<IReadOnlyList<ReviewResponse>>(reviews);
        }

        public async Task<IReadOnlyList<ReviewResponse>> GetUserReviewsAsync(int userId, CancellationToken ct = default)
        {
            var reviews = await _reviewRepo.GetByUserIdAsync(userId, ct);
            return _mapper.Map<IReadOnlyList<ReviewResponse>>(reviews);
        }

        public async Task<double> GetAverageRatingAsync(int seedId, CancellationToken ct = default)
        {
            return await _reviewRepo.GetAverageRatingAsync(seedId, ct);
        }

        public async Task<bool> HasUserReviewedSeedAsync(int userId, int seedId, CancellationToken ct = default)
        {
            return await _reviewRepo.HasUserReviewedSeedAsync(userId, seedId, ct);
        }
    }
}