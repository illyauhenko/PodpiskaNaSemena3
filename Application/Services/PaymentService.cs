using PodpiskaNaSemena.Application.Models.Payment;
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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly ISubscriptionRepository _subscriptionRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IPaymentRepository paymentRepo,
            ISubscriptionRepository subscriptionRepo,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _paymentRepo = paymentRepo;
            _subscriptionRepo = subscriptionRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(CreatePaymentRequest request, CancellationToken ct = default)
        {
            var subscription = await _subscriptionRepo.GetByIdAsync(request.SubscriptionId, ct)
                ?? throw new EntityNotFoundException("Subscription", request.SubscriptionId);

            // Создаем платеж через Value Objects
            var payment = new Payment(
                subscription.Id,
                new Amount(request.Amount),
                new PaymentMethod(request.PaymentMethod)
            );

            // Связываем платеж с подпиской
            subscription.LinkPayment(payment);

            await _paymentRepo.AddAsync(payment, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(int paymentId, CancellationToken ct = default)
        {
            var payment = await _paymentRepo.GetByIdAsync(paymentId, ct)
                ?? throw new EntityNotFoundException("Payment", paymentId);

            // Имитируем обработку платежа
            payment.MarkAsPaid();

            await _paymentRepo.UpdateAsync(payment, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse> GetPaymentAsync(int id, CancellationToken ct = default)
        {
            var payment = await _paymentRepo.GetByIdAsync(id, ct);
            if (payment == null)
                throw new EntityNotFoundException("Payment", id);

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task MarkPaymentAsPaidAsync(int paymentId, CancellationToken ct = default)
        {
            var payment = await _paymentRepo.GetByIdAsync(paymentId, ct)
                ?? throw new EntityNotFoundException("Payment", paymentId);

            payment.MarkAsPaid();
            await _paymentRepo.UpdateAsync(payment, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task MarkPaymentAsFailedAsync(int paymentId, string reason, CancellationToken ct = default)
        {
            var payment = await _paymentRepo.GetByIdAsync(paymentId, ct)
                ?? throw new EntityNotFoundException("Payment", paymentId);

            payment.MarkAsFailed(reason);
            await _paymentRepo.UpdateAsync(payment, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<PaymentResponse?> GetPaymentBySubscriptionAsync(int subscriptionId, CancellationToken ct = default)
        {
            var payment = await _paymentRepo.GetBySubscriptionIdAsync(subscriptionId, ct);
            return payment != null ? _mapper.Map<PaymentResponse>(payment) : null;
        }

        public async Task<IReadOnlyList<PaymentResponse>> GetPendingPaymentsAsync(CancellationToken ct = default)
        {
            var payments = await _paymentRepo.GetPendingPaymentsAsync(ct);
            return _mapper.Map<IReadOnlyList<PaymentResponse>>(payments);
        }
    }
}