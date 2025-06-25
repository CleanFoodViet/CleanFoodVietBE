using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs;
using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderPaymentDTOs;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Specifications;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Paginate;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
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
            var result = await repo.GetPagingListAsync(
                spec: spec,
                selector: so => new ServicePackageOrderDTO
                {
                    ServicePackageOrderId = so.ServicePackageOrderId,
                    GardenerId = so.GardenerId,
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

            return result;
        }

        // Get the details of a specific service package order by its ID
        public async Task<ServicePackageOrderDTO> GetOrderDetailAsync(Ulid orderId)
        {
            var repo = _uow.GetRepository<ServicePackageOrder>();

            // Load the order with its payments
            var order = await repo.GetAsync(
                selector: so => new ServicePackageOrderDTO
                {
                    ServicePackageOrderId = so.ServicePackageOrderId,
                    GardenerId = so.GardenerId,
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

            if (order == null)
                throw new Exception("Order not found.");

            return order;
        }

    }
}
