using CleanFoodVietAPI.Application.Services.Implements;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Data.Entities;
using CleanFoodVietAPI.Data.Repositories.Implements;
using CleanFoodVietAPI.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IChatMessageService, ChatMessageService>();
            services.AddScoped<ICertificateService, CertificateService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IServiceFeatureService, ServiceFeatureService>();
            services.AddScoped<IServicePackageService, ServicePackageService>();
            services.AddScoped<ISubscriptionContractService, SubscriptionContractService>();
            services.AddScoped<IServicePackageOrderService, ServicePackageOrderService>();
            #endregion

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
    }
}
