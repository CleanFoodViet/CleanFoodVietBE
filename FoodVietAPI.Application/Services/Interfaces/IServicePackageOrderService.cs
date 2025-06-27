using CleanFoodVietAPI.Application.DTOs.SubscriptionOrderDTOs;
using CleanFoodVietAPI.Data.Paginate;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe.Checkout;
using Stripe;
using System.Threading.Tasks;

namespace CleanFoodVietAPI.Application.Services.Interfaces
{
    public interface IServicePackageOrderService
    {
        Task<IPaginate<ServicePackageOrderDTO>> GetOrdersAsync(
            int page, int size,
            string? filterField = null, string? filterValue = null,
            string? sortField = null, string? sortOrder = "asc",
            string? search = null);

        Task<ServicePackageOrderDTO> GetOrderDetailAsync(Ulid orderId);

        Ulid CreatePendingOrder(
            Ulid gardenerId,
            Ulid servicePackageId,
            decimal totalAmount);

        // Called when Stripe fires checkout.session.completed
        Task HandleCheckoutSessionCompletedAsync(Stripe.Checkout.Session session);

        // Called when Stripe fires payment_intent.payment_failed
        Task HandlePaymentFailedAsync(PaymentIntent intent);
    }
}
