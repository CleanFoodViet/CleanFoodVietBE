using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderPaymentDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Implements;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Linq;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Implements
{
    public class ServicePackageOrderService : IServicePackageOrderService
    {
        private readonly IUnitOfWork<CleanFoodVietDbContext> _uow;

        public ServicePackageOrderService(
            IUnitOfWork<CleanFoodVietDbContext> uow)
        {
            _uow = uow;
        }


        // Get a paginated list of service package orders with filtering, sorting, and searching
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

        // Get the details of a specific service package order by its ID
        public async Task<ServicePackageOrderDTO> GetOrderDetailAsync(Ulid orderId)
        {
            var repo = _uow.GetRepository<ServicePackageOrder>();
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

        // Create a new service package order with a PENDING status
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

            // 2) Build the Payment record (PENDING)
            var payment = new ServicePackageOrderPayment
            {
                ServicePackageOrderPaymentId = paymentId,
                ServicePackageOrderId = orderId,
                PaymentAmount = totalAmount,
                Currency = "usd",       // or pull from your package
                Status = "PENDING",
                PaymentDate = now,         // initial timestamp
                PaymentMethod = ""           // fill in once you know it
            };

            // 3) Insert both in your UoW
            var orderRepo = _uow.GetRepository<ServicePackageOrder>();
            var paymentRepo = _uow.GetRepository<ServicePackageOrderPayment>();

            orderRepo.InsertAsync(order);
            paymentRepo.InsertAsync(payment);

            // 4) Commit transaction
            _uow.Commit();

            return orderId;
        }

        // Handle the Stripe Checkout Session completed event
        public async Task HandleCheckoutSessionCompletedAsync(Stripe.Checkout.Session session)
        {
            // 1) Figure out which order this is
            if (!Ulid.TryParse(session.ClientReferenceId, out var orderId))
                return;

            // 2) Load & update Order + Payment
            var orderRepo = _uow.GetRepository<ServicePackageOrder>();
            var paymentRepo = _uow.GetRepository<ServicePackageOrderPayment>();

            var order = await orderRepo.GetAsync(
                selector: o => o,
                predicate: o => o.ServicePackageOrderId == orderId);
            var payment = await paymentRepo.GetAsync(
                selector: p => p,
                predicate: p => p.ServicePackageOrderId == orderId);

            if (order == null || payment == null)
                return;

            // 3) Mark them SUCCESS
            order.Status = "SUCCESS";
            payment.Status = "SUCCESS";
            payment.PaymentDate = DateTime.UtcNow;
            _uow.Commit();

            // 4) Load the package AND its features
            var pkgRepo = _uow.GetRepository<ServicePackage>();
            var pkg = await pkgRepo.GetAsync(
                selector: sp => sp,
                predicate: sp => sp.ServicePackageId == order.ServicePackageId,
                include: sp => sp.Include(x => x.ServicePackageFeatures));

            if (pkg == null)
                return;

            // 5) Create SubscriptionContract
            var now = DateTime.UtcNow;
            var subId = Ulid.NewUlid();

            var contract = new SubscriptionContract
            {
                SubscriptionId = subId,
                GardenerId = order.GardenerId,
                ServicePackageId = order.ServicePackageId,
                DurationInDays = pkg.Duration,
                StartDate = now,
                EndDate = now.AddDays(pkg.Duration),
                Status = "ACTIVE",
                CreatedAt = now
            };

            _uow.GetRepository<SubscriptionContract>().InsertAsync(contract);

            // 6) Use each PackageServiceFeature→ServiceFeature for Benefit rows
            foreach (var pkgFeat in pkg.ServicePackageFeatures)
            {
                var sf = pkgFeat.ServiceFeature;
                var benefit = new SubscriptionContractBenefit
                {
                    SubscriptionContractBenefitId = Ulid.NewUlid(),
                    SubscriptionContractId = subId,
                    BenefitType = sf.ServiceFeatureName, // e.g. "POST_QUOTA"
                    DefaultValue = sf.DefaultValue,
                    RemainingValue = sf.DefaultValue,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                _uow.GetRepository<SubscriptionContractBenefit>().InsertAsync(benefit);
            }

            _uow.Commit();
        }

        // Handle the Stripe Payment Intent failed event
        public async Task HandlePaymentFailedAsync(PaymentIntent intent)
        {
            // 1) Try to read orderId from metadata (you set it on SessionCreateOptions)
            if (!intent.Metadata.TryGetValue("orderId", out var ordStr) ||
                !Ulid.TryParse(ordStr, out var orderId))
            {
                return;
            }

            var orderRepo = _uow.GetRepository<ServicePackageOrder>();
            var paymentRepo = _uow.GetRepository<ServicePackageOrderPayment>();

            var order = await orderRepo.GetAsync(
                selector: o => o,
                predicate: o => o.ServicePackageOrderId == orderId);
            var payment = await paymentRepo.GetAsync(
                selector: p => p,
                predicate: p => p.ServicePackageOrderId == orderId);

            if (order == null || payment == null)
                return;

            order.Status = "FAILED";
            payment.Status = "FAILED";
            payment.PaymentDate = DateTime.UtcNow;
            _uow.Commit();
        }

    }
}
