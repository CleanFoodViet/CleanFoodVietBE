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

            //Account address endpoints
            public const string AccountAddressesEndpoint = AccountEndpoint + "/addresses";
            public const string AccountAddressEndpoint = AccountAddressesEndpoint + "/{addressId}";

            //Gardener Certificate endpoints
            public const string GardenerCertificatesEndpoint = AccountEndpoint + "/gardener/certificates";
            public const string GardenerCertificateEndpoint = GardenerCertificatesEndpoint + "/{certificateId}";

            //Appointment endpoints
            public const string AccountAppointmentEndpoint = AccountEndpoint + "/appointments";

            //Notification endpoints
            public const string AccountNotificationEndpoint = AccountEndpoint + "/notifications";

            //Favorite post endpoints
            public const string RetailerFavPostsEndpoint = AccountEndpoint + "/retailer/fav-posts";
            public const string RetailerFavPostEndpoint = RetailerFavPostsEndpoint + "/{postId}";
        }

        public static class Appointment
        {
            public const string AppointmentsEndpoint = ApiEndpoint + "/appointments";
            public const string AppointmentEndpoint = AppointmentsEndpoint + "/{id}";
        }

        public static class RetailerCart
        {
            //Retailer cart endpoint
            public const string RetailerCartEndpoint = RetailerApiEndpoint + "/{id}/carts";
        }

        public static class ChatMessage
        {
            public const string ChatMessagesEndpoint = ApiEndpoint + "/chat-message";
        }

        public static class Notification
        {
            public const string NotificationEndpoint = ApiEndpoint + "/notifications";
        }

        public static class Post
        {
            public const string PostsEndpoint = ApiEndpoint + "/posts";
            public const string PostEndpoint = PostsEndpoint + "/{id}";
            public const string PostStatusEndpoint = PostEndpoint + "/status";
        }

        public static class Product
        {
            public const string GardenerProductsEndpoint = GardenerApiEndpoint + "/{gardenerId}/products";
            public const string ProductsEndpoint = ApiEndpoint + "/products";
            public const string ProductEndpoint = ProductsEndpoint + "/{id}";
            public const string ProductPricesEndpoint = ProductEndpoint + "/prices";
        }

        public static class ProductCategory
        {
            public const string ProductCategoriesEndpoint = ApiEndpoint + "/product-categories";
            public const string ProductCategoryEndpoint = ProductCategoriesEndpoint + "/{id}";
        }

        public static class Report
        {
            public const string ReportsEndpoint = ApiEndpoint + "/reports";
            public const string ReportEndpoint = ReportsEndpoint + "/{id}";
        }

        public static class ServiceFeature
        {
            public const string ServiceFeaturesEndpoint = AdminApiEndpoint + "/service-features";
            public const string ServiceFeatureEndpoint = ServiceFeaturesEndpoint + "/{id}";
            public const string DisableServiceFeatureEndpoint = ServiceFeaturesEndpoint + "/disable/{id}";
        }

        public static class ServicePackageEndpoint
        {
            public const string ServicePackagesEndpoint = AdminApiEndpoint + "/service-packages";
            public const string ServicePackageDetailEndpoint = ServicePackagesEndpoint + "/{id}";
            public const string DisableServicePackageEndpoint = ServicePackagesEndpoint + "/disable/{id}";
            public const string ActivateServicePackageEndpoint = ServicePackagesEndpoint + "/activate/{id}";
        }

        public static class SubscriptionContract
        {
            public const string SubscriptionContractsEndpoint = ApiEndpoint + "/subscription-contracts";
            public const string SubscriptionContractEndpoint = SubscriptionContractsEndpoint + "/{id}";
        }

        public static class ServicePackageOrder
        {
            public const string ServicePackageOrdersEndpoint = AdminApiEndpoint + "/service-package-orders";
            public const string ServicePackageOrderEndpoint = ServicePackageOrdersEndpoint + "/{id}";
        }
    }
}
