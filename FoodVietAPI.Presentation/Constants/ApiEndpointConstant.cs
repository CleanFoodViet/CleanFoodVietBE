using System.Runtime.CompilerServices;

namespace CleanFoodVietAPI.Presentation.Constants
{
    public static class ApiEndpointConstant
    {
        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public const string AdminApiEndpoint = ApiEndpoint + "/admin";
        public const string GardenerApiEndpoint = ApiEndpoint + "/gardener";
        public const string RetailerApiEndpoint = ApiEndpoint + "/retailer";

        public static class Authentication
        {
            public const string AuthEndpoint = ApiEndpoint + "/auth";
            public const string LoginEndpoint = ApiEndpoint + "/login";
            public const string RegisterEndpoint = ApiEndpoint + "/register";
            public const string GardenerRegisterEndpoint = ApiEndpoint + "/gardener-register";
            public const string AdminRegisterEndpoint = ApiEndpoint + "/admin-register";
        }

        public static class Account
        {
            //Account endpoint
            public const string AccountsEndpoint = ApiEndpoint + "/accounts";
            public const string AccountEndpoint = AccountsEndpoint + "/{id}";
            public const string AccountStatuEndpoint = AccountEndpoint + "/status";

            public const string RetailerAccountsEndpoint = AccountsEndpoint + "/retailers";
            public const string GardenerAccountsEndpoint = AccountsEndpoint + "/gardeners";
            public const string AccountProfileEndpoint = AccountEndpoint + "/profile";
            public const string AccountPasswordEndpoint = AccountsEndpoint + "/password";

            //Account address endpoints
            public const string AccountAddressesEndpoint = AccountEndpoint + "/addresses";

            //Appointment endpoints
            public const string AccountAppointmentEndpoint = AccountEndpoint + "/appointments";
            public const string ScheduledAppointmentEndpoint = AccountAppointmentEndpoint + "/scheduled";
            public const string RequestAppointmentEndpoint = AccountAppointmentEndpoint + "/requested";

            //Notification endpoints
            public const string AccountNotificationEndpoint = AccountEndpoint + "/notifications";

            //Order endpoint
            public const string AccountOrdersEndpoint = AccountEndpoint + "/orders";
            public const string AccountOrderEndpoint = AccountOrdersEndpoint + "/{orderId}";
        }

        public static class Address
        {
            public const string AddressesEndpoint = ApiEndpoint + "/addresses";
            public const string AddressEndpoint = AddressesEndpoint + "/{id}";
        }

        public static class Appointment
        {
            public const string AppointmentsEndpoint = ApiEndpoint + "/appointments";
            public const string AppointmentEndpoint = AppointmentsEndpoint + "/{id}";
            public const string StatusAppointmentEndpoint = AppointmentEndpoint + "/status";
            public const string CancelAppointmentEndpoint = AppointmentEndpoint + "/cancel";
        }

        public static class RetailerCart
        {
            public const string RetailerCartEndpoint = RetailerApiEndpoint + "/{id}/carts";
        }

        public static class Certificate
        {
            public const string GardenerCertificatesEndpoint = GardenerApiEndpoint + "/{id}/certificates";
            public const string CertificatesEndpoint = ApiEndpoint + "/certificates";
            public const string CertificateEndpoint = CertificatesEndpoint + "/{id}";
        }

        public static class ChatMessage
        {
            public const string ChatMessagesEndpoint = ApiEndpoint + "/chat-message";
        }

        public static class Favorite
        {
            public const string RetailerFavPostsEndpoint = RetailerApiEndpoint + "/{id}/fav-posts";
            public const string RetailerFavPostEndpoint = RetailerFavPostsEndpoint + "/{postId}";
        }

        public static class Notification
        {
            public const string NotificationsEndpoint = ApiEndpoint + "/notifications";
            public const string NotificationEndpoint = NotificationsEndpoint + "/{id}";
        }

        public static class Order
        {
            public const string OrdersEndpoint = ApiEndpoint + "/orders";
            public const string OrderEndpoint = OrdersEndpoint + "/{id}";
            public const string OrderShippingCostEndpoint = OrderEndpoint + "/shipping-cost";
            public const string OrderRejectEndpoint = OrderEndpoint + "/order-reject";
            public const string OrderDeatilCheckEndpoint = OrderEndpoint + "/order-details";
            public const string OrderDeliveriesEndpoint = OrderEndpoint + "/order-deliveries";
            public const string OrderDeliveryEndpoint = OrderDeliveriesEndpoint + "/{orderDeliveryId}";
        }

        public static class Post
        {
            public const string PostsEndpoint = ApiEndpoint + "/posts";
            public const string RetailerPostsEndpoint = RetailerApiEndpoint + "/posts";
            public const string PostEndpoint = PostsEndpoint + "/{id}";
            public const string PostStatusEndpoint = PostEndpoint + "/status";

            public const string GardenerPostEndpoint = GardenerApiEndpoint + "/{id}/posts";
        }

        public static class Product
        {
            public const string GardenerProductsEndpoint = GardenerApiEndpoint + "/{gardenerId}/products";
            public const string GardenerProductEndpoint = GardenerProductsEndpoint + "/{id}";
            public const string ProductsEndpoint = ApiEndpoint + "/products";
            public const string ProductEndpoint = ProductsEndpoint + "/{id}";
            public const string ProductStatusEndpoint = ProductEndpoint + "/status";
            public const string ProductReviewsEndpoint = ProductEndpoint + "/reviews";
            public const string ProductPricesEndpoint = ProductEndpoint + "/prices";
            public const string ProductPriceEndpoint = ProductPricesEndpoint + "/{priceId}";
            public const string ProductCertificatesEndpoint = ProductEndpoint + "/product-certificates";
        }

        public static class ProductCategory
        {
            public const string GardenerProductCategoriesEndpoint = GardenerApiEndpoint + "/{id}/product-categories";
            public const string ProductCategoriesEndpoint = ApiEndpoint + "/categories";
            public const string ProductCategoryEndpoint = ProductCategoriesEndpoint + "/{id}";
        }

        public static class Report
        {
            public const string ReportsEndpoint = ApiEndpoint + "/reports";
            public const string UserReportsEndpoint = ApiEndpoint + "/user-reports";
            public const string ReportEndpoint = ReportsEndpoint + "/{id}";
        }

        public static class ServiceFeature
        {
            public const string ServiceFeaturesEndpoint = AdminApiEndpoint + "/service-features";
            public const string ServiceFeatureDetailEndpoint = ServiceFeaturesEndpoint + "/detail";
            public const string DisableServiceFeatureEndpoint = ServiceFeaturesEndpoint + "/disable";
            public const string UpdateServiceFeatureEndpoint = ServiceFeaturesEndpoint + "/update";
        }

        public static class ServicePackageEndpoint
        {
            public const string ServicePackagesEndpoint = AdminApiEndpoint + "/service-packages";
            public const string ServicePackageDetailEndpoint = ServicePackagesEndpoint + "/detail";
            public const string DisableServicePackageEndpoint = ServicePackagesEndpoint + "/disable";
            public const string ActivateServicePackageEndpoint = ServicePackagesEndpoint + "/activate";
        }

        public static class SubscriptionContract
        {
            public const string SubscriptionContractsEndpoint = AdminApiEndpoint + "/subscription-contracts";
            public const string SubscriptionContractDetailEndpoint = SubscriptionContractsEndpoint + "/detail";
        }

        public static class ServicePackageOrder
        {
            public const string ServicePackageOrdersEndpoint = AdminApiEndpoint + "/service-package-orders";
            public const string ServicePackageOrderDetailEndpoint = ServicePackageOrdersEndpoint + "/detail";
        }

        public static class Payment
        {
            public const string GardenerPaymentsEndpoint = GardenerApiEndpoint + "/payments";
            public const string GardenerTestPaymentsEndpoint = GardenerApiEndpoint + "/test-payments";
        }

        public static class Webhook
        {
            public const string StripeWebhookEndpointTest = AdminApiEndpoint + "/webhook/stripe/test-post";
            public const string StripeWebhookEndpoint = AdminApiEndpoint + "/webhook/stripe";
        }

        public static class GardenerEndpoint
        {
            public const string YourCurrentSubscriptionEndpoint = GardenerApiEndpoint + "/your-current-subscription";
            public const string CurrentSubscriptionBenefitEndpoint = GardenerApiEndpoint + "/current-subscription-benefit";
            public const string PaymentsHistoryEndpoint = GardenerApiEndpoint + "/payments-history";
            public const string ContractsHistoryEndpoint = GardenerApiEndpoint + "/contracts-history";
        }

        public static class Review
        {
            // POST /api/v1/retailer/{retailerId}/orders/{orderId}/details/{orderDetailId}/reviews
            public const string CreateForOrderDetail =
                RetailerApiEndpoint + "/{retailerId}/orders/{orderId}/details/{orderDetailId}/reviews";

            // GET  /api/v1/retailer/{retailerId}/orders/{orderId}/details/{orderDetailId}/review
            public const string GetForOrderDetail =
                RetailerApiEndpoint + "/{retailerId}/orders/{orderId}/details/{orderDetailId}/review";

            // GET  /api/v1/products/{productId}/reviews
            // You already have this under Product.ProductReviewsEndpoint
        }
      
        public static class Statistic
        {
            public const string GardenerStatisticEndpoint = GardenerApiEndpoint + "/{id}/statistic";
            public const string GardenerGeneralStatisticEndpoint = GardenerStatisticEndpoint + "/general";
            public const string GardenerYearOrderStatisticEndpoint = GardenerStatisticEndpoint + "/year-orders";
            public const string GardenerUpcommingAppointmentEndpoint = GardenerStatisticEndpoint + "/upcomming-appointment";
        }
    }
}
