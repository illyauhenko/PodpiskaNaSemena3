using AutoMapper;
using PodpiskaNaSemena.Application.Models.Subscription;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;

namespace PodpiskaNaSemena.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepo;
        private readonly IUserRepository _userRepo;
        private readonly ISeedRepository _seedRepo;
        private readonly IMapper _mapper;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepo,
            IUserRepository userRepo,
            ISeedRepository seedRepo,
            IMapper mapper)
        {
            _subscriptionRepo = subscriptionRepo;
            _userRepo = userRepo;
            _seedRepo = seedRepo;
            _mapper = mapper;
        }

        public async Task<SubscriptionModel> CreateAsync(SubscriptionCreateModel model, CancellationToken ct = default)
        {
            var user = await _userRepo.GetByIdAsync(model.UserId, ct);
            var seed = await _seedRepo.GetByIdAsync(model.SeedId, ct);

            if (user == null || seed == null)
                throw new DomainException("User or seed not found");

            bool hasActiveSub = await _subscriptionRepo.HasActiveSubscriptionAsync(user.Id, seed.Id, ct);
            if (hasActiveSub)
                throw new DomainException("Active subscription already exists");

            var subscription = new Subscription(user.Id, seed.Id, model.StartDate, model.EndDate);
            await _subscriptionRepo.AddAsync(subscription, ct);

            return _mapper.Map<SubscriptionModel>(subscription);
        }

        public async Task CancelAsync(int subscriptionId, CancellationToken ct = default)
        {
            var subscription = await _subscriptionRepo.GetByIdAsync(subscriptionId, ct);
            subscription.Cancel();
            await _subscriptionRepo.UpdateAsync(subscription, ct);
        }
    }
}