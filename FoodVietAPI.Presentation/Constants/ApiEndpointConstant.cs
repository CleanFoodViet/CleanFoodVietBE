namespace CleanFoodVietAPI.Presentation.Constants
{
    public static class ApiEndpointConstant
    {
        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public const string AdminApiEndpoint = ApiEndpoint + "/admin";

        public static class Authentication
        {
            public const string AuthEndpoint = ApiEndpoint + "/auth";
            public const string LoginEndpoint = ApiEndpoint + "/auth";
            public const string RegisterEndpoint = ApiEndpoint + "/auth";
        }

        public static class Account
        {
            public const string AccountsEndpoint = ApiEndpoint + "/accounts";
            public const string AccountEndpoint = AccountsEndpoint + "/{id}";
            public const string AccountProfileEndpoint = AccountEndpoint + "/profile";
        }

        public static class ProductCategory
        {
            public const string ProductCategoriesEndpoint = ApiEndpoint + "/product-categories";
            public const string ProductCategoryEndpoint = ProductCategoriesEndpoint + "/{id}";
        }

        public static class ServiceFeature
        {
            public const string ServiceFeaturesEndpoint = AdminApiEndpoint + "/service-features";
            public const string ServiceFeatureEndpoint = ServiceFeaturesEndpoint + "/{id}";
        }
    }
}
