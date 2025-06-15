using PodpiskaNaSemena.Application.Models.Seed;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using AutoMapper;

namespace PodpiskaNaSemena.Application.Services
{
    public class SeedService : ISeedService
    {
        private readonly ISeedRepository _seedRepo;
        private readonly IMapper _mapper;

        public SeedService(ISeedRepository seedRepo, IMapper mapper)
        {
            _seedRepo = seedRepo;
            _mapper = mapper;
        }

        public async Task<SeedModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var seed = await _seedRepo.GetByIdAsync(id, ct);
            return _mapper.Map<SeedModel>(seed);
        }
    }
}