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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System.Linq;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ServicePackageOrderService : IServicePackageOrderService
    {

        private static readonly IReadOnlyDictionary<ServiceFeatureActionEnum, int> _coreDefaults
            = new Dictionary<ServiceFeatureActionEnum, int>
            {
                [ServiceFeatureActionEnum.POST_QUOTA] = 20,
                [ServiceFeatureActionEnum.POST_PRIORITY] = 100,
                [ServiceFeatureActionEnum.IMAGE_LIMIT] = 5
            };

        private readonly IUnitOfWork<CleanFoodVietDbContext> _uow;
        private readonly ILogger<ServicePackageOrderService> _logger;

        public ServicePackageOrderService(
            IUnitOfWork<CleanFoodVietDbContext> uow,
            ILogger<ServicePackageOrderService> logger)
        {
            _uow = uow;
            _logger = logger;
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

            var repo = _uow.GetRepository<ServicePackageOrder>();
            return await repo.GetPagingListAsync(
                spec: spec,
                include: so => so.Include(x => x.ServicePackage),
                selector: so => new ServicePackageOrderDTO
                {
                    ServicePackageOrderId = so.ServicePackageOrderId,
                    GardenerId = so.GardenerId,

                    // map gardener info
                    GardenerName = so.Gardener.Name,
                    GardenerEmail = so.Gardener.Email,
                    GardenerPhone = so.Gardener.PhoneNumber,

                    ServicePackageId = so.ServicePackageId,
                    ServicePackageName = so.ServicePackage.PackageName,
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
            var repo = _uow.GetRepository<ServicePackageOrder>();
            var dto = await repo.GetAsync(
                include: so => so.Include(x => x.ServicePackage),
                selector: so => new ServicePackageOrderDTO
                {
                    ServicePackageOrderId = so.ServicePackageOrderId,
                    GardenerId = so.GardenerId,
                    GardenerName = so.Gardener.Name,
                    GardenerEmail = so.Gardener.Email,
                    GardenerPhone = so.Gardener.PhoneNumber,
                    ServicePackageId = so.ServicePackageId,
                    ServicePackageName = so.ServicePackage.PackageName,
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
            var payments = await _uow.GetRepository<ServicePackageOrderPayment>()
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
                Currency = "VND",
                Status = "PENDING",
                PaymentDate = now,
                PaymentMethod = "CARD"
            };

            // 3) Insert both in your UoW
            _uow.GetRepository<ServicePackageOrder>().InsertAsync(order);
            _uow.GetRepository<ServicePackageOrderPayment>().InsertAsync(payment);

            // 4) Commit transaction
            _uow.Commit();

            return orderId;
        }
        #endregion

        // Handle the Stripe Checkout Session completed event
        // This method is called when Stripe fires the checkout.session.completed event
        #region HandleCheckoutSessionCompletedAsync
        public async Task HandleCheckoutSessionCompletedAsync(Session session)
        {
            // 1) Parse the OrderId
            if (!Ulid.TryParse(session.ClientReferenceId, out var orderId))
                return;

            var now = DateTime.UtcNow;
            var orderRepo = _uow.GetRepository<ServicePackageOrder>();
            var paymentRepo = _uow.GetRepository<ServicePackageOrderPayment>();

            // 2) Load Order & Payment (default tracking or not, we'll still call UpdateAsync)
            var order = await orderRepo.GetAsync(
                selector: o => o,
                predicate: o => o.ServicePackageOrderId == orderId);

            var payment = await paymentRepo.GetAsync(
                selector: p => p,
                predicate: p => p.ServicePackageOrderId == orderId);

            if (order == null || payment == null)
                return;

            // 2a) Idempotency check
            if (order.Status == "SUCCESS")
                return;

            // 3) Mutate both entities
            order.Status = "SUCCESS";
            payment.Status = "SUCCESS";
            payment.PaymentDate = now;
            payment.PaymentMethod = "CARD";

            // 4) Tell the repos to update
            orderRepo.UpdateAsync(order);
            paymentRepo.UpdateAsync(payment);

            // 5) Commit asynchronously so the UPDATE statements run
            await _uow.CommitAsync();

            // 6) Now continue with contract creation…
            var pkg = await _uow.GetRepository<ServicePackage>()
                .GetAsync(
                    selector: sp => sp,
                    predicate: sp => sp.ServicePackageId == order.ServicePackageId,
                    include: q => q
                        .Include(sp => sp.ServicePackageFeatures)
                         .ThenInclude(psf => psf.ServiceFeature)
                );

            if (pkg == null)
                return;

            var contractRepo = _uow.GetRepository<SubscriptionContract>();
            var existing = (await contractRepo.GetListAsync(
                                   predicate: c => c.GardenerId == order.GardenerId))
                              .OrderBy(c => c.StartDate)
                              .ToList();

            var lastEnd = existing.Select(c => c.EndDate).DefaultIfEmpty(now).Max();
            var hasActive = existing.Any(c =>
                c.Status == "ACTIVE" &&
                c.StartDate <= now &&
                c.EndDate > now);

            var newStatus = hasActive ? "PENDING" : "ACTIVE";
            var startDate = hasActive ? lastEnd : now;
            var endDate = startDate.AddDays(pkg.Duration);

            var subId = Ulid.NewUlid();
            var contract = new SubscriptionContract
            {
                SubscriptionId = subId,
                GardenerId = order.GardenerId,
                ServicePackageId = order.ServicePackageId,
                DurationInDays = pkg.Duration,
                SubscriptionType = "CUSTOM",
                StartDate = startDate,
                EndDate = endDate,
                Status = newStatus,
                CreatedAt = now
            };

            await contractRepo.InsertAsync(contract);
            await _uow.CommitAsync();

            if (newStatus == "ACTIVE")
            {
                var pkgFeatures = pkg.ServicePackageFeatures
                    .ToDictionary(
                        psf => Enum.Parse<ServiceFeatureActionEnum>(psf.ServiceFeature.Action),
                        psf => psf.ServiceFeature.DefaultValue
                    );

                var benRepo = _uow.GetRepository<SubscriptionContractBenefit>();

                foreach (ServiceFeatureActionEnum action in Enum.GetValues<ServiceFeatureActionEnum>())
                {
                    var defaultVal = pkgFeatures.TryGetValue(action, out var val)
                                     ? val
                                     : _coreDefaults[action];

                    var ben = new SubscriptionContractBenefit
                    {
                        SubscriptionContractBenefitId = Ulid.NewUlid(),
                        SubscriptionContractId = subId,
                        BenefitType = action.ToString(),
                        DefaultValue = defaultVal,
                        RemainingValue = defaultVal,
                        CreatedAt = now,
                        UpdatedAt = now
                    };

                    await benRepo.InsertAsync(ben);
                }

                await _uow.CommitAsync();
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
            var orderRepo = _uow.GetRepository<ServicePackageOrder>();
            var paymentRepo = _uow.GetRepository<ServicePackageOrderPayment>();

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
            var contractRepo = _uow.GetRepository<SubscriptionContract>();
            var pendingContracts = (await contractRepo.GetListAsync(
                predicate: c =>
                    c.GardenerId == order.GardenerId
                 && c.ServicePackageId == order.ServicePackageId
                 && c.Status == "PENDING"))
              .ToList();

            foreach (var c in pendingContracts)
                contractRepo.DeleteAsync(c);

            // 5) Commit everything
            await _uow.CommitAsync();

            _logger.LogWarning(
                "Payment FAILED for order {OrderId}. Cleared {Count} pending contracts.",
                orderId,
                pendingContracts.Count);
        }
        #endregion
    }
}