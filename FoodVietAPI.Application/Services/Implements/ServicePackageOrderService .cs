using AutoMapper;
using CleanFoodVietAPI.Application.DTOs.PaymentHistoryDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderPaymentDTOs;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Enums.ServiceFeatureEnums;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Implements;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System.Linq;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ServicePackageOrderService : BaseService<ServicePackageOrderService>, IServicePackageOrderService
    {

        private static readonly IReadOnlyDictionary<ServiceFeatureActionEnum, int> _coreDefaults
            = new Dictionary<ServiceFeatureActionEnum, int>
            {
                [ServiceFeatureActionEnum.POST_QUOTA] = 20,
                [ServiceFeatureActionEnum.POST_PRIORITY] = 100,
                [ServiceFeatureActionEnum.IMAGE_LIMIT] = 5
            };

        public ServicePackageOrderService(IUnitOfWork<CleanFoodVietDbContext> unitOfWork, ILogger<ServicePackageOrderService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
            : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }

        // Get a paginated list of service package orders with filtering, sorting, and searching
        #region GetOrdersAsync
        public async Task<IPaginate<ServicePackageOrderDTO>> GetOrdersAsync(
            int page, int size,
            string? filterField, string? filterValue,
            string? sortField, string? sortOrder,
            string? search)
        {
            var spec = new ServicePackageOrderSpecification(
                filterField, filterValue, sortField, sortOrder, search);

            var repo = _unitOfWork.GetRepository<ServicePackageOrder>();
            return await repo.GetPagingListAsync(
                spec: spec,
                selector: so => new ServicePackageOrderDTO
                {
                    ServicePackageOrderId = so.ServicePackageOrderId,
                    GardenerId = so.GardenerId,

                    // map gardener info
                    GardenerName = so.Gardener.Name,
                    GardenerEmail = so.Gardener.Email,
                    GardenerPhone = so.Gardener.PhoneNumber,

                    ServicePackageId = so.ServicePackageId,
                    TotalAmount = so.TotalAmount,
                    Status = so.Status,
                    CreatedAt = so.CreatedAt,

                    Payments = so.ServicePackageOrderPayments
                                  .Select(p => new SubscriptionOrderPaymentDTO
                                  {
                                      SubscriptionOrderPaymentId = p.ServicePackageOrderPaymentId,
                                      PaymentAmount = p.PaymentAmount,
                                      Currency = p.Currency,
                                      Status = p.Status,
                                      PaymentDate = p.PaymentDate
                                  })
                                  .ToList()
                },
                page: page,
                size: size);
        }
        #endregion

        // Get the details of a specific service package order by its ID
        #region GetOrderDetailAsync
        public async Task<ServicePackageOrderDTO> GetOrderDetailAsync(Ulid orderId)
        {
            var repo = _unitOfWork.GetRepository<ServicePackageOrder>();
            var dto = await repo.GetAsync(
                selector: so => new ServicePackageOrderDTO
                {
                    ServicePackageOrderId = so.ServicePackageOrderId,
                    GardenerId = so.GardenerId,
                    GardenerName = so.Gardener.Name,
                    GardenerEmail = so.Gardener.Email,
                    GardenerPhone = so.Gardener.PhoneNumber,
                    ServicePackageId = so.ServicePackageId,
                    TotalAmount = so.TotalAmount,
                    Status = so.Status,
                    CreatedAt = so.CreatedAt,
                    Payments = so.ServicePackageOrderPayments
                                  .Select(p => new SubscriptionOrderPaymentDTO
                                  {
                                      SubscriptionOrderPaymentId = p.ServicePackageOrderPaymentId,
                                      PaymentAmount = p.PaymentAmount,
                                      Currency = p.Currency,
                                      Status = p.Status,
                                      PaymentDate = p.PaymentDate
                                  })
                                  .ToList()
                },
                predicate: so => so.ServicePackageOrderId == orderId);

            if (dto == null)
                throw new KeyNotFoundException("Service package order not found.");

            return dto;
        }
        #endregion

        // Get the payment history for a specific gardener
        #region GetPaymentHistoryAsync
        public async Task<IPaginate<PaymentHistoryDTO>> GetPaymentHistoryAsync(string gardenerId, int page, int size)
        {
            // 0) Validate & parse
            if (!Ulid.TryParse(gardenerId, out var gId))
                throw new DomainValidationException($"Invalid Gardener id: '{gardenerId}'.");

            // 1) Query payments joined to orders & package
            var payments = await _unitOfWork.GetRepository<ServicePackageOrderPayment>()
                .GetPagingListAsync(
                    predicate: p => p.ServicePackageOrder.GardenerId == gId,
                    include: q => q
                        .Include(p => p.ServicePackageOrder)
                         .ThenInclude(o => o.ServicePackage),
                    selector: p => new PaymentHistoryDTO
                    {
                        OrderId = p.ServicePackageOrderId.ToString(),
                        PaymentId = p.ServicePackageOrderPaymentId.ToString(),
                        PaymentAmount = p.PaymentAmount,
                        Currency = p.Currency,
                        Status = p.Status,
                        PaymentDate = p.PaymentDate,
                        PaymentMethod = p.PaymentMethod,
                        OrderTotalAmount = p.ServicePackageOrder.TotalAmount,
                        OrderStatus = p.ServicePackageOrder.Status,
                        PackageName = p.ServicePackageOrder.ServicePackage.PackageName
                    },
                    page: page, size: size
                );


            return payments;
        }
        #endregion

        // Create a new service package order with a PENDING status
        #region CreatePendingOrder
        public Ulid CreatePendingOrder(
            Ulid gardenerId,
            Ulid servicePackageId,
            decimal totalAmount)
        {
            var now = DateTime.UtcNow;
            var orderId = Ulid.NewUlid();
            var paymentId = Ulid.NewUlid();

            // 1) Build the Order (PENDING)
            var order = new ServicePackageOrder
            {
                ServicePackageOrderId = orderId,
                GardenerId = gardenerId,
                ServicePackageId = servicePackageId,
                TotalAmount = totalAmount,
                Status = "PENDING",
                CreatedAt = now
            };

            // 2) Build the Payment record (PENDING, CARD)
            var payment = new ServicePackageOrderPayment
            {
                ServicePackageOrderPaymentId = paymentId,
                ServicePackageOrderId = orderId,
                PaymentAmount = totalAmount,
                Currency = "USD",
                Status = "PENDING",
                PaymentDate = now,
                PaymentMethod = "CARD"
            };

            // 3) Insert both in your UoW
            _unitOfWork.GetRepository<ServicePackageOrder>().InsertAsync(order);
            _unitOfWork.GetRepository<ServicePackageOrderPayment>().InsertAsync(payment);

            // 4) Commit transaction
            _unitOfWork.Commit();

            return orderId;
        }
        #endregion

        // Handle the Stripe Checkout Session completed event
        // This method is called when Stripe fires the checkout.session.completed event
        #region HandleCheckoutSessionCompletedAsync
        public async Task HandleCheckoutSessionCompletedAsync(Session session)
        {
            // 1) Parse OrderId
            if (!Ulid.TryParse(session.ClientReferenceId, out var orderId))
                return;

            // ← Only one "now" in the whole method
            var now = DateTime.UtcNow;

            // 2) Load & update Order + Payment → SUCCESS
            var orderRepo = _unitOfWork.GetRepository<ServicePackageOrder>();
            var paymentRepo = _unitOfWork.GetRepository<ServicePackageOrderPayment>();

            var order = await orderRepo.GetAsync(o => o, o => o.ServicePackageOrderId == orderId);
            var payment = await paymentRepo.GetAsync(p => p, p => p.ServicePackageOrderId == orderId);
            if (order == null || payment == null)
                return;

            order.Status = "SUCCESS";
            payment.Status = "SUCCESS";
            payment.PaymentDate = now;
            payment.PaymentMethod = "CARD";
            _unitOfWork.Commit();

            // 3) Load ServicePackage + Features
            var pkg = await _unitOfWork.GetRepository<ServicePackage>()
                .GetAsync(
                    selector: sp => sp,
                    predicate: sp => sp.ServicePackageId == order.ServicePackageId,
                    include: sp => sp
                       .Include(x => x.ServicePackageFeatures)
                       .ThenInclude(psf => psf.ServiceFeature)
                );
            if (pkg == null)
                return;

            // 4) Load & sort existing contracts
            var contractRepo = _unitOfWork.GetRepository<SubscriptionContract>();
            var existing = (await contractRepo.GetListAsync(
                                predicate: c => c.GardenerId == order.GardenerId))
                           .OrderBy(c => c.StartDate)
                           .ToList();

            // 5) Compute start/end and status
            var lastEnd = existing.Select(c => c.EndDate).DefaultIfEmpty(now).Max();
            var hasActive = existing.Any(c =>
                c.Status == "ACTIVE" &&
                c.StartDate <= now &&
                c.EndDate > now);

            var newStatus = hasActive ? "PENDING" : "ACTIVE";
            var startDate = hasActive ? lastEnd : now;
            var endDate = startDate.AddDays(pkg.Duration);

            // 6) Insert the new contract
            var subId = Ulid.NewUlid();
            var contract = new SubscriptionContract
            {
                SubscriptionId = subId,
                GardenerId = order.GardenerId,
                ServicePackageId = order.ServicePackageId,
                DurationInDays = pkg.Duration,
                StartDate = startDate,
                EndDate = endDate,
                Status = newStatus,
                CreatedAt = now
            };

            await contractRepo.InsertAsync(contract);
            await _unitOfWork.CommitAsync();

            // 7) Insert benefits only if ACTIVE
            if (newStatus == "ACTIVE")
            {
                // build a lookup of package-specific feature values
                var pkgFeatures = pkg.ServicePackageFeatures
                    .ToDictionary(
                       psf => Enum.Parse<ServiceFeatureActionEnum>(psf.ServiceFeature.Action),
                       psf => psf.ServiceFeature.DefaultValue);

                var benRepo = _unitOfWork.GetRepository<SubscriptionContractBenefit>();

                foreach (ServiceFeatureActionEnum action in Enum.GetValues<ServiceFeatureActionEnum>())
                {
                    // choose package override or core default
                    var defaultValue = pkgFeatures.TryGetValue(action, out var pkgVal)
                        ? pkgVal
                        : _coreDefaults[action];

                    var ben = new SubscriptionContractBenefit
                    {
                        SubscriptionContractBenefitId = Ulid.NewUlid(),
                        SubscriptionContractId = subId,
                        BenefitType = action.ToString(),
                        DefaultValue = defaultValue,
                        RemainingValue = defaultValue,
                        CreatedAt = now,
                        UpdatedAt = now
                    };

                    await benRepo.InsertAsync(ben);
                }

                await _unitOfWork.CommitAsync();
            }
        }
        #endregion

        // Handle the Stripe Payment Intent failed event
        // This method is called when Stripe fires the payment_intent.payment_failed event
        #region HandlePaymentFailedAsync
        public async Task HandlePaymentFailedAsync(PaymentIntent intent)
        {
            // 1) Read orderId from metadata
            if (!intent.Metadata.TryGetValue("orderId", out var ordStr) ||
                !Ulid.TryParse(ordStr, out var orderId))
            {
                _logger.LogWarning("Could not parse orderId from PaymentIntent.Metadata for Intent {IntentId}",
                    intent.Id);
                return;
            }

            // 2) Load Order + Payment
            var orderRepo = _unitOfWork.GetRepository<ServicePackageOrder>();
            var paymentRepo = _unitOfWork.GetRepository<ServicePackageOrderPayment>();

            var order = await orderRepo.GetAsync(o => o,
                o => o.ServicePackageOrderId == orderId);
            var payment = await paymentRepo.GetAsync(p => p,
                p => p.ServicePackageOrderId == orderId);

            if (order == null || payment == null)
            {
                _logger.LogWarning("Order or payment not found for OrderId {OrderId}", orderId);
                return;
            }

            // 3) Mark both FAILED
            order.Status = "FAILED";
            payment.Status = "FAILED";
            payment.PaymentDate = DateTime.UtcNow;

            // 4) Optionally clean up any queued (PENDING) contracts for this order
            var contractRepo = _unitOfWork.GetRepository<SubscriptionContract>();
            var pendingContracts = (await contractRepo.GetListAsync(
                predicate: c =>
                    c.GardenerId == order.GardenerId
                 && c.ServicePackageId == order.ServicePackageId
                 && c.Status == "PENDING"))
              .ToList();

            foreach (var c in pendingContracts)
                contractRepo.DeleteAsync(c);

            // 5) Commit everything
            await _unitOfWork.CommitAsync();

            _logger.LogWarning(
                "Payment FAILED for order {OrderId}. Cleared {Count} pending contracts.",
                orderId,
                pendingContracts.Count);
        }
        #endregion
    }
}
