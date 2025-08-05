using CleanFoodVietAPI.Application.Interfaces;
using CleanFoodVietAPI.Application.Services.Implements;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Implements;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using Stripe.Checkout;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;


namespace CleanFoodVietAPI.Presentation.Extensions
{
    public static class DependencyServices
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<CleanFoodVietDbContext>, UnitOfWork<CleanFoodVietDbContext>>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<CleanFoodVietDbContext>(options => options.UseMySQL(connectionString!));
            return services;
        }

        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.AllowAnyOrigin() // Change domain url of FE for server-side cookie purpose
                    //.AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            #region Service DI
            services.AddScoped<IAccountService, Application.Services.Implements.AccountService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDeliveryService, OrderDeliveryService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IProductService, Application.Services.Implements.ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IProductCertificateServcie, ProductCertificateServcie>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IServiceFeatureService, ServiceFeatureService>();
            services.AddScoped<IServicePackageService, ServicePackageService>();
            services.AddScoped<ISubscriptionContractService, SubscriptionContractService>();
            services.AddScoped<IServicePackageOrderService, ServicePackageOrderService>();
            services.AddScoped<IReviewService, Application.Services.Implements.ReviewService>();
            services.AddScoped<IStatisticService, StatisticService>();
            //services.AddScoped<ISubscriptionReconciler, SubscriptionReconciler>();
            //services.AddScoped<IGardenerRepository, GardenerRepository>();
            //services.AddScoped<ExpireContractsJob>();
            #endregion

            return services;
        }

        public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Clean Food APIs System", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
                });
                options.EnableAnnotations();
            });
            return services;
        }

        public static IServiceCollection AddJwtAuthenticationScheme(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.GetSection("Authentication:Issuer").Value,
                    ValidAudience = config.GetSection("Authentication:Audience").Value,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Prefer the cookie first
                        //if (context.Request.Cookies.TryGetValue("jwt", out var jwt))
                        //{
                        //    context.Token = jwt;
                        //}
                        //// If not found, fall back to header (for Scalar testing)
                        //else if (context.Request.Headers.ContainsKey("Authorization"))
                        //{
                        var token = context.Request.Headers["Authorization"].ToString();
                        if (token.StartsWith("Bearer "))
                        {
                            context.Token = token.Substring("Bearer ".Length);
                        }
                        //}

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        /// <summary>
        /// Configures Stripe.NET with your SecretKey and registers any required Stripe clients.
        /// </summary>
        public static IServiceCollection AddStripeConfiguration(
            this IServiceCollection services,
            IConfiguration config)
        {
            // 1) Read keys
            var stripeSection = config.GetSection("Stripe");
            var secretKey = stripeSection["SecretKey"];
            var publishableKey = stripeSection["PublishableKey"];

            // 2) Configure the static Stripe API key
            StripeConfiguration.ApiKey = secretKey;

            // 3) (Optional) make your publishable key available via IOptions<StripeOptions>
            services.Configure<StripeOptions>(stripeSection);

            // 4) (Optional) register any Stripe SDK clients you want to inject
            services.AddSingleton<SessionService>();
            services.AddSingleton<CustomerService>();
            services.AddSingleton<PaymentIntentService>();
            // … etc.

            return services;
        }
    }
}
