using CleanFoodVietAPI.Presentation.Extensions;
using CleanFoodVietAPI.Presentation.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Core ASP.NET + OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi("v1", opts =>
    opts.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

// 2) App-wide services (pulled from your extension methods)
builder.Services
    .AddCorsConfig()                              // CORS
    .AddDatabase(builder.Configuration)           // EF DbContext
    .AddUnitOfWork()                              // UoW
    .AddJwtAuthenticationScheme(builder.Configuration)  // JWT Auth
    .AddServices(builder.Configuration)           // your business services
    .AddConfigSwagger()                           // SwaggerGen security
    .AddStripeConfiguration(builder.Configuration) // Stripe SDK clients
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// 3) (Optional) Hangfire for local testing only
// if (builder.Environment.IsDevelopment())
// {
//     builder.Services.AddHangfire(…);
//     builder.Services.AddHangfireServer();
// }

var app = builder.Build();

// 4) Global middleware
app.UseMiddleware<GlobalException>();
// (You’ve removed the old ReconcileMiddleware here)

// 5) Standard pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();

// 6) OpenAPI + Swagger UI
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

// 7) Hangfire Dashboard + RecurringJobs (local only)
// if (app.Environment.IsDevelopment())
// {
//     app.UseHangfireDashboard("/hangfire");
//     RecurringJob.AddOrUpdate<ExpireContractsJob>(…);
//     RecurringJob.AddOrUpdate("heartbeat", …);
// }

// 8) MVC endpoints
app.MapControllers();
app.MapScalarApiReference(o =>
    o.WithTitle("Clean Food API")
     .WithTheme(ScalarTheme.BluePlanet)
     .WithDarkMode(true));

// 9) Run!
app.Run();