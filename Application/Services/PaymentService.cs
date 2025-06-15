using PodpiskaNaSemena.Application.Models.Payment;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using AutoMapper;


namespace PodpiskaNaSemena.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly ISubscriptionRepository _subscriptionRepo;
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepo,
            ISubscriptionRepository subscriptionRepo,
            IMapper mapper)
        {
            _paymentRepo = paymentRepo;
            _subscriptionRepo = subscriptionRepo;
            _mapper = mapper;
        }

        public async Task<PaymentModel> ProcessPaymentAsync(PaymentCreateModel model, CancellationToken ct = default)
        {
            var subscription = await _subscriptionRepo.GetByIdAsync(model.SubscriptionId, ct);

            var payment = new Payment(
                subscription.Id,
                model.Amount,
                model.PaymentMethod
            );

            subscription.LinkPayment(payment);
            await _paymentRepo.AddAsync(payment, ct);
            return _mapper.Map<PaymentModel>(payment);
        }
    }
}