using PodpiskaNaSemena.Application.Models.Review;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using AutoMapper;

using PodpiskaNaSemena.Domain.ValueObjects;

namespace PodpiskaNaSemena.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IUserRepository _userRepo;
        private readonly ISeedRepository _seedRepo;
        private readonly IMapper _mapper;

        public ReviewService(
            IReviewRepository reviewRepo,
            IUserRepository userRepo,
            ISeedRepository seedRepo,
            IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _userRepo = userRepo;
            _seedRepo = seedRepo;
            _mapper = mapper;
        }

        public async Task<ReviewModel> CreateAsync(ReviewCreateModel model, CancellationToken ct = default)
        {
            var user = await _userRepo.GetByIdAsync(model.UserId, ct);
            var seed = await _seedRepo.GetByIdAsync(model.SeedId, ct);

            var review = new Review(
                user.Id,
                seed.Id,
                new Rating(model.Rating),
                model.Comment != null ? new Comment(model.Comment) : null
            );

            await _reviewRepo.AddAsync(review, ct);
            return _mapper.Map<ReviewModel>(review);
        }
    }
}