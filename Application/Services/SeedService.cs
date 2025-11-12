using PodpiskaNaSemena.Application.Models.Seed;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena.Domain.ValueObjects;
using AutoMapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PodpiskaNaSemena.Application.Services.Abstractions;

namespace PodpiskaNaSemena.Application.Services
{
    public class SeedService : ISeedService
    {
        private readonly ISeedRepository _seedRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SeedService(
            ISeedRepository seedRepo,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _seedRepo = seedRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<SeedResponse> CreateSeedAsync(CreateSeedRequest request, CancellationToken ct = default)
        {
            // Создаем семена через конструктор с Value Objects
            var seed = new Seed(0,
                new SeedName(request.Name),
                new Description(request.Description),
                new Price(request.Price));

            await _seedRepo.AddAsync(seed, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<SeedResponse>(seed);
        }

        public async Task<SeedResponse> UpdateSeedAsync(int id, CreateSeedRequest request, CancellationToken ct = default)
        {
            var seed = await _seedRepo.GetByIdAsync(id, ct);
            if (seed == null)
                throw new EntityNotFoundException("Seed", id);

            // В реальности здесь был бы метод Update в Seed entity
            // Пока просто возвращаем обновленные данные
            var updatedSeed = new Seed(id,
                new SeedName(request.Name),
                new Description(request.Description),
                new Price(request.Price));

            await _seedRepo.UpdateAsync(updatedSeed, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<SeedResponse>(updatedSeed);
        }

        public async Task<SeedResponse> GetSeedAsync(int id, CancellationToken ct = default)
        {
            var seed = await _seedRepo.GetByIdAsync(id, ct);
            if (seed == null)
                throw new EntityNotFoundException("Seed", id);

            return _mapper.Map<SeedResponse>(seed);
        }

        public async Task<SeedDetailsResponse> GetSeedDetailsAsync(int id, CancellationToken ct = default)
        {
            var seed = await _seedRepo.GetByIdAsync(id, ct);
            if (seed == null)
                throw new EntityNotFoundException("Seed", id);

            return _mapper.Map<SeedDetailsResponse>(seed);
        }

        public async Task<SeedWithReviewsResponse> GetSeedWithReviewsAsync(int id, CancellationToken ct = default)
        {
            var seed = await _seedRepo.GetByIdAsync(id, ct);
            if (seed == null)
                throw new EntityNotFoundException("Seed", id);

            // Получаем отзывы для этого семени
            var reviews = seed.Reviews.ToList();
            var reviewResponses = _mapper.Map<List<Application.Models.Review.ReviewResponse>>(reviews);

            return new SeedWithReviewsResponse(
                seed.Id,
                seed.Name.Value,
                seed.Description.Value,
                seed.Price.Value,
                seed.CalculateAverageRating(),
                reviewResponses
            );
        }

        public async Task<IReadOnlyList<SeedResponse>> SearchSeedsAsync(string keyword, CancellationToken ct = default)
        {
            var seeds = await _seedRepo.SearchByNameAsync(keyword, ct);
            return _mapper.Map<IReadOnlyList<SeedResponse>>(seeds);
        }

        public async Task<IReadOnlyList<SeedResponse>> GetTopRatedSeedsAsync(int count, CancellationToken ct = default)
        {
            var seeds = await _seedRepo.GetTopRatedAsync(count, ct);
            return _mapper.Map<IReadOnlyList<SeedResponse>>(seeds);
        }

        public async Task<IReadOnlyList<SeedResponse>> GetAvailableSeedsAsync(CancellationToken ct = default)
        {
            var seeds = await _seedRepo.GetAvailableSeedsAsync(ct);
            return _mapper.Map<IReadOnlyList<SeedResponse>>(seeds);
        }
    }
}