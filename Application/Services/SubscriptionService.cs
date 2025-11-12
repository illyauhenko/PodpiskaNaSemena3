using AutoMapper;
using PodpiskaNaSemena.Application.Models.Subscription;
using PodpiskaNaSemena.Application.Services.Abstractions;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace PodpiskaNaSemena.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepo;
        private readonly IUserRepository _userRepo;
        private readonly ISeedRepository _seedRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepo,
            IUserRepository userRepo,
            ISeedRepository seedRepo,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _subscriptionRepo = subscriptionRepo;
            _userRepo = userRepo;
            _seedRepo = seedRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request, CancellationToken ct = default)
        {
            // Проверяем существование пользователя и семян
            var user = await _userRepo.GetByIdAsync(request.UserId, ct)
                ?? throw new EntityNotFoundException("User", request.UserId);
            var seed = await _seedRepo.GetByIdAsync(request.SeedId, ct)
                ?? throw new EntityNotFoundException("Seed", request.SeedId);

            // Проверяем нет ли активной подписки
            bool hasActiveSub = await _subscriptionRepo.HasActiveSubscriptionAsync(user.Id, seed.Id, ct);
            if (hasActiveSub)
                throw new DomainException("Активная подписка уже существует");

            // Создаем подписку через доменные методы
            var subscription = request.SubscriptionType switch
            {
                "Monthly" => Subscription.CreateMonthlySubscription(seed.Id, DateTime.UtcNow),
                "Quarterly" => Subscription.CreateQuarterlySubscription(seed.Id, DateTime.UtcNow),
                "Yearly" => Subscription.CreateYearlySubscription(seed.Id, DateTime.UtcNow),
                _ => throw new DomainException("Неизвестный тип подписки")
            };

            // Добавляем пользователя к подписке (многие-ко-многим)
            subscription.AddUser(user);

            await _subscriptionRepo.AddAsync(subscription, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<SubscriptionResponse>(subscription);
        }

        public async Task CancelSubscriptionAsync(int subscriptionId, CancellationToken ct = default)
        {
            var subscription = await _subscriptionRepo.GetByIdAsync(subscriptionId, ct);
            if (subscription == null)
                throw new EntityNotFoundException("Subscription", subscriptionId);

            subscription.Cancel();
            await _subscriptionRepo.UpdateAsync(subscription, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<SubscriptionDetailsResponse> GetSubscriptionAsync(int id, CancellationToken ct = default)
        {
            var subscription = await _subscriptionRepo.GetByIdAsync(id, ct);
            if (subscription == null)
                throw new EntityNotFoundException("Subscription", id);

            return _mapper.Map<SubscriptionDetailsResponse>(subscription);
        }

        public async Task<IReadOnlyList<SubscriptionResponse>> GetUserSubscriptionsAsync(int userId, CancellationToken ct = default)
        {
            var subscriptions = await _subscriptionRepo.GetActiveSubscriptionsForUserAsync(userId, ct);
            return _mapper.Map<IReadOnlyList<SubscriptionResponse>>(subscriptions);
        }

        public async Task<IReadOnlyList<SubscriptionResponse>> GetActiveSubscriptionsAsync(CancellationToken ct = default)
        {
            var subscriptions = await _subscriptionRepo.GetSubscriptionsByStatusAsync(Domain.Enums.SubscriptionStatus.Active, ct);
            return _mapper.Map<IReadOnlyList<SubscriptionResponse>>(subscriptions);
        }

        public async Task<bool> HasActiveSubscriptionAsync(int userId, int seedId, CancellationToken ct = default)
        {
            return await _subscriptionRepo.HasActiveSubscriptionAsync(userId, seedId, ct);
        }
    }
}